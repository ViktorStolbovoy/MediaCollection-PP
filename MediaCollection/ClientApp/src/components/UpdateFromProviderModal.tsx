import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { request } from '../websocket';

interface TmdbToolTitle {
  Id: number;
  TitleName: string;
  Year: number;
  Description: string;
  ImdbId: string;
  Kind: number;
}

interface TmdbToolInitResponse {
  Titles: TmdbToolTitle[];
}

interface TmdbToolSearchResult {
  TmdbId: number;
  IsTv: boolean;
  Title: string;
  Overview: string;
  PosterPath: string;
  ReleaseYear: number;
  PosterMimeType: string | null;
  PosterBase64: string | null;
}

interface TmdbToolSearchResponse {
  Results: TmdbToolSearchResult[];
}

interface TmdbToolApplyResponse {
  Title: TmdbToolTitle;
  ImageId: number | null;
}

interface TmdbToolSaveManualResponse {
  Title: TmdbToolTitle;
}

type Props = {
  onClose: () => void;
};

const TV_KINDS = new Set<number>([1, 2, 7]);

function isTvKind(kind: number): boolean {
  return TV_KINDS.has(kind);
}

function posterDataUrl(r: TmdbToolSearchResult): string | null {
  if (!r.PosterBase64) return null;
  const mime = r.PosterMimeType || 'image/jpeg';
  return `data:${mime};base64,${r.PosterBase64}`;
}

function formatResultLabel(r: TmdbToolSearchResult): string {
  if (r.ReleaseYear) return `${r.Title || '(untitled)'} (${r.ReleaseYear})`;
  return r.Title || '(untitled)';
}

