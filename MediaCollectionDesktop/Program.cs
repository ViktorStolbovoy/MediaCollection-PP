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
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
				.Build();

			var dataSource = configuration["DataSource"];
			if (string.IsNullOrWhiteSpace(dataSource))
				throw new InvalidOperationException("Set \"DataSource\" in appsettings.json (copied next to the executable).");

			if (!Path.IsPathRooted(dataSource))
				dataSource = Path.GetFullPath(Path.Combine(baseDir, dataSource));

			DB.Configure(dataSource);

			Application.Run(new MainForm());
		}
	}
}
