using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCollection
{
	public class SortableTitles : List<Title>, BrightIdeasSoftware.IVirtualListDataSource
	{
		public SortableTitles(List<Title> titles) 
		{
			base.AddRange(titles);
		}

		public void AddObjects(System.Collections.ICollection modelObjects)
		{
			base.AddRange(modelObjects.Cast<Title>());
		}

		public object GetNthObject(int n)
		{
			return this[n];
		}

		public int GetObjectCount()
		{
			return this.Count;
		}

		public int GetObjectIndex(object model)
		{
			return this.IndexOf(model as Title);
		}

		public void InsertObjects(int index, System.Collections.ICollection modelObjects)
		{
			base.InsertRange(index, modelObjects.Cast<Title>());
		}

		public void PrepareCache(int first, int last)
		{
			throw new NotImplementedException();
		}

		public void RemoveObjects(System.Collections.ICollection modelObjects)
		{
			foreach(object o in modelObjects) 
			{
				var t = o as Title;
				if (t != null) 
				{
					base.Remove(t);
				}
			}
			
		}

		public int SearchText(string value, int first, int last, BrightIdeasSoftware.OLVColumn column)
		{
			string text = (value ?? "").ToLower();
			for(int i = first; i <= last; i ++)
			{
				if ((base[i].TitleName ?? "").ToLower() == text) return i;
			}
			return -1;
		}

		public void SetObjects(System.Collections.IEnumerable collection)
		{
			throw new NotImplementedException();
		}

		public void Sort(BrightIdeasSoftware.OLVColumn column, System.Windows.Forms.SortOrder order)
		{
			base.Sort();
		}

		public void UpdateObject(int index, object modelObject)
		{
			var t = (Title)modelObject;
			base[index] = t;
		}
	}
}
