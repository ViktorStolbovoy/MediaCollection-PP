using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Stolbovoy.Utils;

namespace MediaCollection.WebSockets
{
	/// <summary>
	/// Handles request/response style WebSocket messages used by the Seasons Tool UI.
	/// Each request carries a <c>RequestId</c> that is echoed back so the client can
	/// correlate responses without subscribing to bespoke per-call channels.
	/// </summary>
	internal static class SeasonsToolHandler
	{
		public const string InitType = "season-tool-init";
		public const string SearchType = "season-tool-search";
		public const string CreateSeriesType = "season-tool-create-series";
		public const string CreateSeasonsType = "season-tool-create-seasons";
		public const string AutoMoveType = "season-tool-auto-move";
		public const string FindConvertType = "season-tool-find-titles-to-convert";
		public const string ApplyConvertType = "season-tool-apply-conversion";

		public static bool IsSeasonsToolMessage(string type)
		{
			return type != null && type.StartsWith("season-tool-", StringComparison.OrdinalIgnoreCase);
		}

		public static async Task<(string ResponseType, object Response, bool Mutated)> HandleAsync(string type, JsonElement payload)
		{
			var requestId = ReadRequestId(payload);
			switch (type)
			{
				case InitType:
					return (InitType + "-response", await HandleInitAsync(requestId, payload), false);
				case SearchType:
					return (SearchType + "-response", await HandleSearchAsync(requestId, payload), false);
				case CreateSeriesType:
					return (CreateSeriesType + "-response", await HandleCreateSeriesAsync(requestId, payload), true);
				case CreateSeasonsType:
					return (CreateSeasonsType + "-response", await HandleCreateSeasonsAsync(requestId, payload), true);
				case AutoMoveType:
					return (AutoMoveType + "-response", await HandleAutoMoveAsync(requestId, payload), true);
				case FindConvertType:
					return (FindConvertType + "-response", await HandleFindConvertAsync(requestId, payload), false);
				case ApplyConvertType:
					return (ApplyConvertType + "-response", await HandleApplyConvertAsync(requestId, payload), true);
				default:
					return (null, null, false);
			}
		}

		private static async Task<object> HandleInitAsync(string requestId, JsonElement payload)
		{
			long titleId = ReadLong(payload, "TitleId");
			if (titleId <= 0) return new SeasonsToolErrorResponse(requestId, "TitleId is required.");

			using var db = DB.GetDatabase();
			var rows = await db.FetchAsync<Title>("WHERE TITLE_ID = @0", titleId);
			var title = rows.FirstOrDefault();
			if (title == null) return new SeasonsToolErrorResponse(requestId, "Title not found.");

			string seriesName;
			Title series = null;
			bool isSeriesTitle = false;

			switch (title.Kind)
			{
				case TitleKind.Series:
					seriesName = title.TitleName ?? "";
					series = title;
					isSeriesTitle = true;
					break;
				case TitleKind.Season:
				{
					seriesName = title.TitleName ?? "";
					int idx = seriesName.IndexOf(" season", StringComparison.OrdinalIgnoreCase);
					if (idx > 0) seriesName = seriesName.Substring(0, idx);
					series = await FindUniqueSeries(seriesName);
					break;
				}
				case TitleKind.Disk:
					seriesName = StoredItem.ParseSeasonDiskEpisode(title.TitleName ?? "").Name ?? "";
					series = await FindUniqueSeries(seriesName);
					break;
				case TitleKind.Episode:
				{
					seriesName = title.TitleName ?? "";
					int idx = seriesName.IndexOf(" episode", StringComparison.OrdinalIgnoreCase);
					if (idx > 0) seriesName = seriesName.Substring(0, idx);
					series = await FindUniqueSeries(seriesName);
					break;
				}
				default:
					seriesName = title.TitleName ?? "";
					break;
			}

			return new SeasonsToolInitResponse
			{
				RequestId = requestId,
				SeriesName = seriesName,
				SeriesId = series?.Id,
				IsSeriesTitle = isSeriesTitle
			};
		}