export function UpdateFromProviderModal({ onClose }: Props) {
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [status, setStatus] = useState<string | null>(null);

  const [titles, setTitles] = useState<TmdbToolTitle[]>([]);
  const [index, setIndex] = useState(0);

  const [name, setName] = useState('');
  const [year, setYear] = useState<number>(0);
  const [description, setDescription] = useState('');
  const [isTv, setIsTv] = useState(false);

  const [overrideTitle, setOverrideTitle] = useState(true);
  const [overrideYear, setOverrideYear] = useState(true);
  const [overrideDescription, setOverrideDescription] = useState(true);

  const [results, setResults] = useState<TmdbToolSearchResult[]>([]);
  const [hasSearched, setHasSearched] = useState(false);

  // Queue of title ids the user marked for later inspection. The desktop app
  // keeps an in-memory list (TitlesForInspection); we surface it inline so the
  // user can navigate back into the queue and revisit those titles.
  const [inspectionIds, setInspectionIds] = useState<number[]>([]);

  const closedRef = useRef(false);

  const currentTitle = titles[index] ?? null;
  const totalTitles = titles.length;

  const populateFromTitle = useCallback((t: TmdbToolTitle) => {
    setName(t.TitleName ?? '');
    setYear(t.Year ?? 0);
    setDescription(t.Description ?? '');
    setIsTv(isTvKind(t.Kind));
    setResults([]);
    setHasSearched(false);
    setError(null);
    setStatus(null);
  }, []);

  // Load queue.
  useEffect(() => {
    let cancelled = false;
    setBusy(true);
    setError(null);
    request<TmdbToolInitResponse>('tmdb-tool-init', {})
      .then((res) => {
        if (cancelled) return;
        const list = res.Titles ?? [];
        setTitles(list);
        setIndex(0);
        if (list.length > 0) {
          populateFromTitle(list[0]);
        } else {
          setStatus('No titles need automatic updating.');
        }
      })
      .catch((err) => {
        if (!cancelled) setError(String((err as Error)?.message ?? err));
      })
      .finally(() => {
        if (!cancelled) setBusy(false);
      });
    return () => {
      cancelled = true;
    };
  }, [populateFromTitle]);

  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [onClose]);

  const goToIndex = useCallback(
    (next: number) => {
      if (next < 0 || next >= titles.length) return;
      setIndex(next);
      const t = titles[next];
      if (t) populateFromTitle(t);
    },
    [titles, populateFromTitle]
  );

  const goNext = useCallback(() => {
    if (index + 1 < titles.length) goToIndex(index + 1);
  }, [index, titles.length, goToIndex]);

  const goPrev = useCallback(() => {
    if (index > 0) goToIndex(index - 1);
  }, [index, goToIndex]);

  const runSearch = useCallback(
    async (query: string, tv: boolean) => {
      const q = (query ?? '').trim();
      if (!q) return;
      if (closedRef.current) return;
      setBusy(true);
      setError(null);
      setStatus(null);
      try {
        const res = await request<TmdbToolSearchResponse>(
          'tmdb-tool-search',
          { Query: q, IsTv: tv },
          { timeoutMs: 60000 }
        );
        setResults(res.Results ?? []);
        setHasSearched(true);
      } catch (err) {
        setError(String((err as Error)?.message ?? err));
        setResults([]);
        setHasSearched(true);
      } finally {
        setBusy(false);
      }
    },
    []
  );

  // Auto-search when the current title changes (mirrors the desktop's
  // PopulateUI -> Search() behaviour).
  useEffect(() => {
    if (!currentTitle) return;
    runSearch(currentTitle.TitleName ?? '', isTvKind(currentTitle.Kind));
    // We deliberately do not depend on `name`/`isTv` so editing those fields
    // does not re-fire the search; the user uses the explicit Search button.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentTitle?.Id]);

  const updateLocalTitle = useCallback((updated: TmdbToolTitle) => {
    setTitles((prev) => prev.map((t) => (t.Id === updated.Id ? updated : t)));
  }, []);

  const applyResult = useCallback(
    async (r: TmdbToolSearchResult) => {
      if (!currentTitle) return;
      setBusy(true);
      setError(null);
      setStatus(null);
      try {
        const res = await request<TmdbToolApplyResponse>(
          'tmdb-tool-apply',
          {
            TitleId: currentTitle.Id,
            TmdbId: r.TmdbId,
            IsTv: r.IsTv,
            OverrideTitle: overrideTitle,
            OverrideDescription: overrideDescription,
            OverrideYear: overrideYear,
          },
          { timeoutMs: 60000 }
        );
        if (res.Title) updateLocalTitle(res.Title);
        setStatus(`Applied "${r.Title}" to "${res.Title?.TitleName ?? currentTitle.TitleName}".`);
        if (index + 1 < titles.length) {
          goToIndex(index + 1);
        }
      } catch (err) {
        setError(String((err as Error)?.message ?? err));
      } finally {
        setBusy(false);
      }
    },
    [currentTitle, overrideTitle, overrideDescription, overrideYear, index, titles.length, goToIndex, updateLocalTitle]
  );

  const saveManual = useCallback(async () => {
    if (!currentTitle) return;
    setBusy(true);
    setError(null);
    setStatus(null);
    try {
      const res = await request<TmdbToolSaveManualResponse>(
        'tmdb-tool-save-manual',
        {
          TitleId: currentTitle.Id,
          Description: description,
          Year: Number.isFinite(year) ? year : 0,
        }
      );
      if (res.Title) updateLocalTitle(res.Title);
      setStatus(`Saved manual edit for "${res.Title?.TitleName ?? currentTitle.TitleName}".`);
      if (index + 1 < titles.length) goToIndex(index + 1);
    } catch (err) {
      setError(String((err as Error)?.message ?? err));
    } finally {
      setBusy(false);
    }
  }, [currentTitle, description, year, index, titles.length, goToIndex, updateLocalTitle]);

  const saveForInspection = useCallback(() => {
    if (!currentTitle) return;
    setInspectionIds((prev) => (prev.includes(currentTitle.Id) ? prev : [...prev, currentTitle.Id]));
    setStatus(`Marked "${currentTitle.TitleName}" for inspection.`);
    if (index + 1 < titles.length) goToIndex(index + 1);
  }, [currentTitle, index, titles.length, goToIndex]);

  const inspectionTitles = useMemo(
    () => inspectionIds.map((id) => titles.find((t) => t.Id === id)).filter((t): t is TmdbToolTitle => !!t),
    [inspectionIds, titles]
  );

  const close = useCallback(() => {
    closedRef.current = true;
    onClose();
  }, [onClose]);

  return (
    <div className="mc-modal-backdrop" onMouseDown={close}>
      <div
        className="mc-modal mc-modal-wide mc-tmdb-tool"
        onMouseDown={(e) => e.stopPropagation()}
      >
        <div className="mc-seasons-tool-header">
          <h2 className="mc-modal-title">Update From TMDB</h2>
          <button type="button" className="mc-seasons-tool-close" onClick={close} aria-label="Close">
            ✕
          </button>
        </div>

        <div className="mc-tmdb-tool-toolbar">
          <button type="button" onClick={goPrev} disabled={busy || index <= 0}>
            ◀ Previous
          </button>
          <span className="mc-muted">
            {totalTitles === 0
              ? 'No titles in queue'
              : `Title ${index + 1} of ${totalTitles}`}
          </span>
          <button type="button" onClick={goNext} disabled={busy || index + 1 >= totalTitles}>
            Next ▶
          </button>
        </div>

        {currentTitle ? (
          <>
            <fieldset className="mc-seasons-tool-section">
              <legend>Search</legend>
              <div className="mc-stack">
                <label htmlFor="mc-tmdb-name" className="mc-seasons-tool-label">
                  Name
                </label>
                <input
                  id="mc-tmdb-name"
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                />
                <button type="button" onClick={() => runSearch(name, isTv)} disabled={busy}>
                  Search
                </button>
              </div>
              <div className="mc-stack">
                <span className="mc-seasons-tool-label">Kind:</span>
                <label>
                  <input
                    type="radio"
                    checked={!isTv}
                    onChange={() => setIsTv(false)}
                  />{' '}
                  Movie
                </label>
                <label>
                  <input
                    type="radio"
                    checked={isTv}
                    onChange={() => setIsTv(true)}
                  />{' '}
                  TV
                </label>
                <span className="mc-tmdb-tool-spacer" />
                <span className="mc-seasons-tool-label">Override on apply:</span>
                <label>
                  <input
                    type="checkbox"
                    checked={overrideTitle}
                    onChange={(e) => setOverrideTitle(e.target.checked)}
                  />{' '}
                  Name
                </label>
                <label>
                  <input
                    type="checkbox"
                    checked={overrideYear}
                    onChange={(e) => setOverrideYear(e.target.checked)}
                  />{' '}
                  Year
                </label>
                <label>
                  <input
                    type="checkbox"
                    checked={overrideDescription}
                    onChange={(e) => setOverrideDescription(e.target.checked)}
                  />{' '}
                  Description
                </label>
              </div>
            </fieldset>

            <fieldset className="mc-seasons-tool-section">
              <legend>Results</legend>
              <div className="mc-tmdb-tool-results-wrap">
                {busy && results.length === 0 && (
                  <div className="mc-muted">Searching TMDB…</div>
                )}
                {!busy && hasSearched && results.length === 0 && (
                  <div className="mc-muted">No results.</div>
                )}
                <ul className="mc-tmdb-tool-grid">
                  {results.map((r) => {
                    const url = posterDataUrl(r);
                    return (
                      <li key={`${r.TmdbId}-${r.IsTv}`} className="mc-tmdb-tool-result">
                        <button
                          type="button"
                          className="mc-tmdb-tool-result-btn"
                          onClick={() => applyResult(r)}
                          disabled={busy}
                          title={r.Overview || formatResultLabel(r)}
                        >
                          {url ? (
                            <img src={url} alt={r.Title || 'poster'} />
                          ) : (
                            <div className="mc-tmdb-tool-poster-fallback">No poster</div>
                          )}
                          <span className="mc-tmdb-tool-result-label">{formatResultLabel(r)}</span>
                        </button>
                      </li>
                    );
                  })}
                </ul>
              </div>
            </fieldset>

            <fieldset className="mc-seasons-tool-section">
              <legend>Manual Entry</legend>
              <div className="mc-stack">
                <label htmlFor="mc-tmdb-year" className="mc-seasons-tool-label">
                  Year
                </label>
                <input
                  id="mc-tmdb-year"
                  type="number"
                  className="mc-tmdb-tool-year"
                  value={year || ''}
                  onChange={(e) => setYear(Number(e.target.value) || 0)}
                />
              </div>
              <div className="mc-stack">
                <label htmlFor="mc-tmdb-description" className="mc-seasons-tool-label">
                  Description
                </label>
                <textarea
                  id="mc-tmdb-description"
                  className="mc-tmdb-tool-description"
                  rows={3}
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                />
              </div>
              <div className="mc-stack">
                <button type="button" onClick={saveManual} disabled={busy}>
                  Save Manual Entry
                </button>
                <button type="button" onClick={saveForInspection} disabled={busy}>
                  Save For Inspection
                </button>
              </div>
            </fieldset>

            {inspectionTitles.length > 0 && (
              <fieldset className="mc-seasons-tool-section">
                <legend>For Inspection</legend>
                <ul className="mc-tmdb-tool-inspection">
                  {inspectionTitles.map((t) => {
                    const idx = titles.findIndex((x) => x.Id === t.Id);
                    return (
                      <li key={t.Id}>
                        <button
                          type="button"
                          className="mc-tmdb-tool-link"
                          onClick={() => idx >= 0 && goToIndex(idx)}
                          disabled={busy || idx < 0}
                        >
                          {t.TitleName || `#${t.Id}`}
                        </button>
                      </li>
                    );
                  })}
                </ul>
              </fieldset>
            )}
          </>
        ) : (
          !busy && <p className="mc-muted">{status ?? 'No titles need automatic updating.'}</p>
        )}

        {error && <div className="mc-modal-error">{error}</div>}
        {status && !error && <div className="mc-muted mc-seasons-tool-status">{status}</div>}

        <div className="mc-modal-actions">
          <button type="button" onClick={close} disabled={busy}>
            Close
          </button>
        </div>
      </div>
    </div>
  );
}
