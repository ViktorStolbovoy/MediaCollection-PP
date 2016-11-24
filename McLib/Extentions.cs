using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;


namespace Stolbovoy.Utils
{
	public static class Extentions
	{
		public static T To<T>(this object value, T defaultValue)
		{
			return To<T>(value, defaultValue, false);
		}

		public static T To<T>(this object value)
		{
			return To<T>(value, default(T), true);
		}

		public static T To<T>(this object value, T defaultValue, bool shouldThrowError)
		{
			Type castType = typeof (T);

			return (T) ToType(value, defaultValue, shouldThrowError, castType);
		}

 		public static object ToType(this object value, object defaultValue, bool shouldThrowError, Type castType)
		{
			bool isNullable = castType.IsGenericType && castType.GetGenericTypeDefinition() == typeof(Nullable<>);
			Type sourceType = value == null ? null : value.GetType();
			if (value == null || value == DBNull.Value || (sourceType == typeof(string) && castType != typeof(string) && string.IsNullOrWhiteSpace((string)value)))
			{
				if (shouldThrowError && castType.IsValueType && !isNullable)
				{
					throw new InvalidCastException("Cannot cast null value to type " + castType.Name);
				}
				else
				{
					return defaultValue;
				}
			}

			if (castType.IsAssignableFrom(sourceType))
			{
				return value;
			}

			Type castBaseType = isNullable ? castType.GetGenericArguments()[0] : castType;
			if (castBaseType == typeof(bool))
			{
				switch (value.ToString().ToLowerInvariant())
				{
					case "1":
					case "t":
					case "on":
					case "true": return true;
					case "0":
					case "f":
					case "off":
					case "false": return false;
					default:
						if (shouldThrowError)
						{
							throw new InvalidCastException(string.Format("Cannot cast value \"{0}\" of the type {1} to bool", value, sourceType.Name));
						}
						else
						{
							return defaultValue;
						}
				}
			}

			if (castBaseType == typeof(DateTime))
			{
				try
				{
					return XmlConvert.ToDateTime(value.ToString(), XmlDateTimeSerializationMode.RoundtripKind);
				}
				catch (Exception err)
				{
					if (shouldThrowError)
					{
						throw new InvalidCastException(
							string.Format("Cannot cast value \"{0}\" of the type {1} to the type {2}", value, sourceType.Name, castType.Name),
							err);
					}
					else
					{
						return defaultValue;
					}
				}
			}

			if (castBaseType.IsEnum)
			{
				if (sourceType == typeof(string) || isNullable)
				{
					try
					{
						return System.Enum.Parse(castBaseType, value.ToString());
					}
					catch (Exception err)
					{
						if (shouldThrowError)
						{
							throw new InvalidCastException(string.Format("Cannot cast value \"{0}\" of the type {1} to the enum type {2}", value, sourceType.Name, castType.Name), err);
						}
						else
						{
							return defaultValue;
						}
					}
				}
				else
				{
					castBaseType = castBaseType.GetEnumUnderlyingType();
				}
			}

			if (value is IConvertible && Type.GetTypeCode(castBaseType) != TypeCode.Object)
			{
				try
				{
					return Convert.ChangeType(value, castBaseType);
				}
				catch (Exception err)
				{
					if (shouldThrowError)
					{
						throw new InvalidCastException(string.Format("Cannot cast value \"{0}\" of the type {1} to the type {2}", value, sourceType.Name, castType.Name), err);
					}
					else
					{
						return defaultValue;
					}
				}
			}

			if (shouldThrowError)
			{
				throw new InvalidCastException(string.Concat("Cannot cast value of type ", sourceType.Name, " to type ", castType.Name));
			}
			else
			{
				return defaultValue;
			}
		}

		public static string Wrap(this string text, int lineLen = 50)
		{
			var res = new char[text.Length << 1];
			int idx = 0;
			int len = 0;
			foreach(char c in text)
			{
				if (char.IsWhiteSpace(c) && len > lineLen)
				{
					res[idx] = '\r';
					idx++;
					res[idx] = '\n';
					idx++;
					len = 0;
				}
				else
				{
					res[idx] = c;
					idx++;
					len++;
				}
			}
			return new string(res, 0, idx);
		}
	}
}