using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection 
{

	[TableName("app_config")]
	[PrimaryKey("CONFIG_KEY")]
	public class AppConfig 
	{
		[Column("CONFIG_KEY")]
		public virtual string ConfigKey { get; set; }
		[Column("CONFIG_VALUE")]
		public virtual string ConfigValue { get; set; }
	}
}
