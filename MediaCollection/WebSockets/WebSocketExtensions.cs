using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MediaCollection.WebSockets
{
	public static class WebSocketExtensions
	{
		public static IServiceCollection AddMediaCollectionWebSockets(this IServiceCollection services)
		{
			services.AddSingleton<WebSocketHub>();
			return services;
		}

		public static WebApplication UseMediaCollectionWebSockets(this WebApplication app)
		{
			app.UseWebSockets();
			app.Map("/ws", async (HttpContext ctx, WebSocketHub hub) => await hub.HandleAsync(ctx));
			return app;
		}
	}
}
