// See https://aka.ms/new-console-template for more information

using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using PuppeteerSharp.Contrib.PageObjects;
using VConsole.PageObject;
using VConsole.Util;

TaskCompletionSource<bool> closed = new TaskCompletionSource<bool>();

// await new BrowserFetcher().DownloadAsync();

var extra = new PuppeteerExtra();

extra.Use(new StealthPlugin());

var browser = await extra.LaunchAsync(new LaunchOptions
{
    Headless = false,
    UserDataDir = Path.Combine(Environment.CurrentDirectory, "userData"),
    Args = ["--window-size=1920,1080"],
    DefaultViewport = new ViewPortOptions
    {
        Width = 1920,
        Height = 1080
    },
    ExecutablePath = ConfigurationUtil.Configuration["GoogleChromeExecutePath"]!
});

browser.Closed += (sender, events) => { closed.TrySetResult(true); };

var page = await browser.NewPageAsync();

MainPage mainPage = await page.GoToAsync<MainPage>(ConfigurationUtil.Configuration["UrlBase:JavLibrary"]!);
UserPage userPage = await mainPage.GoToUserPage();
UserWantToPage userWantToPage = await userPage.GoToUserWantToPage();

int[] indexs = userWantToPage.AllPageSelectorsIndex;

Dictionary<string, string> videoList = new Dictionary<string, string>();

foreach (int index in indexs)
{
    userWantToPage = await userWantToPage.GoToUserWantToPage(index);
    foreach (var video in await userWantToPage.GetUserWantToList())
    {
        videoList.Add(video.Key, video.Value);
    }
}

var videoDetails = videoList.Select(pair =>
{
    LoggerInstance.LogInformation(pair.Key);
    VideoDetailPage videoDetailPage = page.GoToAsync<VideoDetailPage>(pair.Value).Result;
    Task.Delay(9000).Wait();
    return videoDetailPage.GetVideoDetail().Result;
}).ToList();


await closed.Task;