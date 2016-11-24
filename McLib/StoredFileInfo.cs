using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using log4net;
using Stolbovoy.Utils;

namespace MediaCollection
{
	//public enum MediaType { Unknown = 0, MediaFileAudio, MediaFileVideo, DvdFolder, BluRayFolder, DvdImage, BluRayImage, AudioFolder, Dvd, BlueRay, Cd, CdImage, Picture, PictureFolder };
	//public enum DataTypeType : byte { MediaFileVideo = 1, MediaFileAudio = 2, BluRayFolder = 3, HddvdFolder = 4, DvdFolder = 5, Picture = 6, Unknown = 0 }

	public class StoredItem : IComparable<StoredItem>
	{
		internal StoredItem(FileInfo file, MediaType dataType, string basePath, long locationBaseId)
		{
			//TODO: rename to relative path and remove base
			RelativePath =  RemoveBasePath(basePath, file.FullName);

			m_name = Path.GetFileNameWithoutExtension(file.Name);
			DataType = dataType;
			m_locationBaseId = locationBaseId;
			//Created = file.CreationTime;
			//Size = file.Length;
		}

		internal StoredItem(DirectoryInfo dir, MediaType dataType, string basePath, long locationBaseId)
		{
			RelativePath = RemoveBasePath(basePath, dir.FullName);
			m_name = dir.Name;
			DataType = dataType;
			m_locationBaseId = locationBaseId;
			//Created = dir.CreationTime;
			//Size = GetDirSize(dir);
		}

		private string RemoveBasePath(string basePath, string path)
		{
			if (string.IsNullOrEmpty(path)) throw new ArgumentException("Path can't be empty", "path");
			if (string.IsNullOrEmpty(basePath)) throw new ArgumentException("BasePath can't be empty", "basePath");

			string bp = Path.GetFullPath(basePath.Trim());
			string p = Path.GetFullPath(path.Trim());
			if (p.StartsWith(bp, StringComparison.OrdinalIgnoreCase)) p = p.Substring(basePath.Length);
			if (p.Length > 0 && (p[0] == '\\' || p[0] == '/')) p = p.Substring(1);
			return p;
		}

		private long m_locationBaseId;

		private string m_name;

		/*static long GetDirSize(DirectoryInfo dir)
		{
			long res = 0;
			foreach (var fi in dir.GetFiles())
			{
				res += fi.Length;
			}

			foreach (var di in dir.GetDirectories())
			{
				res += GetDirSize(di);
			}
			return res;
		}*/

		/*public override string ToString()
		{
			return Title;
		} */

		public string Title
		{
			get
			{
				if (m_title == null)
				{
					ProcessCommonPatterns();
				}
				return m_title;
			}
			set { m_title = value; }
		}
		private string m_title;

		public int Episode 
		{ 
			get 
			{
				if (m_episode < 0) ProcessCommonPatterns();
				return m_episode; 
			}
			set { m_episode = value; }
		}
		public int Season 
		{
			get
			{
				if (m_season < 0) ProcessCommonPatterns();
				return m_season;
			}
			set { m_season = value; }
		}
		public int Disk 
		{
			get
			{
				if (m_disk < 0) ProcessCommonPatterns();
				return m_disk;
			}
			set { m_disk = value; }
		}
		private int m_episode = -1;
		private int m_season = -1;
		private int m_disk = -1;

		public string RelativePath { get; private set; }
		public MediaType DataType { get; set; }

		#region IComparable<T> Members

		public int CompareTo(StoredItem other)
		{
			return RelativePath.CompareTo(other.RelativePath);
		}

		#endregion

		#region Format Name
		

