using System.Collections.Concurrent;
using Microsoft.Playwright;

namespace Common.Playwright.Browser;

public class PlaywrightPageProvider
{
    private readonly ConcurrentDictionary<string, Task<IPage>> _pages = new();
    private readonly PlaywrightBrowserProvider _browserProvider;

    public PlaywrightPageProvider(PlaywrightBrowserProvider browserProvider)
    {
        _browserProvider = browserProvider;
    }

    public async Task<IPage> GetPageAsync(string identifier)
    {
        return await _pages.GetOrAdd(identifier, async _ =>
        {
            var browserContext = await _browserProvider.GetInstanceAsync(identifier).ConfigureAwait(false);
            var page = await browserContext.NewPageAsync().ConfigureAwait(false);
            return page;
        }).ConfigureAwait(false);
    }
    
    public void DisposeInstance(string identifier)
    {
        _pages.TryRemove(identifier, out _);
    }
}