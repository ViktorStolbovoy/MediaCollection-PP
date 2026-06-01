import { Dispatch, SetStateAction } from 'react';
import { apiJson } from '../api';
import type { Device, TitleDetailDto } from '../pages/LibraryPage';

interface TitleDetailFormProps {
  detail: TitleDetailDto;
  selectedId: number | null;
  resourceKind: 'video' | 'audio';
  playbackDevices: Device[];
  playbackDeviceId: number;
  setPlaybackDeviceId: Dispatch<SetStateAction<number>>;
  imageIndex: number;
  setImageIndex: Dispatch<SetStateAction<number>>;
  setMessage: Dispatch<SetStateAction<string | null>>;
}

export function TitleDetailFormReadOnly({
  detail,
  selectedId,
  resourceKind,
  playbackDevices,
  playbackDeviceId,
  setPlaybackDeviceId,
  imageIndex,
  setImageIndex,
  setMessage,
}: TitleDetailFormProps) {
  const currentImage = detail?.Images?.[imageIndex];
  const imageUrl =
    selectedId && currentImage ? `/api/titles/${selectedId}/images/${currentImage.Id}/file` : '';


  const sde: string[] = [];
  if (detail.Title.Year > 0) sde.push(detail.Title.Year.toString());
  if (detail.Title.Season) sde.push(`Season ${detail.Title.Season}`);
  if (detail.Title.Disk) sde.push(`Disk ${detail.Title.Disk}`);
  if (detail.Title.EpisodeOrTrack) sde.push((resourceKind === "audio" ? " Track " : " Episode ") + detail.Title.EpisodeOrTrack);

  return (
    <div>
      <h3 style={{ marginBottom: 0 }}>{detail.Title.TitleName}</h3>
      <small style={{ display: 'block', margin: 0, marginBottom: 10 }}>
        {sde.join(" ")} {detail.Title.ImdbId && (
          <a
            href={`https://www.imdb.com/title/${detail.Title.ImdbId}`}
            target="_blank"
            rel="noreferrer"
          >
            IMDb
          </a>
        )}</small>
      <div>{detail.Title.Description}</div>
      {detail.Locations.length > 0 && <div className="mc-playback-controls">
        <button
          className="mc-play-btn"
          type="button"
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
        on
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
      </div>}
      <div style={{ fontSize: '0.85em' }}>
        {detail.Locations.map((loc) => (
          <div key={loc.Id}>
            {loc.LocationBase}: {loc.LocationData}
          </div>
        ))}
      </div>
      <div>
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
        </div>
      </div>
    </div >
  );
}
