using System;
using System.Text;
using System.Collections.Generic;
using NPoco;

namespace MediaCollection 
{
	[TableName("title_property")]
	[PrimaryKey("Title_Id,Property_Id", AutoIncrement = false)]	
	public class TitleProperty 
	{
		[Column("TITLE_ID")]
		public virtual long Title_Id { get; set; }
		[Column("PROPERTY_ID")]
		public virtual long Property_Id { get; set; }
		[Column("PROPERTY_VALUE")]
		public virtual string PropertyValue { get; set; }

	
	}
}
