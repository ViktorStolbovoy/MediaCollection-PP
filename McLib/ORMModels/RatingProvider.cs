using System;
using System.Text;
using System.Collections.Generic;
using NPoco;

namespace MediaCollection {
    
	[TableName("RATING_PROVIDER")]
	[PrimaryKey("RATING_ID", AutoIncrement=true)]
	public class RatingProvider : UpdatableModelWithId
	{
		[Column("RATING_ID")]
        public override long Id { get; set; }
		[Column("RATING_KIND")]
        public virtual int RatingKind { get; set; }
		[Column("RATING_NAME")]
        public virtual string RatingName { get; set; }

		[Column("RATING_MIN")]
		public virtual float RatingMin { get; set; }

		[Column("RATING_MAX")]
		public virtual float RatingMax { get; set; }

		[Column("RATING_STEP")]
		public virtual float RatingStep { get; set; }

		public override void Delete()
		{
			Delete(Id);
		}

		public static void Delete(long id)
		{
			using (var db = DB.GetDatabase())
			{
				int cnt = db.Query<TitleRating>().Where(x => x.RatingId == id).Count();
				if (cnt > 0) throw new ApplicationException(string.Format("Can't delete rating provider: it is used by {0} ratings", cnt));
				db.Execute("DELETE FROM RATING_PROVIDER WHERE RATING_ID = @0", id);
			}
		}
    }
}
