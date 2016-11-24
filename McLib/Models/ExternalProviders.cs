using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCollection
{
	public class Titleinfo
	{
		public Title TitleData;
		public List<TitlePropertyAccessor> Properties;
		public string ResourceId;
		public List<string> ImageUrls;
		public List<TitleRating> Ratings;
	}

	public abstract class InfoProvider
	{
		public abstract int Id {get;}
		public abstract string Name {get;}
		public abstract List<Titleinfo> GetInfo(string name);
	}


	//OMDb
	//IMDB
}
