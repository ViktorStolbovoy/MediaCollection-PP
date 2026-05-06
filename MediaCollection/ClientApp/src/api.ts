async function parseError(res: Response): Promise<string> {
  const t = await res.text();
  try {
    const j = JSON.parse(t);
    return j.error ?? t;
  } catch {
    return t || res.statusText;
  }
}

export async function apiJson<T>(path: string, init?: RequestInit): Promise<T> {
  const headers: Record<string, string> = {
    Accept: 'application/json',
    ...(init?.headers as Record<string, string>),
  };
  if (init?.body && !(init.body instanceof FormData) && !headers['Content-Type'])
    headers['Content-Type'] = 'application/json';

  const res = await fetch(path, { ...init, headers });
  if (!res.ok) throw new Error(await parseError(res));
  if (res.status === 204) return undefined as T;
  return (await res.json()) as T;
}

export async function uploadTitleImage(titleId: number, file: File): Promise<unknown> {
  const fd = new FormData();
  fd.append('file', file);
  const res = await fetch(`/api/titles/${titleId}/images`, { method: 'POST', body: fd });
  if (!res.ok) throw new Error(await parseError(res));
  return res.json();
}
