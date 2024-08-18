using TestServer.Scripts.Common;
using TestServer.Scripts.Models;

namespace TestServer.Scripts.Repositories
{
    public class MessageListRepository : IMessageRepository
    {
        private static readonly List<MessageModel> Messages = new List<MessageModel>();

        public async Task<Result<bool>> AddMessage(MessageModel messageModel)
        {
            try
            {
                messageModel.Id = Messages.Count + 1;
                Messages.Add(messageModel);
                return await Task.FromResult(new Result<bool>(true));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Result<bool>(ex.Message));
            }
        }

        public async Task<Result<IEnumerable<MessageModel>>> GetMessages(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = Messages.Where(x => x.Date >= startDate && x.Date <= endDate).ToList();
                return await Task.FromResult(new Result<IEnumerable<MessageModel>>(result));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Result<IEnumerable<MessageModel>>(ex.Message));
            }
        }
    }
}
