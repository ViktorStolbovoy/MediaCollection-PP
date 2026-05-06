using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaCollection
{
	public static class TitleHierarchyService
	{
		public static bool CanDrop(Title source, Title destination)
		{
			if (source == null || destination == null) return false;

			switch (destination.Kind)
			{
				case TitleKind.Album: return source.Kind == TitleKind.Track;
				case TitleKind.AlbumArtist: return source.Kind == TitleKind.Track || source.Kind == TitleKind.Album;
				case TitleKind.Disk: return source.Kind == TitleKind.Episode || source.Kind == TitleKind.Title;
				case TitleKind.Episode: return false;
				case TitleKind.Season: return source.Kind == TitleKind.Episode || source.Kind == TitleKind.Title || source.Kind == TitleKind.Disk;
				case TitleKind.Series: return source.Kind == TitleKind.Season || source.Kind == TitleKind.Episode || source.Kind == TitleKind.Title || source.Kind == TitleKind.Disk;
				default: return false;
			}
		}

		public static void ApplyReparent(Title item, Title parent)
		{
			long parentId = parent.Id;
			switch (parent.Kind)
			{
				case TitleKind.Season:
					if (item.Kind == TitleKind.Title) item.Kind = TitleKind.Disk;
					if (parent.Season > 0) item.Season = parent.Season;
					break;
				case TitleKind.Series:
					if (item.Kind != TitleKind.Season && item.Season > 0)
					{
						long seasonId = -1;
						foreach (var title in TitlePersistence.ListTitlesByParent(parentId))
						{
							if (title.Kind == TitleKind.Season && title.Season == item.Season)
							{
								seasonId = title.Id;
								break;
							}
						}
						if (seasonId < 0)
						{
							seasonId = TitlePersistence.AddTitle(
								parent.TitleName + " Season " + item.Season.ToString(),
								TitleKind.Season,
								item.Season,
								0,
								0,
								parentId).Id;
						}
						parentId = seasonId;
					}
					break;
			}
			item.ParentTitleId = parentId;
		}

		public static string TryMove(long sourceId, long parentId)
		{
			using (var db = DB.GetDatabase())
			{
				var source = db.SingleById<Title>(sourceId);
				var parent = db.SingleById<Title>(parentId);
				if (source == null) return "Source title not found.";
				if (parent == null) return "Parent title not found.";
				if (!CanDrop(source, parent)) return "Cannot move this title under the selected parent.";
				ApplyReparent(source, parent);
				source.DateModifiedUtc = GeneralPersistense.GetTimestamp();
				GeneralPersistense.Upsert(source);
			}
			return null;
		}
	}
}
