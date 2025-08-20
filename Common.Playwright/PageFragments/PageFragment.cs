using Common.Playwright.PageObjects;
using Microsoft.Playwright;
using Serilog;

namespace Common.Playwright.PageFragments;

public class PageFragment
{
    protected readonly PlaywrightLifecycleManager LifecycleManager;
    public ILocator Root { get; }
    public ILogger Logger { get; }

    public PageFragment(PlaywrightLifecycleManager lifecycleManager, ILocator root)
    {
        LifecycleManager = lifecycleManager;
        Root = root;
        Logger = lifecycleManager.GetService<ILogger>();
    }

    protected async Task<T> NewPage<T>() where T : PageObject => await LifecycleManager.CreatePageObjectAsync<T>();
    public ILocator Locator(string selector) => Root.Locator(selector);
    public T LocatorFragment<T>(string selector) where T : PageFragment => Root.LocatorFragment<T>(selector, LifecycleManager);
    public ILocatorAssertions Expect() => Assertions.Expect(Root);
}