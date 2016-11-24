using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection 
{

	public enum MediaSampleKind {Image, Video}

	[TableName("MEDIA_SAMPLE")]
	[PrimaryKey("SAMLE_ID", AutoIncrement=true)]
    public class MediaSample : IModelWithId
	{
		[Column("SAMLE_ID")]
		public virtual long Id { get; set; }
		[Column("TITLE_ID")]
        public virtual long TitleId { get; set; }
		[Column("MEDIA_KIND")]
		public virtual MediaSampleKind MediaKind { get; set; }
		[Column("EXTENSION")]
        public virtual string Extension { get; set; }
    }
}
