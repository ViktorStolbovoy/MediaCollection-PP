import { FormEvent, useEffect, useRef, useState } from 'react';

type Props = {
  onCancel: () => void;
  onSuccess: () => void;
};

export function LoginModal({ onCancel, onSuccess }: Props) {
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [busy, setBusy] = useState(false);
  const inputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    inputRef.current?.focus();
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onCancel();
    };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [onCancel]);

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    if (busy) return;
    setBusy(true);
    setError(null);
    try {
      const res = await fetch('/api/auth/login', {
        method: 'POST',
        credentials: 'same-origin',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ password }),
      });
      if (res.ok) {
        onSuccess();
        return;
      }
      if (res.status === 401) {
        setError('Invalid password.');
      } else {
        let msg = `Login failed (${res.status}).`;
        try {
          const body = await res.json();
          if (body && typeof body.error === 'string') msg = body.error;
        } catch {
          /* ignore */
        }
        setError(msg);
      }
    } catch (err) {
      setError((err as Error)?.message ?? 'Network error.');
    } finally {
      setBusy(false);
    }
  };

  return (
    <div className="mc-modal-backdrop" onMouseDown={onCancel}>
      <div className="mc-modal" onMouseDown={(e) => e.stopPropagation()}>
        <form onSubmit={submit}>
          <h2 className="mc-modal-title">Enter configuration password</h2>
          <p className="mc-modal-help">
            Authentication is required to make changes to the collection.
          </p>
          <label className="mc-modal-label" htmlFor="mc-login-password">Password</label>
          <input
            id="mc-login-password"
            ref={inputRef}
            type="password"
            autoComplete="current-password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            disabled={busy}
          />
          {error && <div className="mc-modal-error">{error}</div>}
          <div className="mc-modal-actions">
            <button type="button" onClick={onCancel} disabled={busy}>Cancel</button>
            <button type="submit" disabled={busy || password.length === 0}>
              {busy ? 'Signing in…' : 'Sign in'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
