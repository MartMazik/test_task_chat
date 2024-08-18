namespace TestServer.Scripts.Common.Logs;

public class DateFileLogDestination : ILogDestination
{
    private readonly string _logDirectory;

    public DateFileLogDestination(string logDirectory)
    {
        _logDirectory = logDirectory;
        Directory.CreateDirectory(logDirectory);
    }

    public async Task LogAsync(DateTime dateTime, string message, string source, Logger.LogLevel level)
    {
        var logPath = Path.Combine(_logDirectory, $"{dateTime:yyyy-MM-dd}.log");
        var logMessage = $"{dateTime:HH:mm:ss.fff} {level} [{source}] {message}";

        await Task.Run(() =>
        {
            File.AppendAllText(logPath, logMessage + Environment.NewLine);
        });
    }
}