using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection 
{
	[TableName("EXTERNAL_PROVIDER")]
	[PrimaryKey("PROVIDER_ID", AutoIncrement = true)]
	public class ExternalProvider : IModelWithId
	{
		[Column("PROVIDER_ID")]
		public virtual long Id { get; set; }
		[Column("PROVIDER_KIND")]
		public virtual string ProviderKind { get; set; }
		[Column("PROVIDER_NAME")]
		public virtual string ProviderName { get; set; }
		[Column("URL_TEMPLATE")]
		public virtual string UrlTemplate { get; set; }
	}
}
