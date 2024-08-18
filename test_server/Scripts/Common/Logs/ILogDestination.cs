namespace TestServer.Scripts.Common.Logs;

public interface ILogDestination
{
    Task LogAsync(DateTime dateTime, string message, string source, Logger.LogLevel level);
}