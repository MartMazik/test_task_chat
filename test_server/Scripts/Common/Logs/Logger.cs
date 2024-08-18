namespace TestServer.Scripts.Common.Logs;

public static class Logger
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    private static readonly List<ILogDestination> Destinations = [];

    public static void AddDestination(ILogDestination destination) => Destinations.Add(destination);

    private static async Task LogAsync(LogLevel level, string source, string message)
    {
        var dateTime = DateTime.Now;
        var tasks = Destinations.Select(destination => destination.LogAsync(dateTime, message, source, level)).ToList();
        await Task.WhenAll(tasks);
    }

    public static void Log(LogLevel level, string message, string source = "General")
    {
        _ = LogAsync(level, source, message);
    }

    public static void Info(string message, string source = "General")
    {
        _ = LogAsync(LogLevel.Info, source, message);
    }

    public static void Warning(string message, string source = "General")
    {
        _ = LogAsync(LogLevel.Warning, source, message);
    }

    public static void Error(string message, string source = "General")
    {
        _ = LogAsync(LogLevel.Error, source, message);
    }
}