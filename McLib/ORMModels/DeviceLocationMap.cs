using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection 
{
	[TableName("DEVICE_LOCATION_MAP")]
	[PrimaryKey("DEVICE_ID,LOCATION_BASE_ID", AutoIncrement = false)]
	public class DeviceLocationMap : UpdatableModel
	{
		[Column("DEVICE_ID")]
		public virtual long DeviceId { get; set; }
		[Column("LOCATION_BASE_ID")]
		public virtual long LocationBaseId { get; set; }
		[Column("LOCATION_MAPPING")]
		public virtual string Mapping { get; set; }

		public override void Delete()
		{
			Delete(DeviceId, LocationBaseId);
		}

		public static void Delete(long deviceId, long locationBaseId)
		{
			using (var db = DB.GetDatabase())
			{
				db.Execute("DELETE FROM DEVICE_LOCATION_MAP WHERE DEVICE_ID = @0 and LOCATION_BASE_ID =@1 ", deviceId, locationBaseId);
			}
		} 
	}


	/// <summary>
	/// DeviceLocationMap - LocationBase mix-in
	/// </summary>
	public class LocationBaseDeviceMapping : DeviceLocationMap
	{
		[Column("LOCATION_KIND")]
		public virtual LocationBaseKind Kind { get; set; }
		[Column("LOCATION")]
		public virtual string Name { get; set; }

		public override void Set()
		{
			//Removing LocationBase mix-in fields
			if (!string.IsNullOrWhiteSpace(this.Mapping))
			{
				var model = new DeviceLocationMap { DeviceId = this.DeviceId, LocationBaseId = this.LocationBaseId, Mapping = this.Mapping };
				model.Set();
			}
			else
			{
				DeviceLocationMap.Delete(DeviceId, LocationBaseId);
			}
		}

		


	}
}
