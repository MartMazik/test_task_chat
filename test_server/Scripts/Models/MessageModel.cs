namespace TestServer.Scripts.Models;

public class MessageModel
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int SerialNumber { get; set; }
}