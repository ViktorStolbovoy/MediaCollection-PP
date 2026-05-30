using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MediaCollection
{
	/// <summary>
	/// Allows GET/HEAD/OPTIONS on /api for everyone, but only authenticated users may
	/// issue mutating requests (POST/PUT/PATCH/DELETE). The login/logout endpoints are
	/// explicitly excluded so that anonymous users can authenticate.
	/// </summary>
	public sealed class ReadOnlyApiMiddleware
	{
		private readonly RequestDelegate _next;

		public ReadOnlyApiMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var path = context.Request.Path.Value ?? "";
			if (!path.StartsWith("/api"))
			{
				await _next(context);
				return;
			}

			if (path.StartsWith("/api/auth"))
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

			if (context.User?.Identity?.IsAuthenticated == true)
			{
				await _next(context);
				return;
			}

			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(
				"{\"error\":\"Authentication required. Click Configure to log in before making changes.\"}");
		}
	}
}
