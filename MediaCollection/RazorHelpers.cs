using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Stolbovoy.Utils;


namespace MediaCollection
{
	public static class RazorHelpers
	{
		public static IEnumerable<SelectListItem> ToSelectList<T>(T selected, System.Resources.ResourceManager resourceManager) 
		{
			Type enumType = typeof(T);
			if (!enumType.IsEnum)
			{
				throw new Exception(String.Format("The enumType passed is not a enum it is {0}", enumType.Name));
			}

			string tpName = enumType.Name;

			foreach (object e in Enum.GetValues(enumType))
			{
				string text = null;
				string enumName = e.ToString();
				if (resourceManager != null)
				{
					text = resourceManager.GetString(tpName + "_" + enumName);
				}
				if (string.IsNullOrEmpty(text)) text = enumName;

				yield return new SelectListItem { Value = enumName, Text = text, Selected = selected.Equals(e) };
			}
		}

		public static string EnumToText<T>(this T value, System.Resources.ResourceManager resourceManager)
		{
			Type enumType = typeof(T);
			string enumName = value.ToString();
			string text = null;
			if (resourceManager != null)
			{
				text = resourceManager.GetString(enumType.Name + "_" + enumName);
			}
			if (string.IsNullOrEmpty(text)) return enumName;
			return text;
		}

		public static string EnumToJavascript<T>(System.Resources.ResourceManager resourceManager)
		{
			Type enumType = typeof(T);
			if (!enumType.IsEnum)
			{
				throw new Exception(String.Format("The enumType passed is not a enum it is {0}", enumType.Name));
			}

			string tpName = enumType.Name;

			var res = new Dictionary<string, string>();
			
			foreach (object e in Enum.GetValues(enumType))
			{
				string text = null;
				string enumName = e.ToString();
				if (resourceManager != null)
				{
					text = resourceManager.GetString(tpName + "_" + enumName);
				}
				if (string.IsNullOrEmpty(text)) text = enumName;
				res.Add(enumName, text);
			}

			return res.ToJson();
		}

	}
}