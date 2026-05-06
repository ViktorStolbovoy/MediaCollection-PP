import { FormEvent, useCallback, useEffect, useMemo, useState } from 'react';
import { apiJson, uploadTitleImage } from '../api';
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

interface TitleDetailDto {
  Title: Title;
  Locations: LocationRow[];
  Ratings: RatingRow[];
  Images: { Id: number; Extension?: string }[];
}

interface LocationRow {
  Id: number;
  TitleId: number;
  LocationBaseId: number;
  LocationData: string;
  LocationBase: string;
  LocationKind: number;
}

interface RatingRow {
  RatingId: number;
  RatingName: string;
  RatingValue: number;
  RatingMin: number;
  RatingMax: number;
  RatingStep: number;
  TitleId: number;
}

interface Device {
  Id: number;
  Name: string;
}

interface KindOpt {
  value: number;
  name: string;
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
  const [kinds, setKinds] = useState<KindOpt[]>([]);
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
    apiJson<KindOpt[]>(`/api/meta/title-kinds?resourceKind=${resourceKind}`)
      .then(setKinds)
      .catch(() => {});
  }, [resourceKind]);

  useEffect(() => {
    apiJson<Device[]>('/api/meta/devices-playback')
      .then((d) => {
        setPlaybackDevices(d);
        if (d.length) setPlaybackDeviceId((prev) => prev || d[0].Id);
      })
      .catch(() => {});
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
    const kids = childCache[t.Id] ?? [];
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
        <span>{t.TitleName}</span>
        <span className="mc-muted">({t.Kind})</span>
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

  const currentImage = detail?.Images?.[imageIndex];
  const imageUrl =
    selectedId && currentImage ? `/api/titles/${selectedId}/images/${currentImage.Id}/file` : '';

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
              const kind = kinds[0]?.value ?? (resourceKind === 'audio' ? 6 : 0);
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
          {draft && detail && (
            <form onSubmit={saveTitle}>
              <fieldset>
                <legend>Title</legend>
                <label>Name</label>
                <input
                  type="text"
                  value={draft.TitleName ?? ''}
                  disabled={ro}
                  onChange={(e) => setDraft({ ...draft, TitleName: e.target.value })}
                />
                <label>Kind</label>
                <select
                  value={draft.Kind ?? 0}
                  disabled={ro}
                  onChange={(e) => setDraft({ ...draft, Kind: Number(e.target.value) })}
                >
                  {kinds.map((k) => (
                    <option key={k.value} value={k.value}>
                      {k.name}
                    </option>
                  ))}
                </select>
                <label>Year</label>
                <input
                  type="number"
                  value={draft.Year ?? 0}
                  disabled={ro}
                  onChange={(e) => setDraft({ ...draft, Year: Number(e.target.value) })}
                />
                <label>IMDb Id</label>
                <input
                  type="text"
                  value={draft.ImdbId ?? ''}
                  disabled={ro}
                  onChange={(e) => setDraft({ ...draft, ImdbId: e.target.value })}
                />
                <label>Season / Disk / Episode</label>
                <div className="mc-stack">
                  <input
                    type="number"
                    disabled={ro}
                    value={draft.Season ?? 0}
                    onChange={(e) => setDraft({ ...draft, Season: Number(e.target.value) })}
                  />
                  <input
                    type="number"
                    disabled={ro}
                    value={draft.Disk ?? 0}
                    onChange={(e) => setDraft({ ...draft, Disk: Number(e.target.value) })}
                  />
                  <input
                    type="number"
                    disabled={ro}
                    value={draft.EpisodeOrTrack ?? 0}
                    onChange={(e) => setDraft({ ...draft, EpisodeOrTrack: Number(e.target.value) })}
                  />
                </div>
                <label>Description</label>
                <textarea
                  disabled={ro}
                  value={draft.Description ?? ''}
                  onChange={(e) => setDraft({ ...draft, Description: e.target.value })}
                />
                <div className="mc-stack">
                  {!ro && <button type="submit">Save title</button>}
                  {!ro && (
                    <button type="button" onClick={() => setDraft({ ...detail.Title })}>
                      Discard
                    </button>
                  )}
                  {!ro && (
                    <button
                      type="button"
                      onClick={() =>
                        apiJson(`/api/titles/${selectedId}/toggle-hidden`, { method: 'POST' }).then(
                          () => reloadRoots()
                        )
                      }
                    >
                      {detail.Title.Hidden ? 'Show' : 'Hide'}
                    </button>
                  )}
                  {!ro && (
                    <button
                      type="button"
                      onClick={() => {
                        if (!selectedId || !confirm('Delete this title?')) return;
                        apiJson(`/api/titles/${selectedId}`, { method: 'DELETE' }).then(() =>
                          reloadRoots()
                        );
                      }}
                    >
                      Delete
                    </button>
                  )}
                  {draft.ImdbId && (
                    <a
                      href={`https://www.imdb.com/title/${draft.ImdbId}`}
                      target="_blank"
                      rel="noreferrer"
                    >
                      Open IMDb
                    </a>
                  )}
                </div>
              </fieldset>
              <fieldset>
                <legend>Locations</legend>
                <div className="mc-stack">
                  <label>
                    Playback device{' '}
                    <select
                      value={playbackDeviceId}
                      onChange={(e) => setPlaybackDeviceId(Number(e.target.value))}
                    >
                      {playbackDevices.map((d) => (
                        <option key={d.Id} value={d.Id}>
                          {d.Name}
                        </option>
                      ))}
                    </select>
                  </label>
                  <button
                    type="button"
                    onClick={() => {
                      if (!selectedId || !playbackDeviceId) return;
                      apiJson('/api/playback/run', {
                        method: 'POST',
                        body: JSON.stringify({ DeviceId: playbackDeviceId, TitleId: selectedId }),
                      }).catch((e) => setMessage(String(e)));
                    }}
                  >
                    Play (launch path)
                  </button>
                </div>
                <div className="mc-table-wrap">
                  <table className="mc-table">
                    <thead>
                      <tr>
                        <th>Base</th>
                        <th>Path</th>
                        <th />
                      </tr>
                    </thead>
                    <tbody>
                      {detail.Locations.map((loc) => (
                        <tr key={loc.Id}>
                          <td>{loc.LocationBase}</td>
                          <td>
                            <input
                              type="text"
                              defaultValue={loc.LocationData}
                              disabled={ro}
                              onBlur={(e) => {
                                if (ro) return;
                                apiJson(`/api/titles/${selectedId}/locations/${loc.Id}`, {
                                  method: 'PUT',
                                  body: JSON.stringify({ LocationData: e.target.value }),
                                }).catch((err) => setMessage(String(err)));
                              }}
                            />
                          </td>
                          <td>
                            {!ro && (
                              <button
                                type="button"
                                onClick={() => {
                                  if (!confirm('Remove location row?')) return;
                                  apiJson(`/api/titles/${selectedId}/locations/${loc.Id}`, {
                                    method: 'DELETE',
                                  }).then(() =>
                                    apiJson<TitleDetailDto>(`/api/titles/${selectedId}`).then(setDetail)
                                  );
                                }}
                              >
                                ✕
                              </button>
                            )}
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </fieldset>
              <fieldset>
                <legend>Ratings</legend>
                {detail.Ratings.map((r) => (
                  <label key={r.RatingId}>
                    {r.RatingName}{' '}
                    <input
                      type="number"
                      step={r.RatingStep}
                      min={r.RatingMin}
                      max={r.RatingMax}
                      disabled={ro}
                      value={r.RatingValue}
                      onChange={(e) => {
                        const v = Number(e.target.value);
                        setDetail({
                          ...detail,
                          Ratings: detail.Ratings.map((x) =>
                            x.RatingId === r.RatingId ? { ...x, RatingValue: v } : x
                          ),
                        });
                      }}
                    />
                  </label>
                ))}
                {!ro && (
                  <button type="button" onClick={saveRatings}>
                    Save ratings
                  </button>
                )}
              </fieldset>
              <fieldset>
                <legend>Images</legend>
                {currentImage && <img className="mc-thumb" src={imageUrl} alt="" />}
                <div className="mc-stack">
                  <button
                    type="button"
                    disabled={imageIndex <= 0}
                    onClick={() => setImageIndex((i) => Math.max(0, i - 1))}
                  >
                    Prev
                  </button>
                  <button
                    type="button"
                    disabled={!detail.Images || imageIndex >= detail.Images.length - 1}
                    onClick={() =>
                      setImageIndex((i) =>
                        detail.Images ? Math.min(detail.Images.length - 1, i + 1) : i
                      )
                    }
                  >
                    Next
                  </button>
                  {!ro && (
                    <label>
                      Add image{' '}
                      <input
                        type="file"
                        accept="image/*"
                        onChange={(e) => {
                          const f = e.target.files?.[0];
                          if (!f || !selectedId) return;
                          uploadTitleImage(selectedId, f)
                            .then(() =>
                              apiJson<TitleDetailDto>(`/api/titles/${selectedId}`).then((d) => {
                                setDetail(d);
                                setImageIndex(d.Images.length - 1);
                              })
                            )
                            .catch((err) => setMessage(String(err)));
                        }}
                      />
                    </label>
                  )}
                  {!ro && currentImage && (
                    <button
                      type="button"
                      onClick={() => {
                        if (!selectedId || !confirm('Delete current image?')) return;
                        apiJson(`/api/titles/${selectedId}/images/${currentImage.Id}`, {
                          method: 'DELETE',
                        }).then(() =>
                          apiJson<TitleDetailDto>(`/api/titles/${selectedId}`).then((d) => {
                            setDetail(d);
                            setImageIndex(0);
                          })
                        );
                      }}
                    >
                      Delete image
                    </button>
                  )}
                </div>
              </fieldset>
            </form>
          )}
        </div>
      </div>
      {dragTitleId != null && (
        <p className="mc-muted">Dragging title #{dragTitleId} — drop on another row to reparent.</p>
      )}
    </div>
  );
}
