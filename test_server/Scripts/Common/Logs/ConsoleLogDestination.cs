namespace TestServer.Scripts.Common.Logs;

public class ConsoleLogDestination : ILogDestination
{
    public async Task LogAsync(DateTime dateTime, string message, string source, Logger.LogLevel level)
    {
        await Task.Run(() => { Console.WriteLine($"{dateTime:HH:mm:ss.fff} {level} [{source}] {message}"); });
    }
}