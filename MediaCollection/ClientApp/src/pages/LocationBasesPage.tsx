import { useEffect, useState } from 'react';
import { apiJson } from '../api';
import { isReadOnly } from '../config';

/** Mirrors McLib `LocationBaseKind` (LOCATION_KIND). */
const LOCATION_BASE_KIND_OPTIONS = [
  { value: 0, name: 'Local' },
  { value: 1, name: 'RemovableHDD' },
  { value: 2, name: 'Shelf' },
  { value: 3, name: 'HTTP' },
] as const;

interface LocationBase {
  Id: number;
  Name: string;
  Kind: number;
}

export function LocationBasesPage() {
  const ro = isReadOnly();
  const [rows, setRows] = useState<LocationBase[]>([]);
  const [err, setErr] = useState<string | null>(null);

  const load = () =>
    apiJson<LocationBase[]>('/api/location-bases')
      .then(setRows)
      .catch((e) => setErr(String(e)));

  useEffect(() => {
    load();
  }, []);

  if (err) return <p style={{ color: 'crimson' }}>{err}</p>;

  return (
    <div>
      <h2>Location bases</h2>
      {!ro && (
        <button
          type="button"
          onClick={() =>
            apiJson<LocationBase>('/api/location-bases', {
              method: 'POST',
              body: JSON.stringify({ Name: '', Kind: 0 }),
            }).then(load)
          }
        >
          Add
        </button>
      )}
      <div className="mc-table-wrap">
        <table className="mc-table">
          <thead>
            <tr>
              <th>Name</th>
              <th>Kind</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {rows.map((r) => (
              <tr key={r.Id}>
                <td>
                  <input
                    type="text"
                    defaultValue={r.Name}
                    disabled={ro}
                    onBlur={(e) =>
                      !ro &&
                      apiJson(`/api/location-bases/${r.Id}`, {
                        method: 'PUT',
                        body: JSON.stringify({ ...r, Name: e.target.value }),
                      }).then(load)
                    }
                  />
                </td>
                <td>
                  <select
                    defaultValue={r.Kind}
                    disabled={ro}
                    onChange={(e) =>
                      !ro &&
                      apiJson(`/api/location-bases/${r.Id}`, {
                        method: 'PUT',
                        body: JSON.stringify({ ...r, Kind: Number(e.target.value) }),
                      }).then(load)
                    }
                  >
                    {!LOCATION_BASE_KIND_OPTIONS.some((o) => o.value === r.Kind) && (
                      <option value={r.Kind}>{r.Kind}</option>
                    )}
                    {LOCATION_BASE_KIND_OPTIONS.map((opt) => (
                      <option key={opt.value} value={opt.value}>
                        {opt.name}
                      </option>
                    ))}
                  </select>
                </td>
                <td>
                  {!ro && (
                    <button
                      type="button"
                      onClick={() =>
                        confirm(`Delete ${r.Name}?`) &&
                        apiJson(`/api/location-bases/${r.Id}`, { method: 'DELETE' }).then(load)
                      }
                    >
                      Delete
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
