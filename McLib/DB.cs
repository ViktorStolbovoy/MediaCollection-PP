using System;
using System.Collections.Generic; 
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Configuration;

namespace MediaCollection
{
	public static class DB
	{
		private static string s_connectionString = GetConnectionStringInternal();

		private static string GetConnectionStringInternal()
		{
			//TODO : set path from config
			var cb = new SQLiteConnectionStringBuilder {
				DataSource = ConfigurationManager.AppSettings["DataSource"],
				//DataSource = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data"), "MediaCollection.s3db"),
				ForeignKeys = true,
				DateTimeFormat = SQLiteDateFormats.ISO8601
			};


			return cb.ToString();
		}

		public static string GetConnectionString()
		{
			return s_connectionString;
		}

		public static SQLiteConnection GetConnection()
		{
			var conn = new SQLiteConnection(s_connectionString);
			//"Server=127.0.0.1;Port=5432;Database=MediaCollection;User Id=sa;Password=4wg;Enlist=true; SSL=True; Sslmode=Prefer");
			conn.Open();
			return conn;
		}

		public static NPoco.IDatabase GetDatabase()
		{
			return new NPoco.Database(s_connectionString, NPoco.DatabaseType.SQLite);
		}

	}
}