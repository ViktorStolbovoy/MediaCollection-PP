import { useEffect, useState } from 'react';
import { apiJson } from '../api';
import { isReadOnly } from '../config';

interface Device {
  Id: number;
  Name: string;
  Data: string;
  Kind: number;
  IsDefault: number;
}

interface MappingRow {
  DeviceId: number;
  LocationBaseId: number;
  Mapping: string;
  Name: string;
  Kind: number;
}

interface Workspace {
  Devices: Device[];
  MappingsByDeviceId: Record<string, MappingRow[]>;
}

export function DevicesPage() {
  const ro = isReadOnly();
  const [data, setData] = useState<Workspace | null>(null);
  const [err, setErr] = useState<string | null>(null);

  const load = () =>
    apiJson<Workspace>('/api/devices')
      .then(setData)
      .catch((e) => setErr(String(e)));

  useEffect(() => {
    load();
  }, []);

  if (err) return <p style={{ color: 'crimson' }}>{err}</p>;
  if (!data) return <p>Loading…</p>;

  return (
    <div>
      <h2>Devices</h2>
      <p className="mc-muted">
        Edit device rows and per–location-base path mappings (same tree as the desktop Devices dialog).
      </p>
      {!ro && (
        <button
          type="button"
          onClick={() =>
            apiJson<Device>('/api/devices', {
              method: 'POST',
              body: JSON.stringify({ Name: 'New Device', Data: '', Kind: 1, IsDefault: 0 }),
            }).then(load)
          }
        >
          Add device
        </button>
      )}
      {data.Devices.map((d) => (
        <fieldset key={d.Id}>
          <legend>{d.Name}</legend>
          <label>
            Name{' '}
            <input
              type="text"
              defaultValue={d.Name}
              disabled={ro}
              onBlur={(e) =>
                !ro &&
                apiJson(`/api/devices/${d.Id}`, {
                  method: 'PUT',
                  body: JSON.stringify({ ...d, Name: e.target.value }),
                }).then(load)
              }
            />
          </label>
          <label>
            Data / format{' '}
            <input
              type="text"
              style={{ width: '100%', maxWidth: 640 }}
              defaultValue={d.Data}
              disabled={ro}
              onBlur={(e) =>
                !ro &&
                apiJson(`/api/devices/${d.Id}`, {
                  method: 'PUT',
                  body: JSON.stringify({ ...d, Data: e.target.value }),
                }).then(load)
              }
            />
          </label>
          <label>
            <input
              type="checkbox"
              checked={d.IsDefault > 0}
              disabled={ro}
              onChange={(e) =>
                !ro &&
                apiJson(`/api/devices/${d.Id}`, {
                  method: 'PUT',
                  body: JSON.stringify({ ...d, IsDefault: e.target.checked ? 1 : 0 }),
                }).then(load)
              }
            />{' '}
            Default playback device
          </label>
          <h4>Mappings</h4>
          <div className="mc-table-wrap">
            <table className="mc-table">
              <thead>
                <tr>
                  <th>Location base</th>
                  <th>Mapping path</th>
                </tr>
              </thead>
              <tbody>
                {(data.MappingsByDeviceId[String(d.Id)] ?? []).map((m) => (
                  <tr key={`${m.DeviceId}-${m.LocationBaseId}`}>
                    <td>{m.Name}</td>
                    <td>
                      <input
                        type="text"
                        defaultValue={m.Mapping ?? ''}
                        disabled={ro}
                        onBlur={(e) =>
                          !ro &&
                          apiJson(`/api/devices/${d.Id}/mappings`, {
                            method: 'PUT',
                            body: JSON.stringify([
                              { LocationBaseId: m.LocationBaseId, Mapping: e.target.value },
                            ]),
                          }).then(load)
                        }
                      />
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </fieldset>
      ))}
    </div>
  );
}