		/// <summary>
		/// Changes name from camel case (FieldName) to "DB column name" format (FIELD_NAME)
		/// </summary>
		public static string FormatName(string inp)
		{
			if (string.IsNullOrEmpty(inp)) return inp;

			inp = inp.Replace("_", " ");
			bool isNextUpperCase = true; //So it will not insert " " before the first char if it is capital
			bool isDigit = true;
			bool isUpper = true;

			char[] cs = inp.ToCharArray();
			char[] res = new char[cs.Length << 1];
			int resIdx = 0;

			for (int i = 0; i < cs.Length; i++)
			{
				char c = cs[i];
				bool isThisUpperCase = isNextUpperCase;

				if (char.IsWhiteSpace(c))
				{
					if (!isNextUpperCase || isDigit)
					{
						res[resIdx] = c;
						resIdx++;
					}
					isNextUpperCase = true;
					isDigit = false;
				}
				else if (char.IsDigit(c))
				{
					if (!isNextUpperCase)
					{
						res[resIdx] = ' ';
						resIdx++;
					}

					isNextUpperCase = true;
					res[resIdx] = c;
					resIdx++;
					isDigit = true;
				}
				else if (c == '-')
				{
					res[resIdx] = '-';
					resIdx++;
					isNextUpperCase = true;
				}
				else
				{
					if (isDigit) //After digit - insert space
					{
						res[resIdx] = ' ';
						resIdx++;
					}

					isDigit = false;
					bool isThisUpper = char.IsUpper(c);

					if (!isNextUpperCase && !isUpper && isThisUpper)
					{
						res[resIdx] = ' ';
						resIdx++;
						isThisUpperCase = true;
					}

					isNextUpperCase = false;

					isUpper = isThisUpper;

					if (isThisUpperCase)
					{
						c = char.ToUpperInvariant(c);
					}
					else
					{
						c = char.ToLowerInvariant(c);
					}

					res[resIdx] = c;
					resIdx++;
					isDigit = false;
				}

			}
			return new string(res, 0, resIdx).Trim();
		}

		private static Regex s_seasonsDisks = new Regex("\\s(S|SEASON)\\s*(\\d{1,2})\\s*D\\s*(\\d{1,2})$", RegexOptions.IgnoreCase);
		private void ProcessCommonPatterns()
		{
			string name = FormatName(m_name);

			int idx = name.IndexOf("1080");
			if (idx > 0) name = name.Substring(0, idx);

			if (name.EndsWith(" Bd")) name = name.Substring(0, name.Length - 3);
			if (name.EndsWith(" 3 D")) name = name.Substring(0, name.Length - 4) + " 3D";
			if (name.EndsWith(" Na")) name = name.Substring(0, name.Length - 3);
			if (name.EndsWith(" Us")) name = name.Substring(0, name.Length - 3);

			if (name.EndsWith(" Iii")) name = name.Substring(0, name.Length - 4) + " III";
			if (name.EndsWith(" Ii")) name = name.Substring(0, name.Length - 4) + " II";


			m_episode = 0;
			m_season = 0;
			m_disk = 0;
			var matches = s_seasonsDisks.Match(name);
			if (matches.Success && matches.Groups.Count == 4)
			{
				m_season = matches.Groups[2].Value.To<int>(0);
				m_disk = matches.Groups[3].Value.To<int>(0);
				m_title = string.Format("{0} (S{1}D{2})", name.Substring(0, matches.Groups[0].Index).Trim(), m_season, m_disk);
			}
			else
			{
				m_title = name.Trim();
			}

			//m_title = s_seasonsDisks.Replace(name, " S$2D$3").Trim();
		}
		#endregion
		
