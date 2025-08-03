using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Contrib.PageObjects;

namespace VConsole.PageObject;

public abstract class UserLoginPage : PuppeteerSharp.Contrib.PageObjects.PageObject
{
    [Selector("#userid")] public virtual required Task<IElementHandle> UserIdInput { get; set; }

    [Selector("#password")] public virtual required Task<IElementHandle> PasswordInput { get; set; }

    private static ILogger<UserLoginPage> Logger =>
        HostProvider.HostInstance!.Services.GetRequiredService<ILogger<UserLoginPage>>();

    public async Task<UserLoginPage> InputUserNameAndPassword(string userName, string password)
    {
        await (await UserIdInput).TypeAsync(userName);
        await (await PasswordInput).TypeAsync(password);
        return this;
    }

    public async Task<UserPage> WaitForUserLogin()
    {
        Logger.LogInformation("");
        return await Page.WaitForNavigationAsync<UserPage>(new NavigationOptions
        {
            Timeout = 0
        });
    }
}