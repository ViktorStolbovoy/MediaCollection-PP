using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Stolbovoy.Utils
{
	public static class ObjectHelpers
	{
		public static string ToJson(this object value)
		{
			if (value == null) return null;
			var serializer = new Newtonsoft.Json.JsonSerializer();
			serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

			var nvc = value as System.Collections.Specialized.NameValueCollection;
			if (nvc != null)
			{
				var dict = new Dictionary<string, string>();
				foreach (string key in nvc.Keys)
				{
					dict.Add(key, nvc[key]);
				}
				value = dict;
			}

			using (var writer = new StringWriter())
			{
				serializer.Serialize(writer, value);
				return writer.ToString();
			}
		}

		public static T FromJson<T>(this string data)  where T : class
		{											  
			if (data == null) return default(T);
			return JsonConvert.DeserializeObject<T>(data);
		}

		public static string JSReplace(this string str)
		{
			if (str == null) return "";

			str = str.Replace(@"\", @"\\");
			str = str.Replace(@"""", @"\""");
			str = str.Replace(@"'", @"\'");
			str = str.Replace("\n", "\\n");
			str = str.Replace("\r", "\\r");
			str = str.Replace("\t", "\\t");

			str = str.Replace("<", "\\u003C"); //IE bug
			str = str.Replace(">", "\\u003E");
			
			str = str.Replace("\u2028", "\\u2028");
			str = str.Replace("\u2029", "\\u2029");
			return str;
		}

	}
}
