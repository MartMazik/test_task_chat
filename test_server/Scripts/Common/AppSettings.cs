namespace TestServer.Scripts.Common;

public static class AppSettings
{
    public static string? ConnectionString { get; private set; } = string.Empty;
    public static string LogsDirectory { get; private set; } = string.Empty;
    
    public static void Initialize(ConfigurationManager builderConfiguration)
    {
        ConnectionString = builderConfiguration.GetConnectionString("DefaultConnection");
        LogsDirectory = builderConfiguration.GetValue<string>("LogsDirectory") ?? "Logs";
    }
}