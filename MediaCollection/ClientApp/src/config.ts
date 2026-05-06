export type AppConfig = {
  readOnly?: boolean;
};

export function getConfig(): AppConfig {
  const w = window as unknown as { __APP_CONFIG__?: AppConfig };
  return w.__APP_CONFIG__ ?? {};
}

export function isReadOnly(): boolean {
  return !!getConfig().readOnly;
}
