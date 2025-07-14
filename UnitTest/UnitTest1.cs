using PuppeteerSharp;
using PuppeteerSharp.Contrib.PageObjects;
using VConsole.Entity;
using VConsole.PageObject;

namespace UnitTest;

public class UnitTest1
{
    private readonly TaskCompletionSource<bool> _closed = new();

    [Fact]
    public async Task TestSearchVideoWithVideoCode()
    {
        // await new BrowserFetcher().DownloadAsync();

        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = false,
            UserDataDir = "C:\\Users\\Mand\\RiderProjects\\VConsole\\VConsole\\bin\\Debug\\net8.0\\userData",
            Args = ["--window-size=1920,1080"],
            DefaultViewport = new ViewPortOptions
            {
                Width = 1920,
                Height = 1080
            },
            ExecutablePath =
                "C:\\Users\\Mand\\RiderProjects\\VConsole\\VConsole\\bin\\Debug\\net8.0\\Chrome\\Win64-122.0.6261.111\\chrome-win64\\chrome.exe"
        });

        browser.Closed += (_, _) => _closed.SetResult(true);

        var page = await browser.NewPageAsync();

        MainPage mainPage = await page.GoToAsync<MainPage>("https://www.x92g.com/cn");

        VideoDetailPage videoDetailPage = await mainPage.SearchVideo("MIDA-241");

        VideoDetailRecord record = await videoDetailPage.GetVideoDetail();

        await _closed.Task;
    }
}