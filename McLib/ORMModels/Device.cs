using System.Threading.Tasks;
using NPoco;


namespace MediaCollection
{
	public enum DeviceType {Dune = 0, Local = 1};
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

		[Column("IS_DEFAULT")]
		public virtual long IsDefault { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public override Task Delete()
		{
			return Delete(Id);
		}

		public static async Task Delete(long id)
		{
			using (var db = DB.GetDatabase())
			{
				await db.ExecuteAsync("DELETE FROM DEVICE_LOCATION_MAP WHERE DEVICE_ID = @0", id);
				await db.ExecuteAsync("DELETE FROM DEVICE WHERE DEVICE_ID = @0", id);
			}
		}
	}
}
