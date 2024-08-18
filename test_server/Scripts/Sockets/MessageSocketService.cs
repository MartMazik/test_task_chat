using System.Text.Json;
using TestServer.Scripts.Common.Logs;
using TestServer.Scripts.Models;

namespace TestServer.Scripts.Sockets
{
    public class MessageSocketService : IMessageSocketService
    {
        public Task SendMessageAsync(MessageModel message)
        {
            try
            {
                var messageJson = JsonSerializer.Serialize(message);
                WebSocketHandler.BroadcastMessage(messageJson);
            }
            catch (Exception ex)
            {
                Logger.Error($"When sending a message \"{message.Message}\": {ex.Message}", "WebSocket");
            }

            return Task.CompletedTask;
        }
    }
}