using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection
{
	public enum DeviceType {Dune = 0, PC = 1};
	[TableName("device")]
	[PrimaryKey("DEVICE_ID", AutoIncrement = true)]
	public class Device : UpdatableModelWithId
	{
		[Column("DEVICE_ID")]
		public override long Id { get; set; }
		[Column("DEVICE_KIND")]
		public virtual DeviceType Kind { get; set; }
		[Column("DEVICE_NAME")]
		public virtual string Name { get; set; }
		[Column("DEVICE_DATA")]
		public virtual string Data { get; set; }

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
				db.Execute("DELETE FROM DEVICE_LOCATION_MAP WHERE DEVICE_ID = @0", id);
				db.Execute("DELETE FROM DEVICE WHERE DEVICE_ID = @0", id);
			}
		}
	}
}
