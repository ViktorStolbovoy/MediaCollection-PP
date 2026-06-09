import { Dispatch, FormEvent, SetStateAction, useState } from 'react';
import { apiJson, uploadTitleImage } from '../api';
import type { Device, KindOpt, Title, TitleDetailDto } from '../pages/LibraryPage';
import { SeasonsToolModal } from './SeasonsToolModal';

const SEASONS_TOOL_KINDS = new Set<number>([1, 2, 4, 7]);

const VIDEO_KINDS: KindOpt[] = [
  { value: 0, name: 'Title' },
  { value: 2, name: 'Series' },
  { value: 1, name: 'Season' },
  { value: 4, name: 'Disk' },
  { value: 7, name: 'Episode' },
];

const AUDIO_KINDS: KindOpt[] = [
  { value: 6, name: 'AlbumArtist' },
  { value: 3, name: 'Album' },
  { value: 5, name: 'Track' },
];

export function titleKinds(resourceKind: 'video' | 'audio'): KindOpt[] {
  return resourceKind === 'audio' ? AUDIO_KINDS : VIDEO_KINDS;
}

interface TitleDetailFormProps {
  draft: Partial<Title>;
  setDraft: Dispatch<SetStateAction<Partial<Title> | null>>;
  detail: TitleDetailDto;
  setDetail: Dispatch<SetStateAction<TitleDetailDto | null>>;
  selectedId: number | null;
  resourceKind: 'video' | 'audio';
  playbackDevices: Device[];
  playbackDeviceId: number;
  setPlaybackDeviceId: Dispatch<SetStateAction<number>>;
  imageIndex: number;
  setImageIndex: Dispatch<SetStateAction<number>>;
  reloadRoots: () => Promise<void> | void;
  saveTitle: (ev?: FormEvent) => void;
  saveRatings: () => void;
  setMessage: Dispatch<SetStateAction<string | null>>;
}

export function TitleDetailForm({
  draft,
  setDraft,
  detail,
  setDetail,
  selectedId,
  resourceKind,
  playbackDevices,
  playbackDeviceId,
  setPlaybackDeviceId,
  imageIndex,
  setImageIndex,
  reloadRoots,
  saveTitle,
  saveRatings,
  setMessage,
}: TitleDetailFormProps) {
  const kinds = titleKinds(resourceKind);
  const currentImage = detail?.Images?.[imageIndex];
  const imageUrl =
    selectedId && currentImage ? `/api/titles/${selectedId}/images/${currentImage.Id}/file` : '';
  const [showSeasonsTool, setShowSeasonsTool] = useState(false);
  const seasonsToolEligible =
    resourceKind === 'video' &&
    selectedId != null &&
    SEASONS_TOOL_KINDS.has(detail.Title.Kind);

  return (
    <form onSubmit={saveTitle}>
      <fieldset>
        <legend>Title</legend>
        <label>Name</label>
        <input
          type="text"
          value={draft.TitleName ?? ''}
          onChange={(e) => setDraft({ ...draft, TitleName: e.target.value })}
        />
        <label>Kind</label>
        <select
          value={draft.Kind ?? 0}
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
          onChange={(e) => setDraft({ ...draft, Year: Number(e.target.value) })}
        />
        <label>IMDb Id</label>
        <input
          type="text"
          value={draft.ImdbId ?? ''}
          onChange={(e) => setDraft({ ...draft, ImdbId: e.target.value })}
        />
        <label>Season / Disk / Episode</label>
        <div className="mc-stack">
          <input
            type="number"
            value={draft.Season ?? 0}
            onChange={(e) => setDraft({ ...draft, Season: Number(e.target.value) })}
          />
          <input
            type="number"
            value={draft.Disk ?? 0}
            onChange={(e) => setDraft({ ...draft, Disk: Number(e.target.value) })}
          />
          <input
            type="number"
            value={draft.EpisodeOrTrack ?? 0}
            onChange={(e) => setDraft({ ...draft, EpisodeOrTrack: Number(e.target.value) })}
          />
        </div>
        <label>Description</label>
        <textarea
          value={draft.Description ?? ''}
          onChange={(e) => setDraft({ ...draft, Description: e.target.value })}
        />
        <div className="mc-stack">
          <button type="submit">Save title</button>
          <button type="button" onClick={() => setDraft({ ...detail.Title })}>
            Discard
          </button>
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
        <div className="mc-playback-controls">
          <label className="mc-playback-controls-label" htmlFor="mc-playback-device">
            Playback device
          </label>
          <select
            id="mc-playback-device"
            value={playbackDeviceId}
            onChange={(e) => setPlaybackDeviceId(Number(e.target.value))}
          >
            {playbackDevices.map((d) => (
              <option key={d.Id} value={d.Id}>
                {d.Name}
              </option>
            ))}
          </select>
          <button
            className="mc-play-btn"
            type="button"
            disabled={!detail.Locations.length}
            onClick={() => {
              if (!selectedId || !playbackDeviceId) return;
              apiJson('/api/playback/run', {
                method: 'POST',
                body: JSON.stringify({ DeviceId: playbackDeviceId, TitleId: selectedId }),
              }).catch((e) => setMessage(String(e)));
            }}
          >
            Play
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
                      onBlur={(e) => {
                        apiJson(`/api/titles/${selectedId}/locations/${loc.Id}`, {
                          method: 'PUT',
                          body: JSON.stringify({ LocationData: e.target.value }),
                        }).catch((err) => setMessage(String(err)));
                      }}
                    />
                  </td>
                  <td>
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
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </fieldset>
      <fieldset>
        <legend>Ratings</legend>
        <div className="mc-table-wrap">
          <table className="mc-table mc-ratings-table">
            <thead>
              <tr>
                <th>Rating</th>
                <th>Value</th>
              </tr>
            </thead>
            <tbody>
              {detail.Ratings.map((r) => (
                <tr key={r.RatingId}>
                  <td>
                    <label htmlFor={`mc-rating-${r.RatingId}`}>{r.RatingName}</label>
                  </td>
                  <td>
                    <input
                      id={`mc-rating-${r.RatingId}`}
                      type="number"
                      step={r.RatingStep}
                      min={0}
                      max={r.RatingMax}
                      placeholder="not set"
                      title={`0 = not set; otherwise ${r.RatingMin}\u2013${r.RatingMax}`}
                      value={r.RatingValue ? r.RatingValue : ''}
                      onChange={(e) => {
                        const raw = e.target.value;
                        const v = raw === '' ? 0 : Number(raw);
                        setDetail({
                          ...detail,
                          Ratings: detail.Ratings.map((x) =>
                            x.RatingId === r.RatingId ? { ...x, RatingValue: v } : x
                          ),
                        });
                      }}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <button type="button" onClick={saveRatings}>
          Save ratings
        </button>
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
          {currentImage && (
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
      {seasonsToolEligible && (
        <div className="mc-title-detail-footer">
          <button type="button" onClick={() => setShowSeasonsTool(true)}>
            Seasons Tool…
          </button>
        </div>
      )}
      {showSeasonsTool && selectedId != null && (
        <SeasonsToolModal titleId={selectedId} onClose={() => setShowSeasonsTool(false)} />
      )}
    </form>
  );
}
