using System;
using System.Text;
using System.Collections.Generic;
using NPoco;

namespace MediaCollection 
{

	public enum LocationBaseKind { Local = 0, RemovableHDD = 1, Shelf = 2, HTTP = 3 }

	[TableName("LOCATION_BASE")]
	[PrimaryKey("LOCATION_BASE_ID", AutoIncrement=true)]
	public class LocationBase : UpdatableModelWithId
	{
		[Column("LOCATION_BASE_ID")]
		public override long Id { get; set; }
		[Column("LOCATION_KIND")]
		public virtual LocationBaseKind Kind { get; set; }
		[Column("LOCATION")]
		public virtual string Name { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public override void Delete()
		{
			Delete(Id);
		}

		public static void Delete(long id)
		{
			using (var db = DB.GetDatabase())
			{
				int cnt = db.Query<DeviceLocationMap>().Where(x => x.LocationBaseId == id).Count();
				if (cnt > 0) throw new ApplicationException(string.Format("Can't delete Location: it is used by {0} devices", cnt));
				db.Execute("DELETE FROM LOCATION_BASE WHERE LOCATION_BASE_ID = @0", id);
			}
		}
	}
}