		private static async Task<object> HandleSearchAsync(string requestId, JsonElement payload)
		{
			string pattern = ReadString(payload, "Pattern") ?? "";

			var seasons = await TitlePersistence.ListTitles(pattern, false, TitleKind.Season);
			var disksAndEpisodes = await TitlePersistence.ListTitles(pattern, false, TitleKind.Disk, TitleKind.Episode);

			var seasonItems = seasons.Select(ToItem).ToList();
			var disksAndEpisodesItems = disksAndEpisodes.Select(ToItem).ToList();

			var seasonsFound = new HashSet<int>(seasonItems.Select(s => s.Season));
			var missing = new HashSet<int>();
			foreach (var it in disksAndEpisodesItems)
			{
				if (it.Season > 0 && !seasonsFound.Contains(it.Season)) missing.Add(it.Season);
			}

			// Try to resolve series id from the pattern (strip trailing wildcard "%" if any)
			Title series = null;
			string seriesGuess = StripPatternWildcard(pattern);
			if (!string.IsNullOrWhiteSpace(seriesGuess))
			{
				series = await FindUniqueSeries(seriesGuess);
			}

			return new SeasonsToolSearchResponse
			{
				RequestId = requestId,
				SeriesId = series?.Id,
				Seasons = seasonItems,
				Items = disksAndEpisodesItems,
				SeasonsToCreate = missing.OrderBy(i => i).ToList()
			};
		}

		private static async Task<object> HandleCreateSeriesAsync(string requestId, JsonElement payload)
		{
			string name = (ReadString(payload, "Name") ?? "").Trim();
			if (string.IsNullOrWhiteSpace(name)) return new SeasonsToolErrorResponse(requestId, "Name is required.");

			var existing = await FindUniqueSeries(name);
			if (existing != null)
			{
				return new SeasonsToolCreateSeriesResponse { RequestId = requestId, Series = ToItem(existing) };
			}

			var series = await TitlePersistence.AddTitle(name, TitleKind.Series, 0, 0, 0, null);
			return new SeasonsToolCreateSeriesResponse { RequestId = requestId, Series = ToItem(series) };
		}

		private static async Task<object> HandleCreateSeasonsAsync(string requestId, JsonElement payload)
		{
			long seriesId = ReadLong(payload, "SeriesId");
			string pattern = ReadString(payload, "Pattern") ?? "";
			var seasons = ReadIntList(payload, "Seasons");
			if (seriesId <= 0) return new SeasonsToolErrorResponse(requestId, "Need to create series first");
			if (string.IsNullOrWhiteSpace(pattern)) return new SeasonsToolErrorResponse(requestId, "Pattern is required.");

			string template = pattern.Replace("%", "Season {0}");
			var existingSeasons = await TitlePersistence.ListTitles(pattern, false, TitleKind.Season);
			var existingSet = new HashSet<int>(existingSeasons.Select(s => s.Season));

			var created = new List<SeasonsToolItem>();
			foreach (int s in seasons.Distinct().OrderBy(i => i))
			{
				if (s <= 0 || existingSet.Contains(s)) continue;
				var name = string.Format(template, s);
				var title = await TitlePersistence.AddTitle(name, TitleKind.Season, s, 0, 0, seriesId);
				created.Add(ToItem(title));
			}

			return new SeasonsToolCreateSeasonsResponse { RequestId = requestId, Created = created };
		}

