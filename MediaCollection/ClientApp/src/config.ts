import { useSyncExternalStore } from 'react';

export type AppConfig = {
  readOnly?: boolean;
};

export function getConfig(): AppConfig {
  const w = window as unknown as { __APP_CONFIG__?: AppConfig };
  return w.__APP_CONFIG__ ?? {};
}

let readOnlyState = !!getConfig().readOnly;
const listeners = new Set<() => void>();

function subscribe(listener: () => void): () => void {
  listeners.add(listener);
  return () => listeners.delete(listener);
}

function getSnapshot(): boolean {
  return readOnlyState;
}

export function setReadOnly(value: boolean): void {
  if (readOnlyState === value) return;
  readOnlyState = value;
  listeners.forEach((l) => l());
}

export function isReadOnly(): boolean {
  return readOnlyState;
}

export function useIsReadOnly(): boolean {
  return useSyncExternalStore(subscribe, getSnapshot);
}
