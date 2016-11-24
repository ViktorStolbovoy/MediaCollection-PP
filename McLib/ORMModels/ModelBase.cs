using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCollection
{
	public interface IModelWithId
	{
		long Id { get; set; }
	}
}
