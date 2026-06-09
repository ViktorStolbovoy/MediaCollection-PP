import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { request } from '../websocket';

interface SeasonsToolItem {
  Id: number;
  TitleName: string;
  Kind: number;
  Season: number;
  Disk: number;
  EpisodeOrTrack: number;
  ParentTitleId: number | null;
}

interface SeasonsToolInitResponse {
  SeriesName: string;
  SeriesId: number | null;
  IsSeriesTitle: boolean;
}

interface SeasonsToolSearchResponse {
  SeriesId: number | null;
  Seasons: SeasonsToolItem[];
  Items: SeasonsToolItem[];
  SeasonsToCreate: number[];
}

interface SeasonsToolCreateSeriesResponse {
  Series: SeasonsToolItem;
}

interface SeasonsToolCreateSeasonsResponse {
  Created: SeasonsToolItem[];
}

interface SeasonsToolAutoMoveResponse {
  MovedSeasons: number;
  MovedItems: number;
}

interface SeasonsToolConvertItem {
  Id: number;
  TitleName: string;
  Kind: number;
  Season: number;
  Disk: number;
  Episode: number;
}

interface SeasonsToolFindConvertResponse {
  Titles: SeasonsToolConvertItem[];
}

interface SeasonsToolApplyConvertResponse {
  Updated: number;
}

type Props = {
  titleId: number;
  onClose: () => void;
};

const DEFAULT_REGEX = 'S (?<s>\\d{2}) E (?<e>\\d{2})';

function parseSeasonNumbers(input: string): number[] {
  const out: number[] = [];
  if (!input) return out;
  for (const piece of input.split(/[,;]/)) {
    const trimmed = piece.trim();
    if (!trimmed) continue;
    const range = trimmed.split('-').map((t) => t.trim());
    if (range.length === 1) {
      const n = Number(range[0]);
      if (Number.isFinite(n) && n > 0) out.push(n);
    } else if (range.length === 2) {
      const a = Number(range[0]);
      const b = Number(range[1]);
      if (Number.isFinite(a) && Number.isFinite(b)) {
        for (let i = Math.min(a, b); i <= Math.max(a, b); i++) {
          if (i > 0) out.push(i);
        }
      }
    }
  }
  return Array.from(new Set(out)).sort((x, y) => x - y);
}

function formatSeasonItem(t: SeasonsToolItem): string {
  return t.TitleName + (t.ParentTitleId ? '' : '  (top)');
}

function formatConvertItem(t: SeasonsToolConvertItem): string {
  const parts = [`S${t.Season}`];
  if (t.Disk) parts.push(`D${t.Disk}`);
  if (t.Episode) parts.push(`E${t.Episode}`);
  return `${t.TitleName}  →  ${parts.join(' ')}`;
}

