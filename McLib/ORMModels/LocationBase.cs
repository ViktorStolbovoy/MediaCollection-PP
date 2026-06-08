using System;
using System.Threading.Tasks;
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

		public override Task Delete()
		{
			return Delete(Id);
		}

		public static async Task Delete(long id)
		{
			using (var db = DB.GetDatabase())
			{
				int cnt = await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM LOCATION WHERE LOCATION_BASE_ID = @0", id);
				if (cnt > 0) throw new ApplicationException(string.Format("Can't delete Location: it is used by {0} title locations", cnt));
				await db.ExecuteAsync("DELETE FROM DEVICE_LOCATION_MAP WHERE LOCATION_BASE_ID = @0", id);
				await db.ExecuteAsync("DELETE FROM LOCATION_BASE WHERE LOCATION_BASE_ID = @0", id);
			}
		}
	}
}
