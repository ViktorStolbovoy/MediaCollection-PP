using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;

namespace MediaCollection
{
	public static class DevicePersistense
	{
		public static List<Device> List()
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<Device>();
			}
		}

		public static List<Device> ListForTitleUpdate()
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<Device>("SELECT * FROM DEVICE WHERE DEVICE_KIND = @0 ORDER BY DEVICE_NAME", DeviceType.PC);
			}
		}

		public static List<Device> ListForPalyback()
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<Device>("SELECT * FROM DEVICE ORDER BY DEVICE_NAME");
			}
		}


		public static List<LocationBaseDeviceMapping> GetLocations(long deviceId)
		{
			const string SQL = @"SELECT b.LOCATION_BASE_ID, b.LOCATION_KIND, b.LOCATION, m.LOCATION_MAPPING, @0 DEVICE_ID
			FROM location_base b
			LEFT JOIN device_location_map m
				on m.LOCATION_BASE_ID = b.LOCATION_BASE_ID
				and m.DEVICE_ID = @0
			WHERE b.LOCATION_KIND <> @1";
			
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<LocationBaseDeviceMapping>(SQL, deviceId, LocationBaseKind.Shelf);
			}
		}

		public static List<LocationBase> GetLocationsForTitleUpdate(long deviceId)
		{
			const string SQL = @"SELECT b.LOCATION_BASE_ID, b.LOCATION_KIND, b.LOCATION
			FROM location_base b
			JOIN device_location_map m
				on m.LOCATION_BASE_ID = b.LOCATION_BASE_ID
				and m.DEVICE_ID = @0
				and m.LOCATION_MAPPING > ''
			WHERE b.LOCATION_KIND not in (@1)";

			using (var db = DB.GetDatabase())
			{
				return db.Fetch<LocationBase>(SQL, deviceId, new [] {LocationBaseKind.Shelf, LocationBaseKind.HTTP});
			}
		}
	}
}
