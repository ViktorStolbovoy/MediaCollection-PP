using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MediaCollection
{
	public static class TitlePersistence
	{
		
		public static async Task<List<Title>> ListTitles(string pattern, bool hidden, params TitleKind[] kinds)
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

				if (string.IsNullOrWhiteSpace(pattern))
				{
					args.Add(hidden ? 1 : 0);
					string sql = $"where KIND IN ({kindPlaceholders}) and HIDDEN = @{kinds.Length}";
					return await db.FetchAsync<Title>(sql, args.ToArray());
				}
				else
				{
					args.Add(pattern);
					args.Add(hidden ? 1 : 0);
					string sql = $"where KIND IN ({kindPlaceholders}) and TITLE_NAME like @{kinds.Length} and HIDDEN = @{kinds.Length + 1}";
					return await db.FetchAsync<Title>(sql, args.ToArray());
				}
			}
		}

		public static async Task<List<Title>> ListRootVideo(bool hidden)
		{
			using (var db = DB.GetDatabase())
			{
                return await db.FetchAsync<Title>("WHERE PARENT_TITLE_ID IS NULL and (KIND =@0 or KIND = @1 or KIND = @2 or KIND = @3 or KIND = @4) and HIDDEN = @5 ORDER BY TITLE_NAME, ORD", TitleKind.Episode, TitleKind.Season, TitleKind.Title, TitleKind.Series, TitleKind.Disk, hidden ? 1 : 0);
			}
		}

		public static async Task<List<Title>> ListRootAudio(bool hidden)
		{
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<Title>("WHERE PARENT_TITLE_ID IS NULL and (KIND = @0 or KIND = @1 or KIND = @2) and HIDDEN = @3 ORDER BY TITLE_NAME, ORD", TitleKind.Album, TitleKind.Track, TitleKind.AlbumArtist, hidden ? 1 : 0);
			}
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
