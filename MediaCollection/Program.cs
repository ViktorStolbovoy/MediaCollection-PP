using System;
using System.IO;
using System.Threading.Tasks;
using MediaCollection.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediaCollection
{
	public class Program
	{
		public static int Main(string[] args)
		{
			if (args.Length > 0 && string.Equals(args[0], "hash-password", StringComparison.OrdinalIgnoreCase))
			{
				return RunHashPassword(args);
			}

			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddMemoryCache();
			builder.Services.AddSingleton<ScanSessionStore>();
			builder.Services.AddControllersWithViews()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver =
						new Newtonsoft.Json.Serialization.DefaultContractResolver();
				});

			builder.Services
				.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
				{
					options.Cookie.Name = "mc.auth";
					options.Cookie.HttpOnly = true;
					options.Cookie.SameSite = SameSiteMode.Strict;
					options.SlidingExpiration = true;
					options.ExpireTimeSpan = System.TimeSpan.FromHours(8);
					options.Events.OnRedirectToLogin = ctx =>
					{
						if (ctx.Request.Path.StartsWithSegments("/api"))
						{
							ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
							return Task.CompletedTask;
						}
						ctx.Response.Redirect(ctx.RedirectUri);
						return Task.CompletedTask;
					};
					options.Events.OnRedirectToAccessDenied = ctx =>
					{
						if (ctx.Request.Path.StartsWithSegments("/api"))
						{
							ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
							return Task.CompletedTask;
						}
						ctx.Response.Redirect(ctx.RedirectUri);
						return Task.CompletedTask;
					};
				});

			var app = builder.Build();
			McLog.Factory = app.Services.GetRequiredService<ILoggerFactory>();
			app.UseAuthentication();
			app.UseAuthorization();

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
			app.UseRouting();
			app.UseMiddleware<ReadOnlyApiMiddleware>();
			app.MapControllers();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
			return 0;
		}

		private static int RunHashPassword(string[] args)
		{
			string password = null;
			int iterations = PasswordHasher.DefaultIterations;

			for (int i = 1; i < args.Length; i++)
			{
				if (args[i] == "--iterations" && i + 1 < args.Length && int.TryParse(args[i + 1], out var it))
				{
					iterations = it;
					i++;
				}
				else if (password == null)
				{
					password = args[i];
				}
			}

			if (string.IsNullOrEmpty(password))
			{
				Console.Error.Write("Password: ");
				password = ReadPasswordFromConsole();
				Console.Error.WriteLine();
			}

			if (string.IsNullOrEmpty(password))
			{
				Console.Error.WriteLine("Password is required.");
				return 2;
			}

			var result = PasswordHasher.Hash(password, iterations);
			Console.WriteLine("Add the following to appsettings.json (or set via user-secrets / environment):");
			Console.WriteLine();
			Console.WriteLine("  \"Auth\": {");
			Console.WriteLine($"    \"PasswordSalt\": \"{result.SaltBase64}\",");
			Console.WriteLine($"    \"PasswordHash\": \"{result.HashBase64}\",");
			Console.WriteLine($"    \"Iterations\": {result.Iterations}");
			Console.WriteLine("  }");
			return 0;
		}

		private static string ReadPasswordFromConsole()
		{
			if (Console.IsInputRedirected)
			{
				return Console.In.ReadLine();
			}

			var buffer = new System.Text.StringBuilder();
			while (true)
			{
				var key = Console.ReadKey(intercept: true);
				if (key.Key == ConsoleKey.Enter) break;
				if (key.Key == ConsoleKey.Backspace)
				{
					if (buffer.Length > 0) buffer.Length--;
					continue;
				}
				if (!char.IsControl(key.KeyChar))
				{
					buffer.Append(key.KeyChar);
				}
			}
			return buffer.ToString();
		}
	}
}
