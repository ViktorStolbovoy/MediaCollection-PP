using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace MediaCollection
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddMemoryCache();
			builder.Services.AddSingleton<ScanSessionStore>();
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
			{
				dataSource = Path.Combine(contentRoot, "App_Data", "MediaCollection.s3db");
			}

			DB.Configure(dataSource);

			var mediaPath = app.Configuration["MediaSamplesPath"];
			if (string.IsNullOrWhiteSpace(mediaPath))
			{
				mediaPath = Path.Combine(contentRoot, "App_Data", "media_samples");
			}

			Directory.CreateDirectory(mediaPath);
			MediaSamplePersistence.s_dataFolder = mediaPath;

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
			app.UseMiddleware<ReadOnlyApiMiddleware>();
			app.MapControllers();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
