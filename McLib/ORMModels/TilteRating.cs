using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection {

	[TableName("title_rating")]
	[PrimaryKey("TITLE_ID,RATING_ID", AutoIncrement = false)]
	public class TitleRating 
	{
		[Column("TITLE_ID")]
		public virtual long TitleId { get; set; }
		[Column("RATING_ID")]
		public virtual long RatingId { get; set; }
		[Column("RATING_VALUE")]
		public virtual float RatingValue { get; set; }
	}

	public class TitleRatingWithName : TitleRating
	{
		[ComputedColumn("RATING_NAME")]
		public virtual string RatingName { get; set; }

		[ComputedColumn("RATING_MIN")]
		public virtual float RatingMin { get; set; }

		[ComputedColumn("RATING_MAX")]
		public virtual float RatingMax { get; set; }

		[ComputedColumn("RATING_STEP")]
		public virtual float RatingStep { get; set; }

		/// <summary>
		/// Upsert
		/// </summary>
		public void Set(long titleId)
		{
			using (var db = DB.GetDatabase())
			{
				if (TitleId <= 0)
				{
					TitleId = titleId;
					db.Insert(this);
				}
				else
				{
					db.Update(this);
				}
			}
		}
	}
}
