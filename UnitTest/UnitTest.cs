using PuppeteerSharp;
using PuppeteerSharp.Contrib.PageObjects;
using VConsole.Entity;
using VConsole.PageObject;
using VConsole.Util;

namespace UnitTest;

public class UnitTest
{
    private readonly TaskCompletionSource<bool> _closed = new();
    
    private async Task<IBrowser> IniBrowser()
    {
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = false,
            UserDataDir = Path.Combine(Environment.CurrentDirectory, "userData"),
            Args = ["--window-size=1920,1080"],
            DefaultViewport = new ViewPortOptions
            {
                Width = 1000,
                Height = 1080
            },
            ExecutablePath = ConfigurationUtil.Configuration["GoogleChromeExecutePath"]!,
            
        });

        browser.Closed += (_, _) => _closed.SetResult(true);

        return browser;
    }

    [Fact]
    public async Task TestSearchVideoWithVideoCode()
    {
        var browser = await IniBrowser();

        var page = await browser.NewPageAsync();

        MainPage mainPage = await page.GoToAsync<MainPage>(ConfigurationUtil.Configuration["UrlBase:JavLibrary"]!);
        await mainPage.AutoSkipWarningPrompt();

        VideoDetailPage videoDetailPage = await mainPage.SearchVideo("MIDA-241");

        VideoDetailRecord record = await videoDetailPage.GetVideoDetail();

        await _closed.Task;
    }

    [Fact]
    public async Task TestUserLogin()
    {
        var browser = await IniBrowser();
        
        var page = await browser.NewPageAsync();
        MainPage mainPage = await page.GoToAsync<MainPage>(ConfigurationUtil.Configuration["UrlBase:JavLibrary"]!);
        await mainPage.AutoSkipWarningPrompt();

        if (!await mainPage.IsLogin())
        {
            await mainPage.RequireUserLogin();
        }
    }
}