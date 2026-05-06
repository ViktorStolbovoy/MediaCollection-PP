import { useEffect, useState } from 'react';
import { apiJson } from '../api';
import { isReadOnly } from '../config';

interface RatingProvider {
  Id: number;
  RatingName: string;
  RatingKind: number;
  RatingMin: number;
  RatingMax: number;
  RatingStep: number;
}

export function RatingsPage() {
  const ro = isReadOnly();
  const [rows, setRows] = useState<RatingProvider[]>([]);
  const [err, setErr] = useState<string | null>(null);

  const load = () =>
    apiJson<RatingProvider[]>('/api/rating-providers')
      .then(setRows)
      .catch((e) => setErr(String(e)));

  useEffect(() => {
    load();
  }, []);

  if (err) return <p style={{ color: 'crimson' }}>{err}</p>;

  return (
    <div>
      <h2>Rating providers</h2>
      {!ro && (
        <button
          type="button"
          onClick={() =>
            apiJson<RatingProvider>('/api/rating-providers', {
              method: 'POST',
              body: JSON.stringify({
                RatingName: 'New Rating',
                RatingKind: 0,
                RatingMin: 1,
                RatingMax: 10,
                RatingStep: 1,
              }),
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
              <th>Min</th>
              <th>Max</th>
              <th>Step</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {rows.map((r) => (
              <tr key={r.Id}>
                <td>
                  <input
                    type="text"
                    defaultValue={r.RatingName}
                    disabled={ro}
                    onBlur={(e) =>
                      !ro &&
                      apiJson(`/api/rating-providers/${r.Id}`, {
                        method: 'PUT',
                        body: JSON.stringify({ ...r, RatingName: e.target.value }),
                      }).then(load)
                    }
                  />
                </td>
                <td>
                  <input
                    type="number"
                    step="0.1"
                    defaultValue={r.RatingMin}
                    disabled={ro}
                    onBlur={(e) =>
                      !ro &&
                      apiJson(`/api/rating-providers/${r.Id}`, {
                        method: 'PUT',
                        body: JSON.stringify({ ...r, RatingMin: Number(e.target.value) }),
                      }).then(load)
                    }
                  />
                </td>
                <td>
                  <input
                    type="number"
                    step="0.1"
                    defaultValue={r.RatingMax}
                    disabled={ro}
                    onBlur={(e) =>
                      !ro &&
                      apiJson(`/api/rating-providers/${r.Id}`, {
                        method: 'PUT',
                        body: JSON.stringify({ ...r, RatingMax: Number(e.target.value) }),
                      }).then(load)
                    }
                  />
                </td>
                <td>
                  <input
                    type="number"
                    step="0.1"
                    defaultValue={r.RatingStep}
                    disabled={ro}
                    onBlur={(e) =>
                      !ro &&
                      apiJson(`/api/rating-providers/${r.Id}`, {
                        method: 'PUT',
                        body: JSON.stringify({ ...r, RatingStep: Number(e.target.value) }),
                      }).then(load)
                    }
                  />
                </td>
                <td>
                  {!ro && (
                    <button
                      type="button"
                      onClick={() =>
                        confirm(`Delete ${r.RatingName}?`) &&
                        apiJson(`/api/rating-providers/${r.Id}`, { method: 'DELETE' }).then(load)
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
