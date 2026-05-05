using System;
using Microsoft.Data.Sqlite;
using NPoco;

namespace MediaCollection
{
	public static class DB
	{
		private static string s_connectionString;

		public static void Configure(string sqliteDatabasePath)
		{
			if (string.IsNullOrWhiteSpace(sqliteDatabasePath))
				throw new ArgumentException("Database path is required.", nameof(sqliteDatabasePath));

			var cb = new SqliteConnectionStringBuilder
			{
				DataSource = sqliteDatabasePath,
				Mode = SqliteOpenMode.ReadWriteCreate,
				ForeignKeys = true,
				Cache = SqliteCacheMode.Shared
			};
			s_connectionString = cb.ConnectionString;
		}

		private static string GetConnectionString()
		{
			if (string.IsNullOrEmpty(s_connectionString))
				throw new InvalidOperationException("Database is not configured. Call DB.Configure(path) at startup.");

			return s_connectionString;
		}

		public static SqliteConnection GetConnection()
		{
			var conn = new SqliteConnection(GetConnectionString());
			conn.Open();
			return conn;
		}

		public static IDatabase GetDatabase()
		{
			var connection = new SqliteConnection(GetConnectionString());
			connection.Open();
			var db = new Database(connection);
			db.KeepConnectionAlive = false;
			return db;
		}
	}
}
