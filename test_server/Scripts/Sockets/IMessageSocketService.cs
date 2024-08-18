using TestServer.Scripts.Models;

namespace TestServer.Scripts.Sockets;

public interface IMessageSocketService
{
    public Task SendMessageAsync(MessageModel message);
}