using System.Globalization;
using PuppeteerSharp;
using PuppeteerSharp.Contrib.Extensions;
using PuppeteerSharp.Contrib.PageObjects;
using VConsole.Entity;

namespace VConsole.PageObject;

public class VideoDetailPage : PuppeteerSharp.Contrib.PageObjects.PageObject
{
    [Selector("#video_info #video_id td.text")]
    public virtual Task<IElementHandle> VideoIdElement { get; }

    [Selector("#video_info #video_date td.text")]
    public virtual Task<IElementHandle> VideoReleaseDateElement { get; }

    [Selector("#video_info #video_length span.text")]
    public virtual Task<IElementHandle> VideoTimeSpanElement { get; }

    [Selector("#video_info #video_genres span.genre a")]
    public virtual Task<IElementHandle[]> VideoGenresElement { get; }

    [Selector("#video_info #video_cast span.star a")]
    public virtual Task<IElementHandle> VideoActorElement { get; }

    public async Task<VideoDetailRecord> GetVideoDetail()
    {
        return new VideoDetailRecord(
            await (await VideoIdElement).InnerTextAsync(),
            DateOnly.ParseExact(await (await VideoReleaseDateElement).InnerTextAsync(), "yyyy-MM-dd",
                CultureInfo.InvariantCulture
            ),
            await (await VideoTimeSpanElement).InnerTextAsync(),
            (await VideoGenresElement).Select(element => element.InnerTextAsync().Result).ToList(),
            await (await VideoActorElement).InnerTextAsync()
        );
    }
}