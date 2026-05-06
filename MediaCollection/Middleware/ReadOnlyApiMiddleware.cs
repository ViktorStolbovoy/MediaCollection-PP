using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MediaCollection
{
	public sealed class ReadOnlyApiMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _configuration;

		public ReadOnlyApiMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;
			_configuration = configuration;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (!_configuration.GetValue<bool>("ReadOnly"))
			{
				await _next(context);
				return;
			}

			var path = context.Request.Path.Value ?? "";
			if (!path.StartsWith("/api"))
			{
				await _next(context);
				return;
			}

			var method = context.Request.Method;
			if (method is "GET" or "HEAD" or "OPTIONS")
			{
				await _next(context);
				return;
			}

			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync("{\"error\":\"Read-only mode is enabled; mutations are disabled.\"}");
		}
	}
}
