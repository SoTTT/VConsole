using PuppeteerSharp;

namespace VConsole.Util;

public static class PageExtensions
{
    public static Task ClickAndWaitForNavigation(this IPage page, IElementHandle clickableElementHandle)
    {
        return Task.WhenAll(clickableElementHandle.ClickAsync(),
            page.WaitForNavigationAsync(new NavigationOptions
                {
                    WaitUntil = [WaitUntilNavigation.Networkidle0],
                    Timeout = 30000 // 延长超时应对慢网络
                }
            ));
    }

    // public static async Task<T> ClickAndWaitForNavigation<T>(this IPage page, IElementHandle clickableElementHandle)
    //     where T : PuppeteerSharp.Contrib.PageObjects.PageObject
    // {
    //     Task task = PuppeteerSharp.Contrib.PageObjects.PageExtensions.WaitForNavigationAsync<T>(page,
    //         new NavigationOptions
    //         {
    //             WaitUntil = [WaitUntilNavigation.Networkidle0],
    //             Timeout = 30000 // 延长超时应对慢网络
    //         });
    //     return await Task<T>.WhenAll(clickableElementHandle.ClickAsync(), task);
    // }
}