export function SeasonsToolModal({ titleId, onClose }: Props) {
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [status, setStatus] = useState<string | null>(null);

  const [seriesName, setSeriesName] = useState('');
  const [seriesId, setSeriesId] = useState<number | null>(null);
  const [seriesIsExistingTitle, setSeriesIsExistingTitle] = useState(false);

  const [pattern, setPattern] = useState('');
  const [seasons, setSeasons] = useState<SeasonsToolItem[]>([]);
  const [items, setItems] = useState<SeasonsToolItem[]>([]);
  const [seasonChecks, setSeasonChecks] = useState<Record<number, boolean>>({});
  const [itemChecks, setItemChecks] = useState<Record<number, boolean>>({});
  const [seasonsToCreate, setSeasonsToCreate] = useState('');

  const [regexText, setRegexText] = useState(DEFAULT_REGEX);
  const [convertMode, setConvertMode] = useState<'find' | 'convert'>('find');
  const [convertItems, setConvertItems] = useState<SeasonsToolConvertItem[]>([]);
  const [convertChecks, setConvertChecks] = useState<Record<number, boolean>>({});

  const closedRef = useRef(false);

  useEffect(() => {
    let cancelled = false;
    setBusy(true);
    setError(null);
    request<SeasonsToolInitResponse>('season-tool-init', { TitleId: titleId })
      .then((res) => {
        if (cancelled) return;
        setSeriesName(res.SeriesName ?? '');
        setSeriesId(res.SeriesId ?? null);
        setSeriesIsExistingTitle(!!res.IsSeriesTitle);
        const seriesText = (res.SeriesName ?? '').trim();
        setPattern(seriesText ? `${seriesText} %` : '');
      })
      .catch((err) => {
        if (!cancelled) setError(String(err?.message ?? err));
      })
      .finally(() => {
        if (!cancelled) setBusy(false);
      });
    return () => {
      cancelled = true;
    };
  }, [titleId]);

  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', onKey);
    return () => window.removeEventListener('keydown', onKey);
  }, [onClose]);

  const seriesCreateDisabled = useMemo(
    () => seriesIsExistingTitle || seriesId != null || !seriesName.trim() || busy,
    [seriesIsExistingTitle, seriesId, seriesName, busy]
  );

  const seasonsToCreateNumbers = useMemo(() => parseSeasonNumbers(seasonsToCreate), [seasonsToCreate]);

  const runSearch = useCallback(async () => {
    if (closedRef.current) return;
    setBusy(true);
    setError(null);
    setStatus(null);
    try {
      const res = await request<SeasonsToolSearchResponse>('season-tool-search', { Pattern: pattern });
      setSeasons(res.Seasons ?? []);
      setItems(res.Items ?? []);
      const sc: Record<number, boolean> = {};
      for (const s of res.Seasons ?? []) sc[s.Id] = s.ParentTitleId == null;
      setSeasonChecks(sc);
      const ic: Record<number, boolean> = {};
      for (const it of res.Items ?? []) ic[it.Id] = it.ParentTitleId == null;
      setItemChecks(ic);
      setSeasonsToCreate((res.SeasonsToCreate ?? []).join(', '));
      if (res.SeriesId) {
        setSeriesId(res.SeriesId);
      }
    } catch (err) {
      setError(String((err as Error)?.message ?? err));
    } finally {
      setBusy(false);
    }
  }, [pattern]);

  const createSeries = useCallback(async () => {
    setBusy(true);
    setError(null);
    setStatus(null);
    try {
      const res = await request<SeasonsToolCreateSeriesResponse>('season-tool-create-series', {
        Name: seriesName.trim(),
      });
      setSeriesId(res.Series?.Id ?? null);
      setStatus(`Created series "${res.Series?.TitleName}"`);
    } catch (err) {
      setError(String((err as Error)?.message ?? err));
    } finally {
      setBusy(false);
    }
  }, [seriesName]);

  const createSeasons = useCallback(async () => {
    if (!seriesId) {
      setError('Need to create series first');
      return;
    }
    const nums = seasonsToCreateNumbers;
    if (!nums.length) return;
    setBusy(true);
    setError(null);
    setStatus(null);
    try {
      const res = await request<SeasonsToolCreateSeasonsResponse>('season-tool-create-seasons', {
        SeriesId: seriesId,
        Pattern: pattern,
        Seasons: nums,
      });
      setStatus(`Created ${res.Created?.length ?? 0} season(s)`);
      setSeasonsToCreate('');
      await runSearch();
    } catch (err) {
      setError(String((err as Error)?.message ?? err));
    } finally {
      setBusy(false);
    }
  }, [seriesId, pattern, seasonsToCreateNumbers, runSearch]);

  const autoMove = useCallback(async () => {
    if (!seriesId) {
      setError('Need to create series first');
      return;
    }
    const seasonIds = seasons.filter((s) => seasonChecks[s.Id]).map((s) => s.Id);
    const itemIds = items.filter((i) => itemChecks[i.Id]).map((i) => i.Id);
    setBusy(true);
    setError(null);
    setStatus(null);
    try {
      const res = await request<SeasonsToolAutoMoveResponse>('season-tool-auto-move', {
        SeriesId: seriesId,
        SeasonIds: seasonIds,
        ItemIds: itemIds,
      });
      setStatus(`Reparented ${res.MovedSeasons} season(s) and ${res.MovedItems} item(s).`);
      await runSearch();
    } catch (err) {
      setError(String((err as Error)?.message ?? err));
    } finally {
      setBusy(false);
    }
  }, [seriesId, seasons, items, seasonChecks, itemChecks, runSearch]);

  const findTitlesToConvert = useCallback(async () => {
    setBusy(true);
    setError(null);
    setStatus(null);
    try {
      const res = await request<SeasonsToolFindConvertResponse>('season-tool-find-titles-to-convert', {
        Pattern: pattern,
        Regex: regexText,
      });
      const titles = res.Titles ?? [];
      setConvertItems(titles);
      const c: Record<number, boolean> = {};
      for (const t of titles) c[t.Id] = true;
      setConvertChecks(c);
      setConvertMode(titles.length > 0 ? 'convert' : 'find');
      setStatus(`Found ${titles.length} title(s) matching the regex.`);
    } catch (err) {
      setError(String((err as Error)?.message ?? err));
    } finally {
      setBusy(false);
    }
  }, [pattern, regexText]);

  const applyConversion = useCallback(async () => {
    const toApply = convertItems.filter((c) => convertChecks[c.Id]);
    if (!toApply.length) {
      setConvertItems([]);
      setConvertChecks({});
      setConvertMode('find');
      return;
    }
    setBusy(true);
    setError(null);
    setStatus(null);
    try {
      const res = await request<SeasonsToolApplyConvertResponse>('season-tool-apply-conversion', {
        Titles: toApply.map((t) => ({
          Id: t.Id,
          Kind: t.Kind,
          Season: t.Season,
          Disk: t.Disk,
          Episode: t.Episode,
        })),
      });
      setStatus(`Converted ${res.Updated} title(s).`);
      setConvertItems([]);
      setConvertChecks({});
      setConvertMode('find');
      await runSearch();
    } catch (err) {
      setError(String((err as Error)?.message ?? err));
    } finally {
      setBusy(false);
    }
  }, [convertItems, convertChecks, runSearch]);

  const onRecategoriseClick = useCallback(() => {
    if (convertMode === 'convert') {
      applyConversion();
    } else {
      findTitlesToConvert();
    }
  }, [convertMode, applyConversion, findTitlesToConvert]);

  const close = useCallback(() => {
    closedRef.current = true;
    onClose();
  }, [onClose]);

  return (
    <div className="mc-modal-backdrop" onMouseDown={close}>
      <div
        className="mc-modal mc-modal-wide mc-seasons-tool"
        onMouseDown={(e) => e.stopPropagation()}
      >
        <div className="mc-seasons-tool-header">
          <h2 className="mc-modal-title">Seasons Tool</h2>
          <button type="button" className="mc-seasons-tool-close" onClick={close} aria-label="Close">
            ✕
          </button>
        </div>

        <fieldset className="mc-seasons-tool-section">
          <legend>Series</legend>
          <div className="mc-stack">
            <label htmlFor="mc-st-series-name" className="mc-seasons-tool-label">
              Name
            </label>
            <input
              id="mc-st-series-name"
              type="text"
              value={seriesName}
              onChange={(e) => {
                setSeriesName(e.target.value);
                setSeriesId(null);
                setSeriesIsExistingTitle(false);
              }}
            />
            <button type="button" onClick={createSeries} disabled={seriesCreateDisabled}>
              Create
            </button>
          </div>
          <small className="mc-muted">
            {seriesId != null
              ? `Series exists (Id ${seriesId}).`
              : 'No existing series detected for this name.'}
          </small>
        </fieldset>

        <fieldset className="mc-seasons-tool-section">
          <legend>Seasons</legend>
          <div className="mc-stack">
            <label htmlFor="mc-st-pattern" className="mc-seasons-tool-label">
              Pattern
            </label>
            <input
              id="mc-st-pattern"
              type="text"
              value={pattern}
              onChange={(e) => setPattern(e.target.value)}
            />
            <button type="button" onClick={runSearch} disabled={busy}>
              Search
            </button>
          </div>
          <div className="mc-seasons-tool-list-wrap">
            <div className="mc-seasons-tool-label">Found:</div>
            <ul className="mc-seasons-tool-list">
              {seasons.length === 0 && <li className="mc-muted">No seasons.</li>}
              {seasons.map((s) => (
                <li key={s.Id}>
                  <label>
                    <input
                      type="checkbox"
                      checked={!!seasonChecks[s.Id]}
                      onChange={(e) =>
                        setSeasonChecks((prev) => ({ ...prev, [s.Id]: e.target.checked }))
                      }
                    />{' '}
                    {formatSeasonItem(s)}
                  </label>
                </li>
              ))}
            </ul>
          </div>
          <div className="mc-stack">
            <label htmlFor="mc-st-create-seasons" className="mc-seasons-tool-label">
              Create:
            </label>
            <input
              id="mc-st-create-seasons"
              type="text"
              value={seasonsToCreate}
              onChange={(e) => setSeasonsToCreate(e.target.value)}
              placeholder="e.g. 1, 2, 4-6"
            />
            <button
              type="button"
              onClick={createSeasons}
              disabled={busy || seriesId == null || seasonsToCreateNumbers.length === 0}
            >
              Create
            </button>
            <button type="button" onClick={autoMove} disabled={busy || seriesId == null}>
              Auto Move
            </button>
          </div>
        </fieldset>

        <fieldset className="mc-seasons-tool-section">
          <legend>Related Discs / Episodes</legend>
          <div className="mc-stack">
            <label htmlFor="mc-st-regex" className="mc-seasons-tool-label">
              Regex:
            </label>
            <input
              id="mc-st-regex"
              type="text"
              value={regexText}
              onChange={(e) => setRegexText(e.target.value)}
            />
            <button type="button" onClick={onRecategoriseClick} disabled={busy}>
              {convertMode === 'convert' ? 'Convert' : 'Recategorise'}
            </button>
          </div>
          <div className="mc-seasons-tool-list-wrap">
            <ul className="mc-seasons-tool-list">
              {convertMode === 'convert' && convertItems.length === 0 && (
                <li className="mc-muted">No matching titles.</li>
              )}
              {convertMode === 'find' &&
                items.map((i) => (
                  <li key={i.Id}>
                    <label>
                      <input
                        type="checkbox"
                        checked={!!itemChecks[i.Id]}
                        onChange={(e) =>
                          setItemChecks((prev) => ({ ...prev, [i.Id]: e.target.checked }))
                        }
                      />{' '}
                      {formatSeasonItem(i)}
                    </label>
                  </li>
                ))}
              {convertMode === 'convert' &&
                convertItems.map((c) => (
                  <li key={c.Id}>
                    <label>
                      <input
                        type="checkbox"
                        checked={!!convertChecks[c.Id]}
                        onChange={(e) =>
                          setConvertChecks((prev) => ({ ...prev, [c.Id]: e.target.checked }))
                        }
                      />{' '}
                      {formatConvertItem(c)}
                    </label>
                  </li>
                ))}
            </ul>
          </div>
        </fieldset>

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
