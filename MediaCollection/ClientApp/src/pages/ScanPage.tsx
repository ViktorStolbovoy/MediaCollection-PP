import { useEffect, useState } from 'react';
import { apiJson } from '../api';
import { isReadOnly } from '../config';

interface Device {
  Id: number;
  Name: string;
}

interface LocationBase {
  Id: number;
  Name: string;
}

interface ScanPreview {
  ScanId: string;
  NewFiles: { RelativePath: string; ParsedTitle: string; DataType: number }[];
  Missing: { LocationId: number; TitleId: number; LocationData: string; TitleName: string }[];
}

export function ScanPage() {
  const ro = isReadOnly();
  const [devices, setDevices] = useState<Device[]>([]);
  const [deviceId, setDeviceId] = useState(0);
  const [locations, setLocations] = useState<LocationBase[]>([]);
  const [locationId, setLocationId] = useState(0);
  const [preview, setPreview] = useState<ScanPreview | null>(null);
  const [delMissing, setDelMissing] = useState<Record<number, boolean>>({});
  const [addNew, setAddNew] = useState<Record<string, boolean>>({});
  const [msg, setMsg] = useState<string | null>(null);

  useEffect(() => {
    apiJson<Device[]>('/api/meta/devices-for-title-update').then((d) => {
      setDevices(d);
      if (d.length) setDeviceId(d[0].Id);
    });
  }, []);

  useEffect(() => {
    if (!deviceId) return;
    apiJson<LocationBase[]>(`/api/meta/locations-for-device/${deviceId}`).then((list) => {
      setLocations(list);
      const firstReal = list.find((l) => l.Id > 0);
      setLocationId(firstReal?.Id ?? 0);
    });
  }, [deviceId]);

  const runPreview = () => {
    setMsg(null);
    if (!deviceId || !locationId) return;
    apiJson<ScanPreview>('/api/scan/preview', {
      method: 'POST',
      body: JSON.stringify({ LocationBaseId: locationId, DeviceId: deviceId }),
    })
      .then((p) => {
        setPreview(p);
        const dm: Record<number, boolean> = {};
        p.Missing.forEach((m) => {
          dm[m.LocationId] = false;
        });
        setDelMissing(dm);
        const an: Record<string, boolean> = {};
        p.NewFiles.forEach((n) => {
          an[n.RelativePath] = true;
        });
        setAddNew(an);
      })
      .catch((e) => setMsg(String(e)));
  };

  const apply = () => {
    if (!preview || ro) return;
    const deleteIds = Object.entries(delMissing)
      .filter(([, v]) => v)
      .map(([k]) => Number(k));
    const importPaths = Object.entries(addNew)
      .filter(([, v]) => v)
      .map(([k]) => k);
    apiJson('/api/scan/apply', {
      method: 'POST',
      body: JSON.stringify({
        ScanId: preview.ScanId,
        DeleteMissingLocationIds: deleteIds,
        ImportNewRelativePaths: importPaths,
      }),
    })
      .then(() => {
        setPreview(null);
        setMsg('Applied.');
      })
      .catch((e) => setMsg(String(e)));
  };

  return (
    <div>
      <h2>Bulk scan</h2>
      <p className="mc-muted">
        Match disk files to database rows for a PC device mapping (same workflow as desktop Bulk Add).
      </p>
      {msg && <p style={{ color: msg.startsWith('Applied') ? 'green' : 'crimson' }}>{msg}</p>}
      <div className="mc-stack">
        <label>
          Device{' '}
          <select value={deviceId} onChange={(e) => setDeviceId(Number(e.target.value))}>
            {devices.map((d) => (
              <option key={d.Id} value={d.Id}>
                {d.Name}
              </option>
            ))}
          </select>
        </label>
        <label>
          Location base{' '}
          <select value={locationId} onChange={(e) => setLocationId(Number(e.target.value))}>
            {locations
              .filter((l) => l.Id > 0)
              .map((l) => (
                <option key={l.Id} value={l.Id}>
                  {l.Name}
                </option>
              ))}
          </select>
        </label>
        <button type="button" onClick={runPreview}>
          Scan
        </button>
        {!ro && preview && (
          <button type="button" onClick={apply}>
            Apply selected changes
          </button>
        )}
      </div>
      {preview && (
        <div className="mc-split" style={{ marginTop: '1rem' }}>
          <fieldset>
            <legend>New on disk ({preview.NewFiles.length})</legend>
            <div className="mc-table-wrap" style={{ maxHeight: 360, overflow: 'auto' }}>
              <table className="mc-table">
                <thead>
                  <tr>
                    <th>Import</th>
                    <th>Path</th>
                    <th>Parsed title</th>
                  </tr>
                </thead>
                <tbody>
                  {preview.NewFiles.map((n) => (
                    <tr key={n.RelativePath}>
                      <td>
                        <input
                          type="checkbox"
                          checked={!!addNew[n.RelativePath]}
                          disabled={ro}
                          onChange={(e) =>
                            setAddNew({ ...addNew, [n.RelativePath]: e.target.checked })
                          }
                        />
                      </td>
                      <td>{n.RelativePath}</td>
                      <td>{n.ParsedTitle}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </fieldset>
          <fieldset>
            <legend>Missing on disk ({preview.Missing.length})</legend>
            <div className="mc-table-wrap" style={{ maxHeight: 360, overflow: 'auto' }}>
              <table className="mc-table">
                <thead>
                  <tr>
                    <th>Delete row</th>
                    <th>Title</th>
                    <th>Path</th>
                  </tr>
                </thead>
                <tbody>
                  {preview.Missing.map((m) => (
                    <tr key={m.LocationId}>
                      <td>
                        <input
                          type="checkbox"
                          checked={!!delMissing[m.LocationId]}
                          disabled={ro}
                          onChange={(e) =>
                            setDelMissing({ ...delMissing, [m.LocationId]: e.target.checked })
                          }
                        />
                      </td>
                      <td>{m.TitleName}</td>
                      <td>{m.LocationData}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </fieldset>
        </div>
      )}
    </div>
  );
}
