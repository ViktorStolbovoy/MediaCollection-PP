using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MediaCollection
{
	public static class MediaSamplePersistence
	{
		public static string s_dataFolder;
		public static async Task<MediaSample> AddSample(byte[] data, long titleId, MediaSampleKind kind, string extension)
		{
			if (data == null) throw new ArgumentNullException("data", "Sample data can't be null");
			var ms = new MediaSample { TitleId = titleId, Extension = extension, MediaKind = kind };
			using (var db = DB.GetDatabase())
			{
				await db.InsertAsync(ms);
			}
			string fn = GetSampleFileName(ms);
			await File.WriteAllBytesAsync(fn, data);
			return ms;
		}

		public static async Task RemoveSample(MediaSample ms)
		{
			string fn = GetSampleFileName(ms);
			File.Delete(fn);
			using (var db = DB.GetDatabase())
			{
				await db.DeleteAsync(ms);
			}
		}


		public static async Task<List<MediaSample>> GetSamples(long titleId, MediaSampleKind kind)
		{
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<MediaSample>("SELECT * FROM MEDIA_SAMPLE WHERE TITLE_ID = @0 and MEDIA_KIND = @1", titleId, kind);
			}
		}



		/*public static ... GetVideos()
		{

		} */

		private static string GetSampleFileName(MediaSample ms)
		{
			if (ms.TitleId < 1) throw new ApplicationException("Can't manipulate media sample file before title was saved");
			if (ms.Id < 1) throw new ApplicationException("Can't manipulate media sample file before metadata was saved");
			string folder = Path.Combine(s_dataFolder, ms.TitleId.ToString());
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
			string fileName = Path.Combine(folder, ms.Id.ToString());
			if (!string.IsNullOrEmpty(ms.Extension))
			{
				if (ms.Extension[0] != '.')
				{
					fileName += "." + ms.Extension;
				}
				else
				{
					fileName += ms.Extension;
				}
			}
			return fileName;
		}

		public static Stream GetData(this MediaSample item)
		{
			string fn = GetSampleFileName(item);
			return File.OpenRead(fn);
		}
	}
}
