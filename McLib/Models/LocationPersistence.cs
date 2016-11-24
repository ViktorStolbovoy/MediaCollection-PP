using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;


namespace MediaCollection
{
	public static class LocationPersistence
	{

		public static List<LocationForDisplay> ListTitleLocations(long titleId)
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<LocationForDisplay>("SELECT l.*, b.LOCATION_KIND, b.LOCATION FROM location l JOIN location_base b ON  b.LOCATION_BASE_ID = l.LOCATION_BASE_ID WHERE l.title_id = @0", titleId);
			}
		}

		public static List<TitleLocation> GetLocationsForBase(long baseId)
		{
			using (var db = DB.GetDatabase())
			{
				return GetLocationForBase(baseId, db);
			}
		}

		public static List<TitleLocation> GetLocationForBase(long baseId, IDatabase db)
		{
			return db.Fetch<TitleLocation>("SELECT * FROM location WHERE LOCATION_BASE_ID = @0", baseId);
		}

		public static IList<LocationBase> ListBases()
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<LocationBase>("SELECT * FROM location_base ORDER BY LOCATION");
			}
		}

		public static LocationBase GetBase(long id)
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<LocationBase>("SELECT * FROM location_base WHERE LOCATION_BASE_ID = @0", id).FirstOrDefault();
			}
		}


		public static DeviceLocationResult GetTitleLocationFull(long deviceId, long titleId)
		{
			//TitleId points to Location(of a title)
			//Location has Location Data (0) and LocationBase data
			//How to format string depends on Device (DEVICE_DATA is the format string). The rules how to access each location base for each device is stored in DeviceLocationMap
			//Device id points to DEvice.DeviceData which is format string where {0} is DeviceLocationMap.LocationMapping, {1} is Location.LocationData

			using (var db = DB.GetDatabase())
			{
				const string SQL = @"SELECT d.DEVICE_DATA, dl.LOCATION_MAPPING, l.LOCATION_DATA, l.MEDIA_KIND , d.DEVICE_KIND
					FROM location l 
					JOIN location_base b 
					  ON b.LOCATION_BASE_ID = l.LOCATION_BASE_ID
					JOIN device_location_map dl
					  ON dl.LOCATION_BASE_ID =  l.LOCATION_BASE_ID
					  AND dl.DEVICE_ID = @1
					JOIN device d
					  ON d.DEVICE_ID = dl.DEVICE_ID
					WHERE l.TITLE_ID =@0";
				var lr = db.Fetch<DeviceLocationResult>(SQL, titleId, deviceId);
				if (lr.Count == 0) return null;
				return lr[0];
			}
		}

		public static void Run(long deviceId, long titleId)
		{
			var dl = GetTitleLocationFull(deviceId, titleId);
			if (dl == null) return;

			dl.Run();
		}
	}



	public class TitleLocation : Location
	{
		public virtual string Name { get { return m_title == null ? "" : m_title.TitleName; } }
		public virtual TitleKind Kind { get { return m_title == null ? TitleKind.Title : m_title.Kind; } }

		public virtual int NewLocationBaseId { get; set; }

		public virtual string NewLocationData { get; set; }

		private Title m_title;

		//Get title info
		public void RetrieveTitleInfo(IDatabase db)
		{
			if (TitleId > 0) m_title = db.SingleById<Title>(this.TitleId);
		}

		public bool ShouldDelete { get; set; }

		public void Delete()
		{
			using (var db = DB.GetDatabase())
			{
				db.Delete<Location>(Id);
				if (m_title != null)
				{
					if (db.Query<Location>().Where(l => l.TitleId == m_title.Id).Count() == 0)
					{
						db.Delete(m_title);
						m_title = null;
					}
				}
			}
		}

		public void SetNewLocation()
		{
			LocationBaseId = NewLocationBaseId;
			LocationData = NewLocationData;
			DateModifiedUtc = GeneralPersistense.GetTimestamp();

			using (var db = DB.GetDatabase())
			{
				db.Update(this);
			}
		}
	}

	public class RescanResults
	{
		//public string ErrorCode;
		public readonly List<StoredItem> NewFiles = new List<StoredItem>();
		public readonly List<TitleLocation> MissingFiles = new List<TitleLocation>();

		private void RetrieveMissingTitleInfo()
		{
			using (var db = DB.GetDatabase())
			{
				foreach (var mf in MissingFiles) mf.RetrieveTitleInfo(db);
			}
		}

		public static RescanResults Run(long locationBaseId, long deviceId)
		{
			var res = new RescanResults();

			//DEVICE_KIND = 1 - PC
			const string SQL = @"SELECT dl.LOCATION_MAPPING
					FROM device_location_map dl
					JOIN device d
					  ON d.DEVICE_ID = dl.DEVICE_ID
					 AND d.DEVICE_KIND = 1
					WHERE dl.LOCATION_BASE_ID = @0
					  AND dl.DEVICE_ID = @1";

			string basePath;
			List<TitleLocation> databaseStorage;
			using (var db = DB.GetDatabase())
			{
				basePath = db.Fetch<string>(SQL, locationBaseId, deviceId).FirstOrDefault();
				databaseStorage = LocationPersistence.GetLocationForBase(locationBaseId, db);
			}

			//Don't process images and unknown files
			var fileStorage = SearchFileStorage.Generate(basePath, locationBaseId).Where((f) => { return f.DataType != MediaType.Picture && f.DataType != MediaType.Unknown && f.DataType != MediaType.PictureFolder; }).ToList();
			/*if (fileStorage.Count == 0) 
			{
				res.ErrorCode = "NO_MEDIA_FILES_FOUND";
				return res;
			} */

			fileStorage.Sort();
			databaseStorage.Sort(); //Sorting here to have exactly the same order

			int basePathLen = basePath.Length;

			int idxFile = 0;
			int idxDb = 0;

			while (idxFile < fileStorage.Count && idxDb < databaseStorage.Count)
			{
				//In real world likely there will be more files on disk than in DB, so there is no optimization for removed files 
				string filePath = fileStorage[idxFile].RelativePath;
				//if (!filePath.StartsWith(basePath)) throw new ApplicationException(string.Format("Path {0} doesn't start with {1}", filePath, basePath));
				string relPath = filePath; //.Substring(basePath.Length);

				int cmp = relPath.CompareTo(databaseStorage[idxDb].LocationData);
				if (cmp == 0)
				{
					//cool - match found
					idxDb++;
					idxFile++;
				}
				else if (cmp < 0)
				{
					//fileStorage precedes -> not in the DB
					res.NewFiles.Add(fileStorage[idxFile]);
					idxFile++;
				}
				else
				{
					//InFiles precedes -> not in the DB
					res.MissingFiles.Add(databaseStorage[idxDb]);
					idxDb++;
				}
			}

			//Leftovers after one of the lists is fully processed 
			for (; idxFile < fileStorage.Count; idxFile++) res.NewFiles.Add(fileStorage[idxFile]);
			for (; idxDb < databaseStorage.Count; idxDb++) res.MissingFiles.Add(databaseStorage[idxDb]);

			res.RetrieveMissingTitleInfo();
			return res;
		}
	}
}