		private static async Task<object> HandleAutoMoveAsync(string requestId, JsonElement payload)
		{
			long seriesId = ReadLong(payload, "SeriesId");
			var seasonIds = ReadLongList(payload, "SeasonIds");
			var itemIds = ReadLongList(payload, "ItemIds");
			if (seriesId <= 0) return new SeasonsToolErrorResponse(requestId, "Need to create series first");

			using var db = DB.GetDatabase();
			var series = (await db.FetchAsync<Title>("WHERE TITLE_ID = @0", seriesId)).FirstOrDefault();
			if (series == null) return new SeasonsToolErrorResponse(requestId, "Series not found.");

			int movedSeasons = 0;
			foreach (var sid in seasonIds.Distinct())
			{
				var season = (await db.FetchAsync<Title>("WHERE TITLE_ID = @0", sid)).FirstOrDefault();
				if (season == null) continue;
				if (season.Kind != TitleKind.Season) continue;
				if (!season.ParentTitleId.HasValue)
				{
					await TitlePersistence.ReparentTitle(season.Id, seriesId);
					movedSeasons++;
				}
			}

			var seasonByNumber = new Dictionary<int, long>();
			foreach (var child in await TitlePersistence.ListTitlesByParent(seriesId))
			{
				if (child.Kind == TitleKind.Season && !seasonByNumber.ContainsKey(child.Season))
				{
					seasonByNumber[child.Season] = child.Id;
				}
			}

			int movedItems = 0;
			foreach (var iid in itemIds.Distinct())
			{
				var item = (await db.FetchAsync<Title>("WHERE TITLE_ID = @0", iid)).FirstOrDefault();
				if (item == null) continue;
				if (item.Kind != TitleKind.Disk && item.Kind != TitleKind.Episode) continue;
				if (item.ParentTitleId.HasValue) continue;
				if (!seasonByNumber.TryGetValue(item.Season, out long parentId)) continue;
				await TitlePersistence.ReparentTitle(item.Id, parentId);
				movedItems++;
			}

			return new SeasonsToolAutoMoveResponse
			{
				RequestId = requestId,
				MovedSeasons = movedSeasons,
				MovedItems = movedItems
			};
		}

		private static async Task<object> HandleFindConvertAsync(string requestId, JsonElement payload)
		{
			string pattern = ReadString(payload, "Pattern") ?? "";
			string regexText = ReadString(payload, "Regex") ?? "";
			if (string.IsNullOrWhiteSpace(regexText))
			{
				return new SeasonsToolErrorResponse(requestId, "Regex is required.");
			}

			Regex regex;
			try
			{
				regex = new Regex(regexText);
			}
			catch (ArgumentException ex)
			{
				return new SeasonsToolErrorResponse(requestId, "Invalid regex: " + ex.Message);
			}

			var candidates = await TitlePersistence.ListTitles(pattern, false, TitleKind.Title, TitleKind.Disk, TitleKind.Episode);
			var hits = new List<SeasonsToolConvertItem>();
			foreach (var t in candidates)
			{
				var m = regex.Match(t.TitleName ?? "");
				if (!m.Success || m.Groups.Count <= 2) continue;
				int season = m.Groups["s"].Value.To<int>(0);
				int disk = m.Groups["d"].Value.To<int>(0);
				int episode = m.Groups["e"].Value.To<int>(0);
				if (disk <= 0 && episode <= 0) continue;
				hits.Add(new SeasonsToolConvertItem
				{
					Id = t.Id,
					TitleName = t.TitleName,
					Kind = (int)(episode > 0 ? TitleKind.Episode : TitleKind.Disk),
					Season = season,
					Disk = disk,
					Episode = episode
				});
			}

			return new SeasonsToolFindConvertResponse { RequestId = requestId, Titles = hits };
		}

		private static async Task<object> HandleApplyConvertAsync(string requestId, JsonElement payload)
		{
			if (!payload.TryGetProperty("Titles", out var arr) || arr.ValueKind != JsonValueKind.Array)
			{
				return new SeasonsToolErrorResponse(requestId, "Titles array is required.");
			}

			int updated = 0;
			using var db = DB.GetDatabase();
			foreach (var el in arr.EnumerateArray())
			{
				long id = ReadLong(el, "Id");
				if (id <= 0) continue;
				var title = (await db.FetchAsync<Title>("WHERE TITLE_ID = @0", id)).FirstOrDefault();
				if (title == null) continue;
				int kind = ReadInt(el, "Kind");
				int season = ReadInt(el, "Season");
				int disk = ReadInt(el, "Disk");
				int episode = ReadInt(el, "Episode");
				title.Kind = (TitleKind)kind;
				title.Season = season;
				title.Disk = disk;
				title.EpisodeOrTrack = episode;
				title.DateModifiedUtc = GeneralPersistense.GetTimestamp();
				await GeneralPersistense.Upsert(title);
				updated++;
			}

			return new SeasonsToolApplyConvertResponse { RequestId = requestId, Updated = updated };
		}

		private static async Task<Title> FindUniqueSeries(string seriesName)
		{
			if (string.IsNullOrWhiteSpace(seriesName)) return null;
			var list = await TitlePersistence.ListTitles(seriesName, false, TitleKind.Series);
			return list != null && list.Count == 1 ? list[0] : null;
		}

