using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Contrib.Extensions;
using PuppeteerSharp.Contrib.PageObjects;
using PuppeteerSharp.Input;
using VConsole.Util;
using PageExtensions = PuppeteerSharp.Contrib.PageObjects.PageExtensions;

namespace VConsole.PageObject;

public class MainPage : PuppeteerSharp.Contrib.PageObjects.PageObject
{
    [Selector("#adultwarningprompt input[value=\"我同意\"][type=\"button\"]")]
    public virtual Task<IElementHandle?> OkButton { get; }

    [Selector("#adultwarningprompt")] public virtual Task<IElementHandle?> WarningPrompt { get; }

    [Selector("div.menutext a")] public virtual Task<IElementHandle[]> UserMenuActions { get; }

    [Selector("form[name=\"searchbar\"] input[type=\"text\"]")]
    public virtual Task<IElementHandle> SearchInput { get; }

    [Selector("form[name=\"searchbar\"] input[type=\"button\"]")]
    public virtual Task<IElementHandle> SearchButton { get; }

    public async Task<UserPage> GoToUserPage()
    {
        await AutoSkipWarningPrompt();
        if (await IsLogin())
        {
            throw new Exception("page state is no login");
        }

        var userMenuActions = await UserMenuActions;
        await userMenuActions[0].ClickAsync();

        return await Page.WaitForNavigationAsync<UserPage>();
    }

    public async Task<MainPage> AutoSkipWarningPrompt()
    {
        var warningPrompt = await WarningPrompt;
        if (warningPrompt == null) return this;
        var button = await OkButton;
        if (button != null)
        {
            await button.ClickAsync();
        }

        return this;
    }

    public async Task<VideoDetailPage> SearchVideo(string code)
    {
        await (await SearchInput).TypeAsync(code);
        await (await SearchButton).ClickAsync();
        return await Page.WaitForNavigationAsync<VideoDetailPage>();
    }

    public async Task<bool> IsLogin()
    {
        var userMenuActions = await UserMenuActions;
        return userMenuActions.Length == 3;
    }

    public async Task<MainPage> RequireUserLogin()
    {
        if (await IsLogin())
        {
            return this;
        }

        string goBackUrl = Page.Url;
        var userMenuActions = await UserMenuActions;
        await userMenuActions[0].ClickAsync();
        var userLoginPage = await Page.WaitForNavigationAsync<UserLoginPage>();
        await userLoginPage.WaitForUserLogin();
        return await Page.GoToAsync<MainPage>(goBackUrl);
    }
}