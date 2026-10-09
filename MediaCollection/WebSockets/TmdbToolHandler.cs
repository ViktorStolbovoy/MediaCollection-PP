using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MediaCollection.WebSockets
{
	/// <summary>
	/// Handles request/response style WebSocket messages used by the bulk
	/// "Update From TMDB" UI. Mirrors the desktop <c>UpdateFromProvider</c>
	/// dialog: load the autoupdate queue, run TMDB searches, and apply a
	/// chosen result (or a manual edit) to a single title.
	/// </summary>
	internal static class TmdbToolHandler
	{
		public const string InitType = "tmdb-tool-init";
		public const string SearchType = "tmdb-tool-search";
		public const string ApplyType = "tmdb-tool-apply";
		public const string SaveManualType = "tmdb-tool-save-manual";

		// 30 seconds matches the client-side request timeout for these messages.
		private static readonly TimeSpan HttpTimeout = TimeSpan.FromSeconds(30);

		public static bool IsTmdbToolMessage(string type)
		{
			return type != null && type.StartsWith("tmdb-tool-", StringComparison.OrdinalIgnoreCase);
		}

		public static async Task<(string ResponseType, object Response, bool Mutated)> HandleAsync(string type, JsonElement payload)
		{
			var requestId = ReadString(payload, "RequestId") ?? "";
			switch (type)
			{
				case InitType:
					return (InitType + "-response", await HandleInitAsync(requestId), false);
				case SearchType:
					return (SearchType + "-response", await HandleSearchAsync(requestId, payload), false);
				case ApplyType:
					return (ApplyType + "-response", await HandleApplyAsync(requestId, payload), true);
				case SaveManualType:
					return (SaveManualType + "-response", await HandleSaveManualAsync(requestId, payload), true);
				default:
					return (null, null, false);
			}
		}

		private static async Task<object> HandleInitAsync(string requestId)
		{
			var titles = await TitlePersistence.GetTitlesForAutoupdate();
			return new TmdbToolInitResponse
			{
				RequestId = requestId,
				Titles = titles.Select(ToQueueItem).ToList()
			};
		}

		private static async Task<object> HandleSearchAsync(string requestId, JsonElement payload)
		{
			string query = (ReadString(payload, "Query") ?? "").Trim();
			bool isTv = ReadBool(payload, "IsTv");
			if (string.IsNullOrEmpty(query)) return new TmdbToolErrorResponse(requestId, "Query is required.");

			using var cts = new CancellationTokenSource(HttpTimeout);
			TmdbData data;
			try
			{
				data = await TmdbData.Get(query, isTv, cts.Token, shouldGetSmallPosters: true);
			}
			catch (Exception ex)
			{
				return new TmdbToolErrorResponse(requestId, "TMDB search failed: " + ex.Message);
			}

			var results = new List<TmdbToolSearchResult>();
			if (data?.Results != null)
			{
				foreach (var r in data.Results)
				{
					results.Add(new TmdbToolSearchResult
					{
						TmdbId = r.Id,
						IsTv = r.IsTv,
						Title = r.Title,
						Overview = r.Overview,
						PosterPath = r.PosterPath,
						ReleaseYear = r.ReleaseDate?.Year ?? r.FirstAirDate?.Year ?? 0,
						PosterMimeType = r.Poster != null && r.Poster.Length > 0 ? GuessImageMime(r.PosterPath) : null,
						PosterBase64 = r.Poster != null && r.Poster.Length > 0 ? Convert.ToBase64String(r.Poster) : null,
					});
				}
			}

			return new TmdbToolSearchResponse
			{
				RequestId = requestId,
				Results = results
			};
		}

		private static async Task<object> HandleApplyAsync(string requestId, JsonElement payload)
		{
			long titleId = ReadLong(payload, "TitleId");
			int tmdbId = ReadInt(payload, "TmdbId");
			bool isTv = ReadBool(payload, "IsTv");
			bool overrideTitle = ReadBool(payload, "OverrideTitle");
			bool overrideDescription = ReadBool(payload, "OverrideDescription");
			bool overrideYear = ReadBool(payload, "OverrideYear");

			if (titleId <= 0) return new TmdbToolErrorResponse(requestId, "TitleId is required.");
			if (tmdbId <= 0) return new TmdbToolErrorResponse(requestId, "TmdbId is required.");

			using var db = DB.GetDatabase();
			var title = (await db.FetchAsync<Title>("WHERE TITLE_ID = @0", titleId)).FirstOrDefault();
			if (title == null) return new TmdbToolErrorResponse(requestId, "Title not found.");

			TmdbResult detail;
			using (var cts = new CancellationTokenSource(HttpTimeout))
			{
				try
				{
					detail = await TmdbResult.GetDetail(tmdbId, isTv, cts.Token);
				}
				catch (Exception ex)
				{
					return new TmdbToolErrorResponse(requestId, "TMDB detail fetch failed: " + ex.Message);
				}
			}
			if (detail == null) return new TmdbToolErrorResponse(requestId, "TMDB returned no detail for the selected result.");

			byte[] posterBytes = null;
			if (!string.IsNullOrEmpty(detail.PosterPath))
			{
				using var cts = new CancellationTokenSource(HttpTimeout);
				try
				{
					await detail.GetPoster(false, cts.Token);
					posterBytes = detail.Poster;
				}
				catch
				{
					// Posters are best-effort: a failure here shouldn't block the title update.
				}
			}

			title.DateModifiedUtc = GeneralPersistense.GetTimestamp();
			if (overrideDescription && !string.IsNullOrWhiteSpace(detail.Overview))
				title.Description = detail.Overview;
			if (overrideTitle && !string.IsNullOrWhiteSpace(detail.Title))
				title.TitleName = detail.Title;
			if (overrideYear && detail.ReleaseDate.HasValue)
				title.Year = detail.ReleaseDate.Value.Year;
			if (!string.IsNullOrWhiteSpace(detail.ImdbId))
				title.ImdbId = detail.ImdbId;
			title.TitleName ??= "";
			await GeneralPersistense.Upsert(title);

			long? imageId = null;
			if (posterBytes != null && posterBytes.Length > 0)
			{
				var sample = await MediaSamplePersistence.AddSample(
					posterBytes,
					title.Id,
					MediaSampleKind.Image,
					Path.GetExtension(detail.PosterPath ?? "") ?? "");
				imageId = sample.Id;
			}

			return new TmdbToolApplyResponse
			{
				RequestId = requestId,
				Title = ToQueueItem(title),
				ImageId = imageId
			};
		}

		private static async Task<object> HandleSaveManualAsync(string requestId, JsonElement payload)
		{
			long titleId = ReadLong(payload, "TitleId");
			if (titleId <= 0) return new TmdbToolErrorResponse(requestId, "TitleId is required.");

			using var db = DB.GetDatabase();
			var title = (await db.FetchAsync<Title>("WHERE TITLE_ID = @0", titleId)).FirstOrDefault();
			if (title == null) return new TmdbToolErrorResponse(requestId, "Title not found.");

			bool modified = false;
			if (TryReadString(payload, "Description", out var desc))
			{
				title.Description = desc ?? "";
				modified = true;
			}
			if (TryReadInt(payload, "Year", out var year))
			{
				title.Year = year;
				modified = true;
			}
			if (modified)
			{
				title.DateModifiedUtc = GeneralPersistense.GetTimestamp();
				await GeneralPersistense.Upsert(title);
			}

			return new TmdbToolSaveManualResponse
			{
				RequestId = requestId,
				Title = ToQueueItem(title)
			};
		}

		private static TmdbToolTitle ToQueueItem(Title t) => new TmdbToolTitle
		{
			Id = t.Id,
			TitleName = t.TitleName ?? "",
			Year = t.Year,
			Description = t.Description ?? "",
			ImdbId = t.ImdbId ?? "",
			Kind = (int)t.Kind
		};

		private static string GuessImageMime(string path)
		{
			var ext = Path.GetExtension(path ?? "").TrimStart('.').ToLowerInvariant();
			return ext switch
			{
				"png" => "image/png",
				"gif" => "image/gif",
				"webp" => "image/webp",
				"bmp" => "image/bmp",
				_ => "image/jpeg"
			};
		}

		private static string ReadString(JsonElement payload, string name)
		{
			if (payload.ValueKind != JsonValueKind.Object) return null;
			if (!payload.TryGetProperty(name, out var prop)) return null;
			return prop.ValueKind == JsonValueKind.String ? prop.GetString() : null;
		}

		private static bool TryReadString(JsonElement payload, string name, out string value)
		{
			value = null;
			if (payload.ValueKind != JsonValueKind.Object) return false;
			if (!payload.TryGetProperty(name, out var prop)) return false;
			if (prop.ValueKind == JsonValueKind.String) { value = prop.GetString(); return true; }
			if (prop.ValueKind == JsonValueKind.Null) { value = null; return true; }
			return false;
		}

		private static long ReadLong(JsonElement payload, string name)
		{
			if (payload.ValueKind != JsonValueKind.Object) return 0;
			if (!payload.TryGetProperty(name, out var prop)) return 0;
			if (prop.ValueKind == JsonValueKind.Number && prop.TryGetInt64(out var v)) return v;
			if (prop.ValueKind == JsonValueKind.String && long.TryParse(prop.GetString(), out var s)) return s;
			return 0;
		}

		private static int ReadInt(JsonElement payload, string name)
		{
			return (int)ReadLong(payload, name);
		}

		private static bool TryReadInt(JsonElement payload, string name, out int value)
		{
			value = 0;
			if (payload.ValueKind != JsonValueKind.Object) return false;
			if (!payload.TryGetProperty(name, out var prop)) return false;
			if (prop.ValueKind == JsonValueKind.Number && prop.TryGetInt32(out var v)) { value = v; return true; }
			if (prop.ValueKind == JsonValueKind.String && int.TryParse(prop.GetString(), out var s)) { value = s; return true; }
			return false;
		}

		private static bool ReadBool(JsonElement payload, string name)
		{
			if (payload.ValueKind != JsonValueKind.Object) return false;
			if (!payload.TryGetProperty(name, out var prop)) return false;
			if (prop.ValueKind == JsonValueKind.True) return true;
			if (prop.ValueKind == JsonValueKind.False) return false;
			if (prop.ValueKind == JsonValueKind.String && bool.TryParse(prop.GetString(), out var b)) return b;
			return false;
		}
	}

	public sealed class TmdbToolTitle
	{
		public long Id { get; set; }
		public string TitleName { get; set; }
		public int Year { get; set; }
		public string Description { get; set; }
		public string ImdbId { get; set; }
		public int Kind { get; set; }
	}

	public sealed class TmdbToolInitResponse
	{
		public string RequestId { get; set; }
		public List<TmdbToolTitle> Titles { get; set; }
	}

	public sealed class TmdbToolSearchResult
	{
		public int TmdbId { get; set; }
		public bool IsTv { get; set; }
		public string Title { get; set; }
		public string Overview { get; set; }
		public string PosterPath { get; set; }
		public int ReleaseYear { get; set; }
		public string PosterMimeType { get; set; }
		public string PosterBase64 { get; set; }
	}

	public sealed class TmdbToolSearchResponse
	{
		public string RequestId { get; set; }
		public List<TmdbToolSearchResult> Results { get; set; }
	}

	public sealed class TmdbToolApplyResponse
	{
		public string RequestId { get; set; }
		public TmdbToolTitle Title { get; set; }
		public long? ImageId { get; set; }
	}

	public sealed class TmdbToolSaveManualResponse
	{
		public string RequestId { get; set; }
		public TmdbToolTitle Title { get; set; }
	}

	public sealed class TmdbToolErrorResponse
	{
		public TmdbToolErrorResponse(string requestId, string error)
		{
			RequestId = requestId;
			Error = error;
		}

		public string RequestId { get; set; }
		public string Error { get; set; }
	}
}
