using Microsoft.Extensions.Configuration;

namespace VConsole.Util;

public static class ConfigurationUtil
{
    public static IConfiguration Configuration { get; }

    static ConfigurationUtil()
    {
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }
}