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
                services.AddHostedService<BrowserHostedService>();
            })
            .Build();

        var service = host.Services.GetService(typeof(BrowserHostedService));

        // HostProvider.HostInstance = host;
        //
        // var extra = new PuppeteerExtra();
        //
        // extra.Use(new StealthPlugin());
        //
        // IBrowser browser;
        // try
        // {
        //     // 尝试连接到已经运行的浏览器实例
        //     var browserUrl = "http://127.0.0.1:9222/json/version";
        //     using var client = new HttpClient();
        //     var response = await client.GetStringAsync(browserUrl);
        //     var versionInfo = System.Text.Json.JsonDocument.Parse(response);
        //     var webSocketDebuggerUrl = versionInfo.RootElement.GetProperty("webSocketDebuggerUrl").GetString();
        //
        //     browser = await Puppeteer.ConnectAsync(new ConnectOptions
        //     {
        //         BrowserWSEndpoint = webSocketDebuggerUrl,
        //         DefaultViewport = null  // 设置默认视口以确保新页面能正确填充窗口
        //     });
        //     LoggerInstance<Program>.Logger.LogInformation("Connected to existing browser instance.");
        // }
        // catch (Exception ex)
        // {
        //     LoggerInstance<Program>.Logger.LogWarning("Failed to connect to existing browser, launching a new one. Error: {ExMessage}", ex.Message);
        //     // 如果连接失败，则启动新的浏览器实例
        //     browser = await extra.LaunchAsync(new LaunchOptions
        //     {
        //         Headless = false,
        //         UserDataDir = Path.Combine(Environment.CurrentDirectory, "userData"),
        //         Args = ["--window-size=1920,1080", "--remote-debugging-port=9222"], // 启用远程调试
        //         DefaultViewport = null,
        //         ExecutablePath = ConfigurationUtil.Configuration["GoogleChromeExecutePath"]!
        //     });
        //     
        //     browser.Closed += (sender, events) => { closed.TrySetResult(true); };
        // }
        //
        // var page = await browser.NewPageAsync();
        //
        // MainPage mainPage = await page.GoToAsync<MainPage>(ConfigurationUtil.Configuration["UrlBase:JavLibrary"]!);
        // mainPage = await mainPage.AutoSkipWarningPrompt();

// VideoDetailPage videoDetailPage = await mainPage.SearchVideo("MIDA-241");
// VideoDetailRecord record = await videoDetailPage.GetVideoDetail();

        // ApplicationDbContext applicationDbContext =
        //     (ApplicationDbContext)host.Services.GetService(typeof(ApplicationDbContext))!;
        //
        // await applicationDbContext.VideoDetailRecords
        //     .Where(p => p.VideoId == "MIDA-241")
        //     .ExecuteDeleteAsync();
        // await applicationDbContext.SaveChangesAsync();


// if (!await mainPage.IsLogin())
// {
//     mainPage = await mainPage.RequireUserLogin();
// }
//
// UserPage userPage = await mainPage.GoToUserPage();
// UserWantToPage userWantToPage = await userPage.GoToUserWantToPage();
//
// int[] indexes = userWantToPage.AllPageSelectorsIndex;

// Dictionary<string, string> videoList = new Dictionary<string, string>();
//
// foreach (int index in indexes)
// {
//     userWantToPage = await userWantToPage.GoToUserWantToPage(index);
//     foreach (var video in await userWantToPage.GetUserWantToList())
//     {
//         videoList.Add(video.Key, video.Value);
//     }
// }
//
// var videoDetails = videoList.Select(pair =>
// {
//     LoggerInstance.LogInformation(pair.Key);
//     VideoDetailPage videoDetailPage = page.GoToAsync<VideoDetailPage>(pair.Value).Result;
//     Task.Delay(9000).Wait();
//     return videoDetailPage.GetVideoDetail().Result;
// }).ToList();


        // await Task.WhenAny(closed.Task, host.RunAsync());
        await host.RunAsync();
    }
}