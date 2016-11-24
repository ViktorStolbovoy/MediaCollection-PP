using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Stolbovoy.Utils;


namespace MediaCollection
{
	public class TableRenderData
	{
		public string TableControlName; 
		public string JSName;
		public string[] Headers;
		public bool CanAdd = true;
		public bool CanDelete = true;
	}

	public class SetRequest
	{
		public long[] Deleted { get; set; }
		public List<string[]> Edits { get; set; }
		public long? ParentId { get; set; }

		static ConcurrentDictionary<Type, PropertyInfo[]> s_accessors = new ConcurrentDictionary<Type, PropertyInfo[]>();

		public IEnumerable<T> ToPoco<T>() where T : new()
		{
			var accessors = GetAccessors<T>();

			if (Edits == null) return new T[0];

			return Edits.Select(r => 
			{
				if (r == null) throw new ApplicationException("Received row can't be null");
				if (r.Length != accessors.Length) throw new ApplicationException(string.Format("Received row should have {0} columns. {1} received", accessors.Length, r.Length));
				var res = new T();
				for (int i = 0; i < r.Length; i ++ )
				{
					var a = accessors[i];
					object o = r[i].ToType(null, true, a.PropertyType);
					a.SetValue(res, o);
				}
				return res; 
			});
		}

		public static IEnumerable<string[]> FromPoco<T>(IEnumerable<T> data)
		{
			var accessors = GetAccessors<T>();
			return data.Select(r => {
				var res = new string[accessors.Length];
				for (int i = 0; i < res.Length; i++)
				{
					object val = accessors[i].GetValue(r);
					res[i] = (val == null) ? "" : val.ToString();
				}
				return res;
			});
		}

		static PropertyInfo[] GetAccessors<T>()
		{
			return s_accessors.GetOrAdd(typeof(T), (t) => {
				var props = t.GetProperties();
				int idIdx = -1;
				for (int i = 0; i < props.Length; i++)
				{
					if (props[i].Name == "Id")
					{
						idIdx = i;
						break;
					}
				}
				var res = new PropertyInfo[props.Length];
				//Ensuring that Id goes first
				int startIdx = 0;
				if (idIdx >= 0)
				{
					res[0] = props[idIdx];
					startIdx = 1;
				}

				for (int i = 0; i < props.Length; i++)
				{
					if (i != idIdx)
					{
						res[startIdx] = props[i];
						startIdx++;
					}
				}
				return res;
			});
		}

		public void Persist<T>(Action<long> deleteMethod) where T : UpdatableModelWithId, new()
		{
			if (Edits != null)
			{
				foreach (var e in ToPoco<T>())
				{
					e.Set();
				}
			}
			if (Deleted != null)
			{
				foreach (var id in Deleted)
				{
					deleteMethod(id);
				}
			}
		}
	}

}