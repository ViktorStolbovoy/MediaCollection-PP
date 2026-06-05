using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaCollection.WebSockets
{
	internal static class LibrarySnapshotBuilder
	{
		public static async Task<LibrarySnapshot> BuildAsync(LibrarySubscription sub)
		{
			List<Title> roots = sub.ResourceKind == "audio"
				? await TitlePersistence.ListRootAudio(sub.IncludeHidden)
				: await TitlePersistence.ListRootVideo(sub.IncludeHidden);
			roots.Sort();

			LibrarySnapshotDetail detail = null;
			if (sub.SelectedTitleId.HasValue)
			{
				detail = await BuildDetailAsync(sub.SelectedTitleId.Value);
			}

			return new LibrarySnapshot
			{
				ResourceKind = sub.ResourceKind,
				IncludeHidden = sub.IncludeHidden,
				SelectedTitleId = sub.SelectedTitleId,
				Roots = roots,
				Detail = detail
			};
		}

		private static async Task<LibrarySnapshotDetail> BuildDetailAsync(long id)
		{
			using var db = DB.GetDatabase();
			var rows = await db.FetchAsync<Title>("WHERE TITLE_ID = @0", id);
			var title = rows.FirstOrDefault();
			if (title == null) return null;

			return new LibrarySnapshotDetail
			{
				Title = title,
				Locations = await LocationPersistence.ListTitleLocations(id),
				Ratings = await TitlePersistence.GetRatings(id),
				Images = await MediaSamplePersistence.GetSamples(id, MediaSampleKind.Image)
			};
		}
	}

	public sealed class LibrarySnapshot
	{
		public string ResourceKind { get; set; }
		public bool IncludeHidden { get; set; }
		public long? SelectedTitleId { get; set; }
		public List<Title> Roots { get; set; }
		public LibrarySnapshotDetail Detail { get; set; }
	}

	public sealed class LibrarySnapshotDetail
	{
		public Title Title { get; set; }
		public List<LocationForDisplay> Locations { get; set; }
		public List<TitleRatingWithName> Ratings { get; set; }
		public List<MediaSample> Images { get; set; }
	}
}
