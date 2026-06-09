using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MediaCollection.WebSockets
{
	public sealed class WebSocketHub
	{
		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = null
		};

		private readonly ConcurrentDictionary<Guid, ClientConnection> _clients = new ConcurrentDictionary<Guid, ClientConnection>();
		private readonly ILogger<WebSocketHub> _logger;

		public WebSocketHub(ILogger<WebSocketHub> logger)
		{
			_logger = logger;
		}

		public int ConnectionCount => _clients.Count;

		public async Task HandleAsync(HttpContext context)
		{
			if (!context.WebSockets.IsWebSocketRequest)
			{
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				return;
			}

			var socket = await context.WebSockets.AcceptWebSocketAsync();
			var clientId = Guid.NewGuid();
			var client = new ClientConnection(socket);
			_clients[clientId] = client;

			try
			{
				await SendAsync(socket, "connected", new { clientId, authenticated = context.User?.Identity?.IsAuthenticated == true });
				await ReceiveLoopAsync(clientId, client, context.RequestAborted);
			}
			catch (OperationCanceledException)
			{
				// shutdown
			}
			catch (WebSocketException ex)
			{
				_logger.LogDebug(ex, "WebSocket {ClientId} closed with error", clientId);
			}
			finally
			{
				_clients.TryRemove(clientId, out _);
				if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
				{
					try
					{
						await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
					}
					catch (WebSocketException)
					{
						// already closed
					}
				}
				socket.Dispose();
			}
		}

		public Task BroadcastAsync(string type, object data = null)
		{
			var json = Serialize(type, data);
			var bytes = Encoding.UTF8.GetBytes(json);
			var segment = new ArraySegment<byte>(bytes);

			var tasks = new List<Task>(_clients.Count);
			foreach (var pair in _clients)
			{
				tasks.Add(SendRawAsync(pair.Value.Socket, segment));
			}

			return Task.WhenAll(tasks);
		}

		/// <summary>
		/// Sends a fresh library snapshot to every client that has subscribed to the library view,
		/// tailored to each client's current filter selection.
		/// </summary>
		public async Task BroadcastLibraryAsync()
		{
			var subscribers = _clients
				.Where(kvp => kvp.Value.LibrarySubscription != null)
				.ToList();

			foreach (var pair in subscribers)
			{
				try
				{
					await SendLibraryAsync(pair.Value);
				}
				catch (Exception ex)
				{
					_logger.LogDebug(ex, "Failed to push library snapshot to {ClientId}", pair.Key);
				}
			}
		}

		private static async Task SendLibraryAsync(ClientConnection client)
		{
			var sub = client.LibrarySubscription;
			if (sub == null) return;

			var snapshot = await LibrarySnapshotBuilder.BuildAsync(sub);
			await SendAsync(client.Socket, "library", snapshot);
		}

		private async Task ReceiveLoopAsync(Guid clientId, ClientConnection client, CancellationToken cancellationToken)
		{
			var buffer = new byte[4096];
			var socket = client.Socket;

			while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
			{
				var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
				if (result.MessageType == WebSocketMessageType.Close)
					break;

				if (result.MessageType != WebSocketMessageType.Text)
					continue;

				var text = Encoding.UTF8.GetString(buffer, 0, result.Count);
				if (!TryParseClientMessage(text, out var type, out var payload))
					continue;

				if (string.Equals(type, "ping", StringComparison.OrdinalIgnoreCase))
				{
					await SendAsync(socket, "pong", null);
				}
				else if (string.Equals(type, "subscribe-library", StringComparison.OrdinalIgnoreCase))
				{
					client.LibrarySubscription = LibrarySubscription.FromPayload(payload);
					try
					{
						await SendLibraryAsync(client);
					}
					catch (Exception ex)
					{
						_logger.LogDebug(ex, "Failed to push initial library snapshot to {ClientId}", clientId);
					}
				}
				else if (string.Equals(type, "unsubscribe-library", StringComparison.OrdinalIgnoreCase))
				{
					client.LibrarySubscription = null;
				}
				else if (SeasonsToolHandler.IsSeasonsToolMessage(type))
				{
					await HandleSeasonsToolAsync(clientId, client, type, payload);
				}
			}
		}

		private async Task HandleSeasonsToolAsync(Guid clientId, ClientConnection client, string type, JsonElement payload)
		{
			try
			{
				var (responseType, response, mutated) = await SeasonsToolHandler.HandleAsync(type, payload);
				if (responseType == null)
				{
					return;
				}

				await SendAsync(client.Socket, responseType, response);

				if (mutated)
				{
					await BroadcastLibraryAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Seasons tool handler {Type} failed for client {ClientId}", type, clientId);
				await SendAsync(client.Socket, type + "-response", new SeasonsToolErrorResponse(
					payload.ValueKind == JsonValueKind.Object && payload.TryGetProperty("RequestId", out var rid) && rid.ValueKind == JsonValueKind.String ? rid.GetString() : "",
					ex.Message));
			}
		}

		private static bool TryParseClientMessage(string json, out string type, out JsonElement data)
		{
			type = null;
			data = default;
			if (string.IsNullOrWhiteSpace(json))
				return false;

			try
			{
				using var doc = JsonDocument.Parse(json);
				if (doc.RootElement.TryGetProperty("type", out var prop) && prop.ValueKind == JsonValueKind.String)
				{
					type = prop.GetString();
					if (string.IsNullOrEmpty(type)) return false;
					if (doc.RootElement.TryGetProperty("data", out var dataProp))
					{
						data = dataProp.Clone();
					}
					return true;
				}
			}
			catch (JsonException)
			{
				// ignore malformed client messages
			}

			return false;
		}

		private static Task SendAsync(WebSocket socket, string type, object data)
		{
			var json = Serialize(type, data);
			var bytes = Encoding.UTF8.GetBytes(json);
			return SendRawAsync(socket, new ArraySegment<byte>(bytes));
		}

		private static async Task SendRawAsync(WebSocket socket, ArraySegment<byte> payload)
		{
			if (socket.State != WebSocketState.Open)
				return;

			try
			{
				await socket.SendAsync(payload, WebSocketMessageType.Text, true, CancellationToken.None);
			}
			catch (WebSocketException)
			{
				// client disconnected
			}
		}

		private static string Serialize(string type, object data)
		{
			if (data == null)
				return JsonSerializer.Serialize(new { type }, JsonOptions);
			return JsonSerializer.Serialize(new { type, data }, JsonOptions);
		}

		private sealed class ClientConnection
		{
			public ClientConnection(WebSocket socket)
			{
				Socket = socket;
			}

			public WebSocket Socket { get; }
			public LibrarySubscription LibrarySubscription { get; set; }
		}
	}
}
