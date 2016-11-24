using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection 
{
	public enum TitleKind { Title, Season, Series, Album, Episode, Track, AlbumArtist }

	[TableName("title")]
	[PrimaryKey("TITLE_ID", AutoIncrement=true)]
	public class Title : IModelWithId
	{
		[Column("TITLE_ID")]
		public virtual long Id { get; set; }
		[Column("TITLE_NAME")]
		public virtual string TitleName { get; set; }
		[Column("KIND")]
		public virtual TitleKind Kind { get; set; }
		[Column("DATE_ADDED_UTC")]
		public virtual string DateAddedUtc { get; set; }
		[Column("DATE_MODIFIED_UTC")]
		public virtual string DateModifiedUtc { get; set; }
		[Column("PARENT_TITLE_ID")]
		public virtual long? ParentTitleId { get; set; }
		[Column("DESCRIPTION")]
		public virtual string Description { get; set; }

		[Column("ORD")]
		public virtual int Ord 
		{
			get 
			{
				return Season * SEASON_ORD_MULTIPLIER + Disk * DISK_ORD_MULTIPLIER + EpisodeOrTrack;
			}
			set
			{
				Season = value / SEASON_ORD_MULTIPLIER;
				int theRest = value % SEASON_ORD_MULTIPLIER;
				Disk = theRest / DISK_ORD_MULTIPLIER;
				EpisodeOrTrack = theRest % DISK_ORD_MULTIPLIER;
			}
		}
		[Column("IMDB_ID")]
		public virtual string ImdbId { get; set; }
		[Column("RELEASE_YEAR")]
		public virtual int Year { get; set; }

		[Ignore]
		public int Season;
		[Ignore]
		public int Disk;
		[Ignore]
		public int EpisodeOrTrack;

		public override string ToString()
		{
			return TitleName;
		}

		private const int SEASON_ORD_MULTIPLIER = 1000000;
		private const int DISK_ORD_MULTIPLIER = 1000;
	}
}
