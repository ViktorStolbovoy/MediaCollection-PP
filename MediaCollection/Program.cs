using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace MediaCollection
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllersWithViews()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver =
						new Newtonsoft.Json.Serialization.DefaultContractResolver();
				});

			var app = builder.Build();

			var contentRoot = app.Environment.ContentRootPath;

			var dataSource = app.Configuration["DataSource"];
			if (string.IsNullOrWhiteSpace(dataSource))
				dataSource = Path.Combine(contentRoot, "App_Data", "MediaCollection.s3db");
			else if (!Path.IsPathRooted(dataSource))
				dataSource = Path.Combine(contentRoot, dataSource);

			DB.Configure(dataSource);

			if (app.Environment.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseExceptionHandler("/Home/Error");

			app.UseStaticFiles();

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(contentRoot, "Content")),
				RequestPath = "/Content"
			});

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(contentRoot, "Scripts")),
				RequestPath = "/Scripts"
			});

			app.UseRouting();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
