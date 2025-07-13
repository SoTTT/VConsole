using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;


namespace VConsole.Util;

public static class LoggerInstance
{
    public static ILogger Logger { get; set; }

    static LoggerInstance()
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        Logger = factory.CreateLogger("VConsole");
        Logger.LogInformation("日志初始化完成");
    }

    public static void LogDebug(string message, params object?[] args)
    {
        Logger.LogDebug(message, args);
    }

    public static void LogInformation(string message, params object?[] args)
    {
        Logger.LogInformation(message, args);
    }

    public static void LogError(string message, params object?[] args)
    {
        Logger.LogError(message, args);
    }
}