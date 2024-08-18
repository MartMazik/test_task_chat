using System.Data;
using Npgsql;
using TestServer.Scripts.Common;
using TestServer.Scripts.Common.Logs;
using TestServer.Scripts.Models;

namespace TestServer.Scripts.Repositories;

public class MessagePsqlRepository(PostgresDal dal) : IMessageRepository
{
    public async Task<Result<bool>> AddMessage(MessageModel messageModel)
    {
        var serialNumber = messageModel.SerialNumber;
        var message = messageModel.Message;
        var date = messageModel.Date;

        const string sql = "INSERT INTO messages (serial_number, message, created_at) VALUES (@serialNumber, @message, @createdAt)";

        var parameters = new[]
        {
            new NpgsqlParameter("@serialNumber", serialNumber),
            new NpgsqlParameter("@message", message),
            new NpgsqlParameter("@createdAt", date)
        };

        try
        {
            var result = await dal.ExecuteNonQueryAsync(sql, parameters);
            
            if (result > 0)
            {
                Logger.Info("Message added.", "Database.AddMessage");
                return new Result<bool>(true);
            }

            Logger.Error("Message not added.", "Database.AddMessage");
            return new Result<bool>(false);
        }
        catch (Exception ex)
        {
            return new Result<bool>($"Adding Message in Database: {ex.Message}. Parameters: SerialNumber={serialNumber}, Message={message}, CreatedAt={date}");
        }
    }

    public async Task<Result<IEnumerable<MessageModel>>> GetMessages(DateTime startDate, DateTime endDate)
    {
        var sql = "SELECT id, serial_number, message, created_at FROM messages WHERE created_at BETWEEN @startDate AND @endDate";

        var parameters = new[]
        {
            new NpgsqlParameter("@startDate", startDate),
            new NpgsqlParameter("@endDate", endDate)
        };

        try
        {

            var dataTable = await dal.ExecuteQueryAsync(sql, parameters);

            var messages = new List<MessageModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                messages.Add(new MessageModel
                {
                    Id = Convert.ToInt32(row["id"]),
                    SerialNumber = Convert.ToInt32(row["serial_number"]),
                    Message = row["message"].ToString(),
                    Date = Convert.ToDateTime(row["created_at"])
                });
            }
            
            Logger.Info($"{messages.Count} messages found. Parameters: StartDate={startDate}, EndDate={endDate}", "Database.GetMessages");

            return new Result<IEnumerable<MessageModel>>(messages);
        }
        catch (Exception ex)
        {
            return new Result<IEnumerable<MessageModel>>($"Getting Messages from Database: {ex.Message}. Parameters: StartDate={startDate}, EndDate={endDate}");
        }
    }
}