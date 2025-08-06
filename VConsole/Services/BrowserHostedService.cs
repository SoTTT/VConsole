using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using PuppeteerSharp.Contrib.PageObjects;
using VConsole.DBContext;
using VConsole.Entity;
using VConsole.PageObject;
using VConsole.Util;

namespace VConsole.Services;

public class BrowserHostedService(
    IConfiguration configuration,
    ILogger<BrowserHostedService> logger,
    IHostApplicationLifetime applicationLifetime,
    IServiceProvider serviceProvider)
    : IHostedService
{
    private readonly TaskCompletionSource<bool> _closed = new();

    private IBrowser _browser = null!;

    private IPage _page = null!;

    private readonly TaskCompletionSource<bool> _initializationCompleted = new();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var extra = new PuppeteerExtra();

        extra.Use(new StealthPlugin());

        try
        {
            // 尝试连接到已经运行的浏览器实例
            var browserUrl = "http://127.0.0.1:9222/json/version";
            using var client = new HttpClient();
            var response = await client.GetStringAsync(browserUrl, cancellationToken);
            var versionInfo = System.Text.Json.JsonDocument.Parse(response);
            var webSocketDebuggerUrl = versionInfo.RootElement.GetProperty("webSocketDebuggerUrl").GetString();

            _browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = webSocketDebuggerUrl,
                DefaultViewport = null // 设置默认视口以确保新页面能正确填充窗口
            });
            logger.LogInformation("Connected to existing browser instance.");
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                "Failed to connect to existing browser, launching a new one. Error: {ExMessage}", ex.Message);
            // 如果连接失败，则启动新的浏览器实例
            _browser = await extra.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                UserDataDir = Path.Combine(Environment.CurrentDirectory, "userData"),
                Args = ["--window-size=1920,1080", "--remote-debugging-port=9222"], // 启用远程调试
                DefaultViewport = null,
                ExecutablePath = configuration["GoogleChromeExecutePath"]!
            });
        }
        
        _browser.Closed += (sender, events) =>
        {
            _closed.TrySetResult(true);
            applicationLifetime.StopApplication();
        };

        logger.LogInformation("browser connected");

        _page = await _browser.NewPageAsync();

        _initializationCompleted.SetResult(true);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("wait for browser closed...");
        await _closed.Task;
        logger.LogInformation("browser closed");
    }

    public async Task GetVideoDetail(string videoId)
    {
        MainPage mainPage = await _page.GoToAsync<MainPage>(configuration["UrlBase:JavLibrary"]!);
        mainPage = await mainPage.AutoSkipWarningPrompt();

        VideoDetailPage videoDetailPage = await mainPage.SearchVideo(videoId);

        VideoDetailRecord entity = await videoDetailPage.GetVideoDetail();

        using var scope = serviceProvider.CreateScope();
        
        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await applicationDbContext.VideoDetailRecords.AddAsync(entity);

        await applicationDbContext.SaveChangesAsync();
    }

    public async Task WaitForServiceInitializationCompleted()
    {
        if (_initializationCompleted.Task.IsCompleted)
        {
            return;
        }

        await _initializationCompleted.Task;
    }
}