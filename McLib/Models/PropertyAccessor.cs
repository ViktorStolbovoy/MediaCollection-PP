using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stolbovoy.Utils;

namespace MediaCollection
{

	public enum TitlePropertyKind { StringProperty, IntegerProperty, DecimalProperty }
	public class TitlePropertyAccessor
	{
		public TitlePropertyAccessor(string name, TitlePropertyKind kind)
		{
			m_name = name;

			using (var db = DB.GetDatabase())
			{
				var prop = db.Query<Property>().Where(p => p.PropertyName == name).FirstOrDefault();
				if (prop == null)
				{
					throw new ApplicationException("Property was not found: " + name);
				}
				m_id = prop.Id;
				m_kind = KindFromString(prop.PropertyKind);
			}
		}

		private string m_name;
		private long m_id;
		private TitlePropertyKind m_kind;

		public string Name { get { return m_name; } }
		public long Id { get { return m_id; } }
		public TitlePropertyKind Kind { get { return m_kind; } }

		private string m_cachedValue;
		private long m_cachedTitleId;

		private TitlePropertyKind KindFromString(string s)
		{
			switch(s)
			{
				case "S": return TitlePropertyKind.StringProperty;
				case "I": return TitlePropertyKind.IntegerProperty;
				case "D": return TitlePropertyKind.DecimalProperty;
				default: throw new ApplicationException("Invalid property kind: " + s ?? "<null>");
			}
		}

		private string StringFromKind(TitlePropertyKind k)
		{
			switch(k)
			{
				case TitlePropertyKind.StringProperty: return "S";
				case TitlePropertyKind.IntegerProperty: return "I";
				case TitlePropertyKind.DecimalProperty: return "D";
				default: throw new ApplicationException("Invalid property kind: " + k.ToString());
			}
		}

		public void Reset()
		{
			m_cachedTitleId = 0;
			m_cachedValue = "";
			if (m_setter != null) m_setter("");
		}

		public string Get(long titleId)
		{
			using (var db = DB.GetDatabase())
			{
				var res = db.Query<TitleProperty>("WHERE PROPERTY_ID = @0 AND TITLE_ID = @1", m_id, titleId);
				if (res == null) return "";
				m_cachedTitleId = titleId;
				var row = res.FirstOrDefault();
				if (row == null)
				{
					m_cachedValue = "";
				}
				else
				{
					m_cachedValue = row.PropertyValue ?? "";
				}
			}
			if (m_setter != null) m_setter(m_cachedValue);
			return m_cachedValue;
		}

		public void Set(long titleId, string value)
		{
			if (IsDirty(titleId, value))
			{
				var prop = new TitleProperty { Property_Id = m_id, Title_Id = titleId, PropertyValue = value };

				using (var db = DB.GetDatabase())
				{
					db.Save <TitleProperty>(prop);
				}
				m_cachedTitleId = titleId;
				m_cachedValue = value;
			}
		}

		public void Set(long titleId, int value)
		{
			if (m_kind != TitlePropertyKind.IntegerProperty) throw new ApplicationException(m_name + " is not an integer property ");
			Set(titleId, value.ToString());
		}

		public void Set(long titleId, decimal value)
		{
			if (m_kind != TitlePropertyKind.IntegerProperty) throw new ApplicationException(m_name + " is not a decimal property ");
			Set(titleId, value.ToString());
		}

		public void Set(long titleId)
		{
			if (m_getter == null) throw new ApplicationException("Getter was not assigned");
			string value = m_getter();
			var prop = new TitleProperty { Property_Id = m_id, Title_Id = titleId, PropertyValue = value };

			using (var db = DB.GetDatabase())
			{
				db.Save<TitleProperty>(prop);
			}
			m_cachedTitleId = titleId;
			m_cachedValue = value;
		}

		public bool IsDirty(long titleId, string value)
		{
			return m_cachedTitleId != titleId || (m_cachedValue ?? "") != (value ?? "");
		}

		public bool IsDirty(long titleId)
		{
			if (m_getter == null) throw new ApplicationException("Getter was not assigned");
			return IsDirty(titleId, m_getter());
		}


		public int GetInt(long titleId)
		{
			if (m_kind == TitlePropertyKind.IntegerProperty) return Get(titleId).To<int>(0);
			throw new ApplicationException(string.Format("Property {0} is not integer. The property type is {1}", m_name, m_kind));
		}

		public decimal GetDecimal(long titleId)
		{
			if (m_kind == TitlePropertyKind.IntegerProperty) return Get(titleId).To<decimal>(0);
			throw new ApplicationException(string.Format("Property {0} is not decimal. The property type is {1}", m_name, m_kind));
		}

		private Func<string> m_getter;
		private Action<string> m_setter;
		public void SetAccessors(Func<string> getter, Action<string> setter)
		{
			m_getter = getter;
			m_setter = setter;
		}
	}
}
