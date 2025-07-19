using PuppeteerSharp;
using PuppeteerSharp.Contrib.PageObjects;

namespace VConsole.PageObject;

public class UserLoginPage : PuppeteerSharp.Contrib.PageObjects.PageObject
{
    [Selector("#userid")] public virtual Task<IElementHandle> UserIdInput { get; }

    [Selector("#password")] public virtual Task<IElementHandle> PasswordInput { get; }

    public async Task<UserLoginPage> InputUserNameAndPassword(string userName, string password)
    {
        await (await UserIdInput).TypeAsync(userName);
        await (await PasswordInput).TypeAsync(password);
        return this;
    }

    public async Task<UserPage> WaitForUserLogin()
    {
        return await Page.WaitForNavigationAsync<UserPage>(new NavigationOptions
        {
            Timeout = 0
        });
    }
}