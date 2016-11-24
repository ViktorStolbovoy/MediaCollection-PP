﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediaCollection
{
	public static class TitlePersistence
	{
		
		public static List<Title> ListTitles(string pattern, TitleKind kind)
		{
			using (var db = DB.GetDatabase())
			{
				if (string.IsNullOrWhiteSpace(pattern))
				{
					return db.Fetch<Title>("where KIND = @0", kind);
				}
				else
				{
					pattern = "%" + pattern + "%";
					return db.Fetch<Title>("where KIND = @0 and TITLE_NAME like @1", kind, pattern);
				}
			}
		}

		public static List<Title> ListRootVideo()
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<Title>("WHERE PARENT_TITLE_ID IS NULL and (KIND =@0 or KIND = @1 or KIND = @2 or KIND = @3) ORDER BY TITLE_NAME, ORD", TitleKind.Episode, TitleKind.Season, TitleKind.Title, TitleKind.Series);
			}
		}

		public static List<Title> ListRootAudio()
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<Title>("WHERE PARENT_TITLE_ID IS NULL and (KIND = @0 or KIND = @1 or KIND = @2) ORDER BY TITLE_NAME, ORD", TitleKind.Album, TitleKind.Track, TitleKind.AlbumArtist);
			}
		}

		public static List<Title> SearchTitlesByLocationBaseId(long locationBaseId)
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<Title>("select t.* from TITLE t join LOCATION l on l.TITLE_ID = t.TITLE_ID where l.LOCATION_BASE_IDKIND = @0", locationBaseId);
			}
		}

		public static List<Title> ListTitlesByParent(long parentTitleId)
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<Title>("select * from TITLE where PARENT_TITLE_ID = @0 ORDER BY ORD, TITLE_NAME", parentTitleId);
			}
		}

		public static long AddTitle(string name, TitleKind kind, int ord, int? parentId)
		{

			string now = GeneralPersistense.GetTimestamp();
			var t = new Title { TitleName = name, Kind = kind, Ord = ord, ParentTitleId = parentId, DateAddedUtc = now, DateModifiedUtc = now };
			using (var db = DB.GetDatabase())
			{
				db.Insert(t);
			}
			return t.Id;
		}

		public static bool SaveTitleName(int titleId, string newName)
		{
			using (var db = DB.GetDatabase())
			{
				return db.Execute("UPDATE TITLE SET TITLE_NAME= @0 WHERE TITLE_ID = @1", newName, titleId) > 0;
			}
		}

		public static bool ReparentTitle(int titleId, int? parentId)
		{
			using (var db = DB.GetDatabase())
			{
				return db.Execute("UPDATE title SET PARENT_TITLE_ID= @0 WHERE TITLE_ID = @1", parentId, titleId) > 0;
			}
		}

		public static bool MoveTitle(int titleId, int ord)
		{
			using (var db = DB.GetDatabase())
			{
				return db.Execute("UPDATE title SET ORD= @0 WHERE TITLE_ID = @1", ord, titleId) > 0;
			}
		}

		public static List<TitleRatingWithName> GetRatings(long titleId) 
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<TitleRatingWithName>("select p.*, tr.RATING_VALUE, tr.TITLE_ID from RATING_PROVIDER p LEFT JOIN TITLE_RATING tr ON p.RATING_ID = tr.RATING_ID and tr.TITLE_ID = @0 ORDER BY p.RATING_NAME", titleId);
			}
		}
	}
}