		public void Save()
		{
			TitleKind kind;

			switch(this.DataType)
			{
				case MediaType.AudioFolder:
				case MediaType.Cd:
				case MediaType.CdImage:
					kind = TitleKind.Album; 
					break;
				case MediaType.MediaFileAudio: 
					kind = TitleKind.Track;
					break;
				default:
					if (Episode > 0 || Disk > 0 || Season > 0) 
					{
						kind = TitleKind.Episode;
					}
					else 
					{
						kind = TitleKind.Title;
					}
					break;
			}

			string now = GeneralPersistense.GetTimestamp();
			var title = new Title { Id = 0, Kind = kind, Ord = 0, TitleName = Title, DateAddedUtc = now, DateModifiedUtc = now, Season = Season, Disk = Disk, EpisodeOrTrack = Episode, ImdbId = "", Description = ""};
			using (var db = DB.GetDatabase())
			{
				db.Insert(title);
				var location = new Location { Id = 0, LocationBaseId = m_locationBaseId, LocationData = RelativePath, MediaKind = DataType, TitleId = title.Id, DateAddedUtc = now, DateModifiedUtc = now };
				db.Insert(location);
			}
		}
	}

	public static class SearchFileStorage
	{
		private static readonly ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		
 
		public static List<StoredItem> Generate(string path, long locationBaseId)
		{
			var res = new List<StoredItem>();
			Populate(new DirectoryInfo(path), res, path, locationBaseId);
			return res;
		}

		static readonly string[] s_fileExtensionsVideo = { "avi", "vob", "mpg", "ts", "mkv", "mpv", "wmv", "3gp", "264", "dat", "evo", "dv", "flv", "h264", "m2ts", "m2v", "mjp", "ogv", "ogm", "qtm", "rm", "swf", "wm" };
		static readonly string[] s_fileExtensionsAudio = { "wav", "flac", "ape", "mp3", "ogg", "ac3", "pcm" };
		static readonly string[] s_fileExtensionsImg =  { "iso", "img", "mdf", "bin", "nrg" };
		static readonly string[] s_fileExtensionsPicture = { "jpg", "jpeg", "tiff", "bmp", "png", "gif" };

		static MediaType GetFileType(string ext, long size)
		{
			if (ext.Length == 0) return MediaType.Unknown;
			ext = ext.Substring(1).ToLowerInvariant(); //Removing dot
			if (s_fileExtensionsVideo.Contains(ext)) return MediaType.MediaFileVideo;
			if (s_fileExtensionsAudio.Contains(ext)) return MediaType.MediaFileAudio;
			if (s_fileExtensionsPicture.Contains(ext)) return MediaType.Picture;
			if (s_fileExtensionsImg.Contains(ext))
			{
				if (size < 1024 * 1024 * 1024) return MediaType.CdImage;
				if (size < 9 * 1024 * 1024 * 1024L) return MediaType.DvdImage;
				return MediaType.BluRayImage;
			}
			return MediaType.Unknown;
		}


		static void Populate(DirectoryInfo path, List<StoredItem> items, string basePath, long locationBaseId)
		{
			MediaType dtp = MediaType.Unknown;
			try
			{
				var dirs = path.GetDirectories();

				foreach (var dir in dirs)
				{
					var dirName = dir.Name.ToUpper();

					if (dirName == "VIDEO_TS")
					{
						dtp = MediaType.DvdFolder;
						break;
					}
					else if (dirName == "BDMV")
					{
						dtp = MediaType.BluRayFolder;
						break;
					}
					else if (dirName == "HVDVD_TS")
					{
						dtp = MediaType.HddvdFolder;
						break;
					}
				}
				if (dtp != MediaType.Unknown)
				{
					items.Add(new StoredItem(path, dtp, basePath, locationBaseId));
				}
				else
				{
					foreach (var dir in dirs)
					{
						Populate(dir, items, basePath, locationBaseId);
					}

					foreach (var file in path.GetFiles())
					{
						dtp = GetFileType(file.Extension, file.Length);
						if (dtp != MediaType.Unknown)
						{
							items.Add(new StoredItem(file, dtp, basePath, locationBaseId));
						}
					}
				}
			}
			catch (Exception err)
			{
				if (err is UnauthorizedAccessException)
				{
					string msg = err.Message.ToLowerInvariant();
					if (msg.Contains("recycle") || msg.Contains("system")) return;
				}
				s_log.Error("Error parsing folder", err);
			}
		}

	}
}