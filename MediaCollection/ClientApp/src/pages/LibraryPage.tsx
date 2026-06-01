import { FormEvent, useCallback, useEffect, useMemo, useState } from 'react';
import { apiJson } from '../api';
import { TitleDetailForm } from '../components/TitleDetailForm';
import { TitleDetailFormReadOnly } from '../components/TitleDetailFormReadOnly';
import { isReadOnly } from '../config';

export interface Title {
  Id: number;
  TitleName: string;
  Kind: number;
  ParentTitleId?: number | null;
  Year: number;
  Description: string;
  ImdbId: string;
  Season: number;
  Disk: number;
  EpisodeOrTrack: number;
  Hidden: boolean;
}

export interface TitleDetailDto {
  Title: Title;
  Locations: LocationRow[];
  Ratings: RatingRow[];
  Images: { Id: number; Extension?: string }[];
}

export interface LocationRow {
  Id: number;
  TitleId: number;
  LocationBaseId: number;
  LocationData: string;
  LocationBase: string;
  LocationKind: number;
}

export interface RatingRow {
  RatingId: number;
  RatingName: string;
  RatingValue: number;
  RatingMin: number;
  RatingMax: number;
  RatingStep: number;
  TitleId: number;
}

export interface Device {
  Id: number;
  Name: string;
}

export interface KindOpt {
  value: number;
  name: string;
}

const KIND_ICONS: Record<number, { icon: string; label: string }> = {
  0: { icon: '🎬', label: 'Title' },
  1: { icon: '📅', label: 'Season' },
  2: { icon: '🎥', label: 'Series' },
  3: { icon: '📁', label: 'Album' },
  4: { icon: '💿', label: 'Disk' },
  5: { icon: '🎵', label: 'Track' },
  6: { icon: '👤', label: 'AlbumArtist' },
  7: { icon: '🎞️', label: 'Episode' },
};

function kindIcon(kind: number): { icon: string; label: string } {
  return KIND_ICONS[kind] ?? { icon: '❔', label: String(kind) };
}

function matchesSearch(t: Title, q: string): boolean {
  if (!q.trim()) return true;
  return (t.TitleName ?? '').toLowerCase().includes(q.trim().toLowerCase());
}

