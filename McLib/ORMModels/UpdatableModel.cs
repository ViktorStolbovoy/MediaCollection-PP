using System;
using System.Threading.Tasks;

namespace MediaCollection
{
	public abstract class UpdatableModel
	{
		public virtual async Task Set()
		{
			using (var db = DB.GetDatabase())
			{
				if (await db.UpdateAsync(this) == 0)
				{
					await db.InsertAsync(this);
				}
			}
		}

		public abstract Task Delete();
		
	}


	public abstract class UpdatableModelWithId : UpdatableModel
	{
		public virtual long Id { get; set; }

		/// <summary>
		/// Upsert
		/// </summary>
		public override async Task Set()
		{
			using (var db = DB.GetDatabase())
			{
				if (Id <= 0)
				{
					await db.InsertAsync(this);
				}
				else
				{
					await db.UpdateAsync(this);
				}
			}
		}


		public virtual async Task Update()
		{
			if (this.Id <= 0) throw new ArgumentException("Id should be positive");

			using (var db = DB.GetDatabase())
			{
				await db.UpdateAsync(this);
			}
		}

		public virtual async Task Insert()
		{
			using (var db = DB.GetDatabase())
			{
				await db.InsertAsync(this);
			}
		}
	}
}
