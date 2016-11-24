using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection 
{
	[TableName("PROPERTY")]
	[PrimaryKey("PROPERTY_ID", AutoIncrement=true)]
	public class Property : IModelWithId
	{
		[Column("PROPERTY_ID")]
		public virtual long Id { get; set; }
		[Column("PROPERTY_KIND")]
		public virtual string PropertyKind { get; set; }
		[Column("PROPERTY_NAME")]
		public virtual string PropertyName { get; set; }
	}
}
