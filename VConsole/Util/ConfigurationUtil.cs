using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VConsole.Util;

public static class ConfigurationUtil
{
    public static IConfiguration Configuration =>
        HostProvider.HostInstance!.Services.GetRequiredService<IConfiguration>();
}