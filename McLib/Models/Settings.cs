using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stolbovoy.Utils;

namespace MediaCollection
{
	public class Settings
	{
		public static async Task<T> Get<T>(string key)
		{
			using (var db = DB.GetDatabase())
			{
				var res = await db.FetchAsync<string>("SELECT CONFIG_VALUE FROM app_config WHERE CONFIG_KEY = @0", key);
				if (res.Count == 0) throw new KeyNotFoundException(string.Format("{0} was not configured", key));
				return res[0].To<T>();
			}
		}

		public async Task Set<T>(string key, T value)
		{
			if (string.IsNullOrEmpty(key)) throw new ArgumentException("Key cannot be empty or null", "key");
			var data = new AppConfig { ConfigKey = key, ConfigValue = value?.ToString() ?? null };
			await GeneralPersistense.Upsert(data);
		}
	}
}
