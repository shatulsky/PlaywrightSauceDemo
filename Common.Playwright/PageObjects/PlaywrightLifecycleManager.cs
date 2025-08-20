using Common.Infrastructure;
using Common.Playwright.Browser;
using Common.Playwright.Browser.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Playwright.PageObjects;

public class PlaywrightLifecycleManager
{
    private readonly PlaywrightBrowserProvider _browserProvider;
    private readonly PageObjectsFactory _pageObjectsFactory;
    private readonly PlaywrightPageProvider _pageProvider;
    private readonly ContextCreationStrategyStorage _strategyStorage;
    private readonly IScopeIdentifier _scopeIdentifier;
    private readonly IServiceProvider _serviceProvider;

    public PlaywrightLifecycleManager(PlaywrightBrowserProvider browserProvider,
        PageObjectsFactory pageObjectsFactory,
        PlaywrightPageProvider pageProvider,
        ContextCreationStrategyStorage strategyStorage,
        IScopeIdentifier scopeIdentifier,
        IServiceProvider serviceProvider)
    {
        _browserProvider = browserProvider;
        _pageObjectsFactory = pageObjectsFactory;
        _pageProvider = pageProvider;
        _strategyStorage = strategyStorage;
        _scopeIdentifier = scopeIdentifier;
        _serviceProvider = serviceProvider;
    }

    public async Task<PageObject> CreatePageObjectAsync(Type pageType)
    {
        var identifier = _scopeIdentifier.Identifier;
        var page = await _pageProvider.GetPageAsync(identifier).ConfigureAwait(false);
        return _pageObjectsFactory.CreatePage(pageType, this, page);
    }

    public async Task<T> CreatePageObjectAsync<T>() where T : PageObject
    {
        var identifier = _scopeIdentifier.Identifier;
        var page = await _pageProvider.GetPageAsync(identifier).ConfigureAwait(false);
        return (T)_pageObjectsFactory.CreatePage(typeof(T), this, page);
    }

    public T GetService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();

    public async Task DisposeInstanceAsync()
    {
        var identifier = _scopeIdentifier.Identifier;
        _pageProvider.DisposeInstance(identifier);
        await _browserProvider.DisposeInstanceAsync(identifier);
        _strategyStorage.CleanInstanceStrategy(identifier);
    }
}