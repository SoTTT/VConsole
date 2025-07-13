using PuppeteerSharp;
using PuppeteerSharp.Contrib.Extensions;
using PuppeteerSharp.Contrib.PageObjects;

namespace VConsole.PageObject;

public abstract class UserWantToPage : PuppeteerSharp.Contrib.PageObjects.PageObject
{
    [Selector("div.page_selector a[class=\"page\"")]
    public virtual Task<IElementHandle[]> PageSelectors { get; }

    [Selector("div.page_selector span[class=\"page current\"]")]
    public virtual Task<IElementHandle?> CurrentPageSelector { get; }

    [Selector("table.videotextlist tbody tr td.title a")]
    public virtual Task<IElementHandle[]> WantToVideoList { get; }

    public IElementHandle[] AllPageSelectors
    {
        get
        {
            IElementHandle[] allPageSelector = PageSelectors.Result.Append(CurrentPageSelector.Result).ToArray()!;
            Array.Sort(allPageSelector, (handle, handle1) => Comparer<int>.Default.Compare(
                int.Parse(handle.GetPropertyAsync("innerText").Result.JsonValueAsync<string>().Result),
                int.Parse(handle1.GetPropertyAsync("innerText").Result.JsonValueAsync<string>().Result)
            ));

            return allPageSelector;
        }
    }

    public int[] AllPageSelectorsIndex
    {
        get
        {
            return Array.ConvertAll(AllPageSelectors,
                handle => int.Parse(handle.GetPropertyAsync("innerText").Result.JsonValueAsync<string>().Result));
        }
    }

    public int CurrentPageSelectorIndex =>
        int.Parse(CurrentPageSelector.Result!.GetPropertyAsync("innerText").Result.JsonValueAsync<string>().Result);

    public IElementHandle GetPageSelector(int index)
    {
        return AllPageSelectors[index - 1];
    }

    public async Task<UserWantToPage> GoToUserWantToPage(int index)
    {
        IElementHandle handle = GetPageSelector(index);
        if (CurrentPageSelectorIndex == index)
        {
            return this;
        }

        await handle.ClickAsync();
        return await Page.WaitForNavigationAsync<UserWantToPage>();
    }

    public async Task<Dictionary<string, string>> GetUserWantToList()
    {
        IElementHandle[] handles = await WantToVideoList;
        var result = new Dictionary<string, string>();

        foreach (IElementHandle elementHandle in handles)
        {
            result[elementHandle.GetPropertyAsync("innerText").Result.JsonValueAsync<string>().Result]
                = elementHandle.GetPropertyAsync("href").Result.JsonValueAsync<string>().Result;
        }

        return result;
    }
}