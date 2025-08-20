using Common.Playwright.PageFragments;
using Microsoft.Playwright;
using Serilog;

namespace Common.Playwright.PageObjects;

public abstract class PageObject
{
    protected readonly PlaywrightLifecycleManager LifecycleManager;
    public IPage Page { get; }
    public ILogger Logger { get; }

    protected PageObject(PlaywrightLifecycleManager lifecycleManager, IPage page)
    {
        LifecycleManager = lifecycleManager;
        Page = page;
        Logger = lifecycleManager.GetService<ILogger>();
    }

    protected async Task<T> NewPage<T>() where T : PageObject => await LifecycleManager.CreatePageObjectAsync<T>();
    protected ILocator Locator(string selector) => Page.Locator(selector);
    protected T LocatorFragment<T>(string selector) where T : PageFragment => Page.LocatorFragment<T>(selector, LifecycleManager);
}