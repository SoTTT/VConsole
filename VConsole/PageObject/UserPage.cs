using PuppeteerSharp;
using PuppeteerSharp.Contrib.PageObjects;

namespace VConsole.PageObject;

public class UserPage : PuppeteerSharp.Contrib.PageObjects.PageObject
{
    [Selector("#leftmenu div.menul1 li a")]
    public virtual Task<IElementHandle[]> LeftMenuAction { get; set; }

    public async Task<UserWantToPage> GoToUserWantToPage()
    {
        var elementHandle = Array.Find(await LeftMenuAction, handle =>
            handle.GetPropertyAsync("innerText").Result.JsonValueAsync<string>().Result.Equals("想要的"));
        if (elementHandle is null)
        {
            throw new Exception("没有找到\"想要的\"按钮");
        }

        await elementHandle.ClickAsync();

        return await Page.WaitForNavigationAsync<UserWantToPage>();
    }
}