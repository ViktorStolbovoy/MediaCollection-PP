using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection {
	[TableName("TILTE_EXTERNAL_LINK")]
	[PrimaryKey("TITLE_ID,PROVIDER_ID", AutoIncrement=false)]
	public class TilteExternalLink 
	{
		[Column("TITLE_ID")]
		public virtual long TitleId { get; set; }
		[Column("PROVIDER_ID")]
		public virtual int ProviderId { get; set; }
		[Column("LINK_DATA")]
		public virtual string LinkData { get; set; }
	}
}
