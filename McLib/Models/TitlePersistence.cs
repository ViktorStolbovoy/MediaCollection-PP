using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MediaCollection
{
	public static class TitlePersistence
	{
		// Returns the SQL fragment to append to a WHERE clause that already has at
		// least one preceding condition. Always starts with " and " (or is empty when
		// the caller wants no filter on HIDDEN at all).
		private static string BuildHiddenClause(HiddenVisibility hiddenVisibility)
		{
			switch (hiddenVisibility)
			{
				case HiddenVisibility.Include:
					return string.Empty;
				case HiddenVisibility.Only:
					return " and HIDDEN = 1";
				case HiddenVisibility.None:
				default:
					return " and HIDDEN = 0";
			}
		}

		// Backward-compatible overload: no parent filter is applied. Provided so that
		// existing call sites passing TitleKind values immediately after hiddenVisibility
		// keep compiling (TitleKind does not convert to long?, so the compiler can't pick
		// the overload below for them).
		public static Task<List<Title>> ListTitles(string pattern, HiddenVisibility hiddenVisibility, params TitleKind[] kinds)
			=> ListTitles(pattern, hiddenVisibility, parentTitleId: null, kinds: kinds);

		// parentTitleId: when null, no parent filter is applied; when set, results are
		// restricted to rows whose PARENT_TITLE_ID equals the value. There is no way
		// through this parameter alone to ask for "roots only" (PARENT_TITLE_ID IS NULL)
		// - callers needing that filter post-fetch.
		public static async Task<List<Title>> ListTitles(string pattern, HiddenVisibility hiddenVisibility, long? parentTitleId, params TitleKind[] kinds)
		{
			if (kinds == null || kinds.Length == 0)
			{
				throw new ArgumentException("At least one kind must be specified.", nameof(kinds));
			}

			using (var db = DB.GetDatabase())
			{
				var args = new List<object>(kinds.Length + 2);
				foreach (var k in kinds) args.Add(k);

				string kindPlaceholders = string.Join(", ", Enumerable.Range(0, kinds.Length).Select(i => "@" + i));

				string patternClause = string.Empty;
				if (!string.IsNullOrWhiteSpace(pattern))
				{
					args.Add(pattern);
					patternClause = $" and TITLE_NAME like @{args.Count - 1}";
				}

				string parentClause = string.Empty;
				if (parentTitleId.HasValue)
				{
					args.Add(parentTitleId.Value);
					parentClause = $" and PARENT_TITLE_ID = @{args.Count - 1}";
				}

				string hiddenClause = BuildHiddenClause(hiddenVisibility);

				string sql = $"where KIND IN ({kindPlaceholders}){patternClause}{parentClause}{hiddenClause} ORDER BY TITLE_NAME, ORD";
				return await db.FetchAsync<Title>(sql, args.ToArray());
			}
		}

		public static async Task<List<Title>> ListRootVideo(HiddenVisibility hiddenVisibility)
		{
			var list = await ListTitles(null, hiddenVisibility,
				TitleKind.Episode, TitleKind.Season, TitleKind.Title, TitleKind.Series, TitleKind.Disk);
			return list.Where(t => !t.ParentTitleId.HasValue).ToList();
		}

		public static async Task<List<Title>> ListRootAudio(HiddenVisibility hiddenVisibility)
		{
			var list = await ListTitles(null, hiddenVisibility,
				TitleKind.Album, TitleKind.Track, TitleKind.AlbumArtist);
			return list.Where(t => !t.ParentTitleId.HasValue).ToList();
		}

		public static async Task<List<Title>> ListTitlesByParent(long parentTitleId)
		{
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<Title>("select * from TITLE where PARENT_TITLE_ID = @0 ORDER BY ORD, TITLE_NAME", parentTitleId);
			}
		}

		public static async Task<List<Title>> GetTitlesForAutoupdate()
		{
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<Title>("WHERE DESCRIPTION = '' AND RELEASE_YEAR = 0 and (KIND =@0 or KIND = @1) and HIDDEN = 0 ORDER BY TITLE_NAME, ORD", TitleKind.Title, TitleKind.Series);
			}
		}

		public static async Task<Title> AddTitle(string name, TitleKind kind, int season, int disk, int episodeOrTrack, long? parentId)
		{

			string now = GeneralPersistense.GetTimestamp();
			var t = new Title { TitleName = name, Kind = kind, Season = season, Disk = disk, EpisodeOrTrack = episodeOrTrack, ParentTitleId = parentId, DateAddedUtc = now, DateModifiedUtc = now, ImdbId = "", Description = "", Hidden = false };
			using (var db = DB.GetDatabase())
			{
				await db.InsertAsync(t);
			}
			return t;
		}

		public static async Task<bool> SaveTitleName(int titleId, string newName)
		{
			using (var db = DB.GetDatabase())
			{
				return await db.ExecuteAsync("UPDATE TITLE SET TITLE_NAME= @0 WHERE TITLE_ID = @1", newName, titleId) > 0;
			}
		}

		public static async Task<bool> ReparentTitle(long titleId, long? parentId)
		{
			using (var db = DB.GetDatabase())
			{
				return await db.ExecuteAsync("UPDATE title SET PARENT_TITLE_ID= @0 WHERE TITLE_ID = @1", parentId, titleId) > 0;
			}
		}

		public static async Task<bool> MoveTitle(int titleId, int ord)
		{
			using (var db = DB.GetDatabase())
			{
				return await db.ExecuteAsync("UPDATE title SET ORD= @0 WHERE TITLE_ID = @1", ord, titleId) > 0;
			}
		}

        public static async Task<bool> SetHidden(long titleId, bool hidden)
        {
            using (var db = DB.GetDatabase())
            {
                return await db.ExecuteAsync("UPDATE title SET HIDDEN = @0 WHERE TITLE_ID = @1", hidden ? 1 : 0, titleId) > 0;
            }
        }

        public static async Task<List<TitleRatingWithName>> GetRatings(long titleId)
		{
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<TitleRatingWithName>("select p.*, tr.RATING_VALUE, tr.TITLE_ID from RATING_PROVIDER p LEFT JOIN TITLE_RATING tr ON p.RATING_ID = tr.RATING_ID and tr.TITLE_ID = @0 ORDER BY p.RATING_NAME", titleId);
			}
		}

		public static async Task DeleteTitle(long titleId)
		{
			var images = await MediaSamplePersistence.GetSamples(titleId, MediaSampleKind.Image);
			if (images != null)
			{
				foreach (var img in images)
				{
					await MediaSamplePersistence.RemoveSample(img);
				}
			}
			
			using (var db = DB.GetDatabase())
			{
                await db.ExecuteAsync("DELETE FROM location WHERE TITLE_ID = @0; DELETE FROM title WHERE TITLE_ID = @0", titleId);
			}
		}
	}
}
