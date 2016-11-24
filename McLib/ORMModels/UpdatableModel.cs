using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCollection
{
	public abstract class UpdatableModel
	{
		public virtual void Set()
		{
			using (var db = DB.GetDatabase())
			{
				if (db.Update(this) == 0)
				{
					db.Insert(this);
				}
			}
		}

		public abstract void Delete();
		
	}


	public abstract class UpdatableModelWithId : UpdatableModel
	{
		public virtual long Id { get; set; }

		/// <summary>
		/// Upsert
		/// </summary>
		public override void Set()
		{
			using (var db = DB.GetDatabase())
			{
				if (Id <= 0)
				{
					db.Insert(this);
				}
				else
				{
					db.Update(this);
				}
			}
		}


		public virtual void Update()
		{
			if (this.Id <= 0) throw new ArgumentException("Id should be positive");

			using (var db = DB.GetDatabase())
			{
				db.Update(this);
			}
		}

		public virtual void Insert()
		{
			using (var db = DB.GetDatabase())
			{
				db.Insert(this);
			}
		}
	}
}
