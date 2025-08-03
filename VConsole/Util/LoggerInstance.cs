using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;


namespace VConsole.Util;

public static class LoggerInstance<T>
{
    public static ILogger<T> Logger => HostProvider.HostInstance!.Services.GetRequiredService<ILogger<T>>();
}