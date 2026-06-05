using System.Threading.Tasks;
using MediaCollection.WebSockets;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MediaCollection.Filters
{
	/// <summary>
	/// Notifies connected WebSocket clients after successful mutating API calls.
	/// </summary>
	public sealed class DataChangedBroadcastFilter : IAsyncActionFilter
	{
		private readonly WebSocketHub _hub;

		public DataChangedBroadcastFilter(WebSocketHub hub)
		{
			_hub = hub;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var executed = await next();
			if (executed.Exception != null) {
				return;
			}

			var http = context.HttpContext;
			var path = http.Request.Path.Value ?? "";
			if (!path.StartsWith("/api") || path.StartsWith("/api/auth")) {
				return;
			}

			var method = http.Request.Method;
			if (method is not ("POST" or "PUT" or "PATCH" or "DELETE")) {
				return;
			}

			var status = http.Response.StatusCode;
			if (status < 200 || status >= 300) {
				return;
			}

			await _hub.BroadcastLibraryAsync();
		}
	}

}
