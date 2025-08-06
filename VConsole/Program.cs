// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VConsole.DBContext;
using VConsole.Services;

namespace VConsole;

internal class Program
{
    public static async Task Main(string[] args)
    {
        TaskCompletionSource<bool> closed = new TaskCompletionSource<bool>();

        using IHost host = Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<ApplicationDbContext>();
                // services.AddHostedService<BrowserHostedService>();
                services.AddSingleton<IHostedService, BrowserHostedService>();
            })
            .Build();
        
        Task runner = host.RunAsync();

        var service = (BrowserHostedService)host.Services.GetRequiredService<IHostedService>();

        await service.WaitForServiceInitializationCompleted();

        await service.GetVideoDetail("MIDA-241");

        await runner;
    }
}