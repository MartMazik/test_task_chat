using TestServer.Scripts.Common;
using TestServer.Scripts.Models;

namespace TestServer.Scripts.Repositories;

public interface IMessageRepository
{
    public Task<Result<bool>> AddMessage(MessageModel messageModel);
    public Task<Result<IEnumerable<MessageModel>>> GetMessages(DateTime startDate, DateTime endDate);
}