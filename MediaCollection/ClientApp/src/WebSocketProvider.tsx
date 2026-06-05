import { useEffect } from 'react';
import { setReadOnly } from './config';
import { sendPing, startWebSocket, subscribe } from './websocket';

export function WebSocketProvider({ children }: { children: React.ReactNode }) {
  useEffect(() => {
    const stop = startWebSocket();
    const unsubAuth = subscribe('auth-changed', (data) => {
      const d = data as { authenticated?: boolean } | undefined;
      if (typeof d?.authenticated === 'boolean') setReadOnly(!d.authenticated);
    });
    const ping = setInterval(sendPing, 60_000);
    return () => {
      clearInterval(ping);
      unsubAuth();
      stop();
    };
  }, []);

  return <>{children}</>;
}
