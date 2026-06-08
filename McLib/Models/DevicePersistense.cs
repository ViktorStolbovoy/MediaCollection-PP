using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaCollection
{
	public static class DevicePersistense
	{
		public static async Task<List<Device>> List()
		{
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<Device>();
			}
		}

		public static async Task<List<Device>> ListForTitleUpdate()
		{
			// We update titles using local mapping.
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<Device>("SELECT * FROM DEVICE WHERE DEVICE_KIND = @0 ORDER BY DEVICE_NAME", DeviceType.Local);
			}
		}

		public static async Task<List<Device>> ListForPalyback()
		{
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<Device>("SELECT * FROM DEVICE ORDER BY IS_DEFAULT desc, DEVICE_NAME");
			}
		}


		public static async Task<List<LocationBaseDeviceMapping>> GetLocations(long deviceId)
		{
			const string SQL = @"SELECT b.LOCATION_BASE_ID, b.LOCATION_KIND, b.LOCATION, m.LOCATION_MAPPING, @0 DEVICE_ID
			FROM location_base b
			LEFT JOIN device_location_map m
				on m.LOCATION_BASE_ID = b.LOCATION_BASE_ID
				and m.DEVICE_ID = @0
			WHERE b.LOCATION_KIND <> @1";
			
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<LocationBaseDeviceMapping>(SQL, deviceId, LocationBaseKind.Shelf);
			}
		}

		public static async Task<List<LocationBase>> GetLocationsForTitleUpdate(long deviceId)
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
				return await db.FetchAsync<LocationBase>(SQL, deviceId, new [] {LocationBaseKind.Shelf, LocationBaseKind.HTTP});
			}
		}
	}
}
