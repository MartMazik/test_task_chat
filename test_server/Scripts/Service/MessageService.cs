using TestServer.Scripts.Common;
using TestServer.Scripts.Common.Logs;
using TestServer.Scripts.Models;
using TestServer.Scripts.Repositories;
using TestServer.Scripts.Sockets;

namespace TestServer.Scripts.Service;

public class MessageService(IMessageRepository messageRepository, IMessageSocketService messageSocketService) : IMessageService
{
    public async Task<Result<int>> SendMessage(MessageModel messageModel)
    {
        messageModel.Date = DateTime.UtcNow;

        if (string.IsNullOrWhiteSpace(messageModel.Message))
            return new Result<int>("Message is empty");

        messageModel.Message = messageModel.Message.Trim();

        if (messageModel.SerialNumber < 0)
            return new Result<int>("Serial number is negative");

        var parts = new List<string>();
        if (messageModel.Message.Length > 128)
        {
            parts = SplitInParts(messageModel.Message, 128);
            Logger.Warning($"Message is too long: {messageModel.Message.Length}. Splitting into {parts.Count} parts", "SendMessage");
        }
        else
        {
            parts.Add(messageModel.Message);
        }
        

        var nextSerialNumber = messageModel.SerialNumber;

        foreach (var part in parts)
        {
            var message = new MessageModel
            {
                Message = part,
                Date = messageModel.Date,
                SerialNumber = nextSerialNumber++
            };

            var result = await messageRepository.AddMessage(message);
            
            if (result.IsError)
            {
                await messageSocketService.SendMessageAsync(message);
                return new Result<int>(result.Message);
            }
            
            await messageSocketService.SendMessageAsync(message);
        }

        return new Result<int>(nextSerialNumber);
    }


    public async Task<Result<List<MessageModel>>> GetMessages(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            return new Result<List<MessageModel>>("Start date is greater than end date");

        var result = await messageRepository.GetMessages(startDate, endDate);

        if (result.IsError)
            return new Result<List<MessageModel>>(result.Message);

        var messages = result.Data;

        return messages != null ? new Result<List<MessageModel>>(messages.ToList()) : new Result<List<MessageModel>>(new List<MessageModel>());
    }

    private static List<string> SplitInParts(string message, int partLength)
    {
        var parts = new List<string>();
        for (var i = 0; i < message.Length; i += partLength)
        {
            parts.Add(message.Substring(i, Math.Min(partLength, message.Length - i)));
        }

        return parts;
    }
}