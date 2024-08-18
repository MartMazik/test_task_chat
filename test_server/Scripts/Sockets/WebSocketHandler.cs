using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using TestServer.Scripts.Common.Logs;

namespace TestServer.Scripts.Sockets
{
    public static class WebSocketHandler
    {
        private static readonly ConcurrentDictionary<string, WebSocket> Clients = new();

        public static async Task HandleWebSocket(HttpContext context)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var clientId = Guid.NewGuid().ToString();
            Clients[clientId] = webSocket;

            Logger.Info($"WebSocket connection established. ClientSocket ID: {clientId}", "WebSocket");

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Logger.Info($"Received message from client {clientId}: {message}", "WebSocket");
                }
            } while (!result.CloseStatus.HasValue);

            Clients.TryRemove(clientId, out _);
            Logger.Info($"WebSocket connection closed. ClientSocket ID: {clientId}", "WebSocket");

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public static void BroadcastMessage(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var client in Clients.Values)
            {
                if (client.State == WebSocketState.Open)
                {
                    _ = client.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                    Logger.Info("Message sent to a socket client.", "WebSocket");
                }
            }
        }
    }
}
