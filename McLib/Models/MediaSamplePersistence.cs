using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCollection
{
	public static class MediaSamplePersistence
	{
		public static string s_dataFolder = Settings.Get<string>("MEDIA_PATH");
		public static MediaSample AddSample(byte[] data, long titleId, MediaSampleKind kind, string extension)
		{
			if (data == null) throw new ArgumentNullException("data", "Sample data can't be null");
			var ms = new MediaSample { TitleId = titleId, Extension = extension, MediaKind = kind };
			using(var db = DB.GetDatabase())
			{
				db.Insert(ms);
			}
			string fn = GetSampleFileName(ms);
			File.WriteAllBytes(fn, data);
			return ms;
		}

		public static void RemoveSample(MediaSample ms)
		{
			string fn = GetSampleFileName(ms);
			File.Delete(fn);
			using (var db = DB.GetDatabase())
			{
				db.Delete(ms);
			}
		}
		

		public static List<MediaSample> GetSamples(long titleId, MediaSampleKind kind)
		{
			using (var db = DB.GetDatabase())
			{
				var res = db.Fetch<MediaSample>("SELECT * FROM MEDIA_SAMPLE WHERE TITLE_ID = @0 and MEDIA_KIND = @1", titleId, kind);
				return res;
			}
		}



		/*public static ... GetVideos()
		{

		} */

		private static string GetSampleFileName(MediaSample ms)
		{
			if(ms.TitleId < 1) throw new ApplicationException("Can't manipulate media sample file before title was saved");
			if(ms.Id < 1) throw new ApplicationException("Can't manipulate media sample file before metadata was saved");
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

		public static Image GetImage(this MediaSample item)
		{
			using(var strm = item.GetData())
			{
				return Image.FromStream(strm);
			}
		}
	}
}