		private static string StripPatternWildcard(string pattern)
		{
			if (string.IsNullOrEmpty(pattern)) return pattern;
			var trimmed = pattern.TrimEnd();
			if (trimmed.EndsWith("%")) trimmed = trimmed.Substring(0, trimmed.Length - 1);
			return trimmed.Trim();
		}

		private static SeasonsToolItem ToItem(Title t) => new SeasonsToolItem
		{
			Id = t.Id,
			TitleName = t.TitleName,
			Kind = (int)t.Kind,
			Season = t.Season,
			Disk = t.Disk,
			EpisodeOrTrack = t.EpisodeOrTrack,
			ParentTitleId = t.ParentTitleId
		};

		private static string ReadRequestId(JsonElement payload)
		{
			return ReadString(payload, "RequestId") ?? "";
		}

		private static string ReadString(JsonElement payload, string name)
		{
			if (payload.ValueKind != JsonValueKind.Object) return null;
			if (!payload.TryGetProperty(name, out var prop)) return null;
			return prop.ValueKind == JsonValueKind.String ? prop.GetString() : null;
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

		private static List<long> ReadLongList(JsonElement payload, string name)
		{
			var result = new List<long>();
			if (payload.ValueKind != JsonValueKind.Object) return result;
			if (!payload.TryGetProperty(name, out var prop) || prop.ValueKind != JsonValueKind.Array) return result;
			foreach (var el in prop.EnumerateArray())
			{
				if (el.ValueKind == JsonValueKind.Number && el.TryGetInt64(out var v)) result.Add(v);
				else if (el.ValueKind == JsonValueKind.String && long.TryParse(el.GetString(), out var s)) result.Add(s);
			}
			return result;
		}

		private static List<int> ReadIntList(JsonElement payload, string name)
		{
			return ReadLongList(payload, name).Select(v => (int)v).ToList();
		}
	}

	public sealed class SeasonsToolItem
	{
		public long Id { get; set; }
		public string TitleName { get; set; }
		public int Kind { get; set; }
		public int Season { get; set; }
		public int Disk { get; set; }
		public int EpisodeOrTrack { get; set; }
		public long? ParentTitleId { get; set; }
	}

	public sealed class SeasonsToolInitResponse
	{
		public string RequestId { get; set; }
		public string SeriesName { get; set; }
		public long? SeriesId { get; set; }
		public bool IsSeriesTitle { get; set; }
	}

	public sealed class SeasonsToolSearchResponse
	{
		public string RequestId { get; set; }
		public long? SeriesId { get; set; }
		public List<SeasonsToolItem> Seasons { get; set; }
		public List<SeasonsToolItem> Items { get; set; }
		public List<int> SeasonsToCreate { get; set; }
	}

	public sealed class SeasonsToolCreateSeriesResponse
	{
		public string RequestId { get; set; }
		public SeasonsToolItem Series { get; set; }
	}

	public sealed class SeasonsToolCreateSeasonsResponse
	{
		public string RequestId { get; set; }
		public List<SeasonsToolItem> Created { get; set; }
	}

	public sealed class SeasonsToolAutoMoveResponse
	{
		public string RequestId { get; set; }
		public int MovedSeasons { get; set; }
		public int MovedItems { get; set; }
	}

	public sealed class SeasonsToolConvertItem
	{
		public long Id { get; set; }
		public string TitleName { get; set; }
		public int Kind { get; set; }
		public int Season { get; set; }
		public int Disk { get; set; }
		public int Episode { get; set; }
	}

	public sealed class SeasonsToolFindConvertResponse
	{
		public string RequestId { get; set; }
		public List<SeasonsToolConvertItem> Titles { get; set; }
	}

	public sealed class SeasonsToolApplyConvertResponse
	{
		public string RequestId { get; set; }
		public int Updated { get; set; }
	}

	public sealed class SeasonsToolErrorResponse
	{
		public SeasonsToolErrorResponse(string requestId, string error)
		{
			RequestId = requestId;
			Error = error;
		}

		public string RequestId { get; set; }
		public string Error { get; set; }
	}
}
