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
