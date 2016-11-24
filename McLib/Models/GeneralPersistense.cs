using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCollection
{
	public static class GeneralPersistense
	{
		public static void Upsert<T>(T data)
		{
			using (var db = DB.GetDatabase())
			{
				if (db.Update(data) == 0)
				{
					db.Insert(data);
				}
			}
		}

		public const string DT_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";
		public static string GetTimestamp()
		{
			return DateTime.UtcNow.ToString(DT_FORMAT);
		}

		public static List<T> FetchAll<T>()
		{
			using (var db = DB.GetDatabase())
			{
				return db.Fetch<T>();
			}
		}
	}
}
