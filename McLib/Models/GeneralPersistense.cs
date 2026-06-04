using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaCollection
{
	public static class GeneralPersistense
	{
		public static async Task Upsert<T>(T data)
		{
			using (var db = DB.GetDatabase())
			{
				if (await db.UpdateAsync(data) == 0)
				{
					await db.InsertAsync(data);
				}
			}
		}

		public const string DT_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";
		public static string GetTimestamp()
		{
			return DateTime.UtcNow.ToString(DT_FORMAT);
		}

		public static async Task<List<T>> FetchAll<T>()
		{
			using (var db = DB.GetDatabase())
			{
				return await db.FetchAsync<T>();
			}
		}
	}
}