export function LibraryPage() {
  const ro = isReadOnly();
  const [resourceKind, setResourceKind] = useState<'video' | 'audio'>('video');
  const [includeHidden, setIncludeHidden] = useState(false);
  const [search, setSearch] = useState('');
  const [roots, setRoots] = useState<Title[]>([]);
  const [open, setOpen] = useState<Set<number>>(new Set());
  const [childCache, setChildCache] = useState<Record<number, Title[]>>({});
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [detail, setDetail] = useState<TitleDetailDto | null>(null);
  const [draft, setDraft] = useState<Partial<Title> | null>(null);
  const [playbackDevices, setPlaybackDevices] = useState<Device[]>([]);
  const [playbackDeviceId, setPlaybackDeviceId] = useState<number>(0);
  const [imageIndex, setImageIndex] = useState(0);
  const [message, setMessage] = useState<string | null>(null);
  const [dragTitleId, setDragTitleId] = useState<number | null>(null);

  const reloadRoots = useCallback(async () => {
    const list = await apiJson<Title[]>(
      `/api/titles/roots?resourceKind=${resourceKind}&includeHidden=${includeHidden}`
    );
    setRoots(list);
    setOpen(new Set());
    setChildCache({});
    setSelectedId(null);
    setDetail(null);
    setDraft(null);
  }, [resourceKind, includeHidden]);

  useEffect(() => {
    reloadRoots().catch((e) => setMessage(String(e)));
  }, [reloadRoots]);

  useEffect(() => {
    apiJson<Device[]>('/api/meta/devices-playback')
      .then((d) => {
        setPlaybackDevices(d);
        if (d.length) setPlaybackDeviceId((prev) => prev || d[0].Id);
      })
      .catch(() => { });
  }, []);

  useEffect(() => {
    if (!selectedId) {
      setDetail(null);
      setDraft(null);
      return;
    }
    apiJson<TitleDetailDto>(`/api/titles/${selectedId}`)
      .then((d) => {
        setDetail(d);
        setDraft({ ...d.Title });
        setImageIndex(0);
      })
      .catch((e) => setMessage(String(e)));
  }, [selectedId]);

  const filteredRoots = useMemo(
    () => roots.filter((t) => matchesSearch(t, search)),
    [roots, search]
  );

  const toggleOpen = async (id: number, e?: React.MouseEvent) => {
    e?.stopPropagation();
    const next = new Set(open);
    if (next.has(id)) next.delete(id);
    else {
      next.add(id);
      if (!childCache[id]) {
        try {
          const ch = await apiJson<Title[]>(`/api/titles/${id}/children`);
          setChildCache((c) => ({ ...c, [id]: ch }));
        } catch (err) {
          setMessage(String(err));
        }
      }
    }
    setOpen(next);
  };

  const renderBranch = (t: Title, depth: number): JSX.Element[] => {
    if (!matchesSearch(t, search) && depth > 0) {
      /* still show if any descendant matches — simplified: always show structure when parent open */
    }

    const isOpen = open.has(t.Id);
    const row = (
      <div
        key={t.Id}
        className={`mc-tree-row ${selectedId === t.Id ? 'mc-selected' : ''}`}
        style={{ paddingLeft: depth * 12 }}
        onClick={() => setSelectedId(t.Id)}
        onDragOver={(e) => e.preventDefault()}
        onDrop={(e) => {
          e.preventDefault();
          const sid = Number(e.dataTransfer.getData('text/title-id'));
          if (!sid || sid === t.Id || ro) return;
          apiJson(`/api/titles/${sid}/move`, {
            method: 'POST',
            body: JSON.stringify({ ParentId: t.Id }),
          })
            .then(() => reloadRoots())
            .catch((err) => setMessage(String(err)));
        }}
      >
        <button type="button" className="mc-muted" onClick={(e) => toggleOpen(t.Id, e)}>
          {isOpen ? '−' : '+'}
        </button>
        {!ro && (
          <span
            className="mc-drag"
            draggable
            onDragStart={(e) => {
              e.dataTransfer.setData('text/title-id', String(t.Id));
              setDragTitleId(t.Id);
            }}
            onDragEnd={() => setDragTitleId(null)}
            title="Drag onto another row to move"
          >
            ⧉
          </span>
        )}
        <span className="mc-kind-icon" title={kindIcon(t.Kind).label} aria-label={kindIcon(t.Kind).label}>
          {kindIcon(t.Kind).icon}
        </span>
        <span>{t.TitleName}</span>
      </div>
    );
    const rest: JSX.Element[] = [row];
    if (isOpen && childCache[t.Id]) {
      for (const c of childCache[t.Id]) {
        rest.push(...renderBranch(c, depth + 1));
      }
    }
    return rest;
  };

  const saveTitle = async (ev?: FormEvent) => {
    ev?.preventDefault();
    if (!draft || !selectedId || ro) return;
    try {
      await apiJson(`/api/titles/${selectedId}`, {
        method: 'PUT',
        body: JSON.stringify({
          TitleName: draft.TitleName,
          Kind: draft.Kind,
          Year: draft.Year ?? 0,
          Description: draft.Description ?? '',
          ImdbId: draft.ImdbId ?? '',
          Season: draft.Season ?? 0,
          Disk: draft.Disk ?? 0,
          EpisodeOrTrack: draft.EpisodeOrTrack ?? 0,
        }),
      });
      await reloadRoots();
      const d = await apiJson<TitleDetailDto>(`/api/titles/${selectedId}`);
      setDetail(d);
      setDraft({ ...d.Title });
      setMessage(null);
    } catch (e) {
      setMessage(String(e));
    }
  };

  const saveRatings = async () => {
    if (!detail || !selectedId || ro) return;
    const body = detail.Ratings.map((r) => ({ RatingId: r.RatingId, Value: r.RatingValue }));
    try {
      await apiJson(`/api/titles/${selectedId}/ratings`, { method: 'PUT', body: JSON.stringify(body) });
      const d = await apiJson<TitleDetailDto>(`/api/titles/${selectedId}`);
      setDetail(d);
    } catch (e) {
      setMessage(String(e));
    }
  };

  return (
    <div>
      <h2>Library</h2>
      {message && <p style={{ color: 'crimson' }}>{message}</p>}
      <div className="mc-stack" style={{ marginBottom: '1rem' }}>
        <label>
          <input
            type="radio"
            checked={resourceKind === 'video'}
            onChange={() => setResourceKind('video')}
          />{' '}
          Video
        </label>
        <label>
          <input
            type="radio"
            checked={resourceKind === 'audio'}
            onChange={() => setResourceKind('audio')}
          />{' '}
          Audio
        </label>
        <label>
          <input
            type="checkbox"
            checked={includeHidden}
            onChange={(e) => setIncludeHidden(e.target.checked)}
          />{' '}
          Include hidden
        </label>
        <button type="button" onClick={() => reloadRoots()}>
          Refresh
        </button>
      </div>
      <div className="mc-stack" style={{ marginBottom: '1rem' }}>
        <input
          type="search"
          placeholder="Search titles…"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          style={{ minWidth: 220 }}
        />
        {!ro && (
          <button
            type="button"
            onClick={async () => {
              const kind = resourceKind === 'audio' ? 6 : 0;
              try {
                const t = await apiJson<Title>('/api/titles', {
                  method: 'POST',
                  body: JSON.stringify({
                    TitleName: 'New',
                    Kind: kind,
                    ParentTitleId: null,
                    Season: 0,
                    Disk: 0,
                    EpisodeOrTrack: 0,
                  }),
                });
                await reloadRoots();
                setSelectedId(t.Id);
              } catch (e) {
                setMessage(String(e));
              }
            }}
          >
            New entry
          </button>
        )}
      </div>
      <div className="mc-split">
        <div className="mc-tree">
          {filteredRoots.flatMap((t) => renderBranch(t, 0))}
          {!filteredRoots.length && <div className="mc-muted">No titles.</div>}
        </div>
        <div className="mc-detail">
          {!draft && <p className="mc-muted">Select a title.</p>}
          {!ro && draft && detail && (
            <TitleDetailForm
              draft={draft}
              setDraft={setDraft}
              detail={detail}
              setDetail={setDetail}
              selectedId={selectedId}
              resourceKind={resourceKind}
              playbackDevices={playbackDevices}
              playbackDeviceId={playbackDeviceId}
              setPlaybackDeviceId={setPlaybackDeviceId}
              imageIndex={imageIndex}
              setImageIndex={setImageIndex}
              reloadRoots={reloadRoots}
              saveTitle={saveTitle}
              saveRatings={saveRatings}
              setMessage={setMessage}
            />
          )}
          {ro && detail && (
            <TitleDetailFormReadOnly
              detail={detail}
              selectedId={selectedId}
              resourceKind={resourceKind}
              playbackDevices={playbackDevices}
              playbackDeviceId={playbackDeviceId}
              setPlaybackDeviceId={setPlaybackDeviceId}
              imageIndex={imageIndex}
              setImageIndex={setImageIndex}
              setMessage={setMessage}
            />
          )}
        </div>
      </div>
      {dragTitleId != null && (
        <p className="mc-muted">Dragging title #{dragTitleId} — drop on another row to reparent.</p>
      )}
    </div>
  );
}
