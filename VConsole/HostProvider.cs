using Microsoft.Extensions.Hosting;

namespace VConsole;

public static partial class HostProvider
{
    public static IHost? HostInstance { get; set; }
}