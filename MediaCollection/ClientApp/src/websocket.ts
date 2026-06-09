export type WsMessage = {
  type: string;
  data?: unknown;
};

type Listener = (data: unknown) => void;

const listeners = new Map<string, Set<Listener>>();
const pendingOutbox: WsMessage[] = [];
const stickyMessages = new Map<string, WsMessage>();

let socket: WebSocket | null = null;
let reconnectTimer: ReturnType<typeof setTimeout> | undefined;
let started = false;

function wsUrl(): string {
  const proto = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
  return `${proto}//${window.location.host}/ws`;
}

function dispatch(msg: WsMessage): void {
  const set = listeners.get(msg.type);
  if (!set) return;
  set.forEach((fn) => fn(msg.data));
}

function scheduleReconnect(): void {
  if (!started) return;
  clearTimeout(reconnectTimer);
  reconnectTimer = setTimeout(connect, 3000);
}

function flushOutbox(): void {
  if (socket?.readyState !== WebSocket.OPEN) return;
  for (const msg of stickyMessages.values()) {
    socket.send(JSON.stringify(msg));
  }
  while (pendingOutbox.length) {
    const msg = pendingOutbox.shift()!;
    socket.send(JSON.stringify(msg));
  }
}

function connect(): void {
  if (!started) return;
  if (socket?.readyState === WebSocket.OPEN || socket?.readyState === WebSocket.CONNECTING) return;

  socket = new WebSocket(wsUrl());

  socket.onopen = () => {
    clearTimeout(reconnectTimer);
    flushOutbox();
  };

  socket.onmessage = (ev) => {
    try {
      const msg = JSON.parse(String(ev.data)) as WsMessage;
      if (msg?.type) dispatch(msg);
    } catch {
      /* ignore malformed payloads */
    }
  };

  socket.onclose = () => {
    socket = null;
    scheduleReconnect();
  };

  socket.onerror = () => {
    socket?.close();
  };
}

export function startWebSocket(): () => void {
  started = true;
  connect();
  return () => {
    started = false;
    clearTimeout(reconnectTimer);
    reconnectTimer = undefined;
    socket?.close();
    socket = null;
  };
}

export function subscribe(type: string, listener: Listener): () => void {
  let set = listeners.get(type);
  if (!set) {
    set = new Set();
    listeners.set(type, set);
  }
  set.add(listener);
  return () => {
    set!.delete(listener);
    if (set!.size === 0) listeners.delete(type);
  };
}

/**
 * Sends a message to the server. When the socket is offline, the message is queued and
 * flushed once the connection is (re)established. Pass `sticky: true` for messages that
 * should be re-sent automatically on every reconnect (such as live subscriptions).
 */
export function sendMessage(type: string, data?: unknown, opts?: { sticky?: boolean }): void {
  const msg: WsMessage = data === undefined ? { type } : { type, data };
  if (opts?.sticky) {
    stickyMessages.set(type, msg);
  }
  if (socket?.readyState === WebSocket.OPEN) {
    socket.send(JSON.stringify(msg));
  } else if (!opts?.sticky) {
    pendingOutbox.push(msg);
  }
}

export function clearStickyMessage(type: string): void {
  stickyMessages.delete(type);
}

export function sendPing(): void {
  if (socket?.readyState !== WebSocket.OPEN) return;
  socket.send(JSON.stringify({ type: 'ping' }));
}

type PendingRequest = {
  resolve: (data: unknown) => void;
  reject: (err: unknown) => void;
  timer: ReturnType<typeof setTimeout> | undefined;
};

const pendingRequests = new Map<string, PendingRequest>();
const wiredResponseTypes = new Set<string>();
let nextRequestSeq = 0;

function ensureResponseListener(responseType: string): void {
  if (wiredResponseTypes.has(responseType)) return;
  wiredResponseTypes.add(responseType);
  subscribe(responseType, (raw) => {
    const payload = raw as { RequestId?: string; Error?: string } | undefined;
    const id = payload?.RequestId;
    if (!id) return;
    const pending = pendingRequests.get(id);
    if (!pending) return;
    pendingRequests.delete(id);
    if (pending.timer) clearTimeout(pending.timer);
    if (payload?.Error) {
      pending.reject(new Error(payload.Error));
    } else {
      pending.resolve(payload);
    }
  });
}

/**
 * Sends a request-style message and resolves once the server replies with a
 * matching `{type}-response` payload (correlated by `RequestId`). The server
 * indicates failure by including an `Error` string on the response.
 */
export function request<T>(
  type: string,
  data?: Record<string, unknown>,
  opts?: { timeoutMs?: number }
): Promise<T> {
  const responseType = `${type}-response`;
  ensureResponseListener(responseType);
  const requestId = `r-${Date.now().toString(36)}-${++nextRequestSeq}`;
  const timeoutMs = opts?.timeoutMs ?? 30000;
  return new Promise<T>((resolve, reject) => {
    const timer =
      timeoutMs > 0
        ? setTimeout(() => {
            pendingRequests.delete(requestId);
            reject(new Error(`Request "${type}" timed out after ${timeoutMs}ms`));
          }, timeoutMs)
        : undefined;
    pendingRequests.set(requestId, {
      resolve: (d) => resolve(d as T),
      reject,
      timer,
    });
    sendMessage(type, { ...(data ?? {}), RequestId: requestId });
  });
}
