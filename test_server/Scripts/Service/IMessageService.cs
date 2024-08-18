using TestServer.Scripts.Common;
using TestServer.Scripts.Models;

namespace TestServer.Scripts.Service;

public interface IMessageService
{
    public Task<Result<int>> SendMessage(MessageModel messageModel);
    public Task<Result<List<MessageModel>>> GetMessages(DateTime startDate, DateTime endDate);
}