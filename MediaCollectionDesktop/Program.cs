using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace MediaCollection
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var baseDir = AppContext.BaseDirectory;
			var configuration = new ConfigurationBuilder()
				.SetBasePath(baseDir)
				.AddJsonFile("appsettings.json", optional: true)
				.Build();

			var dataSource = configuration["DataSource"];
			if (string.IsNullOrWhiteSpace(dataSource))
			{
				dataSource = Path.Combine(baseDir, "..", "..", "..", "..", "MediaCollection", "App_Data", "MediaCollection.s3db");
			}

			if (!Path.IsPathRooted(dataSource))
				dataSource = Path.GetFullPath(Path.Combine(baseDir, dataSource));

			DB.Configure(dataSource);

			Application.Run(new MainForm());
		}
	}
}
