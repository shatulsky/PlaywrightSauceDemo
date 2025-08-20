using Common.Infrastructure;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace Common.Playwright.Browser.Context;

public abstract class BrowserContextCreationStrategy
{
    private readonly PageObjectsFactory _pageObjectsFactory;
    private readonly PlaywrightLifecycleManager _lifecycleManager;
    private readonly PlaywrightConfigurationData _playwrightConfiguration;

    protected BrowserContextCreationStrategy(PageObjectsFactory pageObjectsFactory,
        PlaywrightConfigurationData playwrightConfiguration,
        PlaywrightLifecycleManager lifecycleManager)
    {
        _pageObjectsFactory = pageObjectsFactory;
        _playwrightConfiguration = playwrightConfiguration;
        _lifecycleManager = lifecycleManager;
    }

    protected abstract string FileName { get; }

    public virtual void BeforeContextCreation()
    {
    }

    public virtual Task AfterContextCreation(IBrowserContext context)
    {
        return Task.CompletedTask;
    }

    public string GetContextFilePath()
    {
        return Path.Combine(PathHelper.GetAssemblyPath(), _playwrightConfiguration.StorageStateDirectory, $"{FileName}.json");
    }

    protected T CreatePageObject<T>(IPage page) where T : PageObject => (T)_pageObjectsFactory.CreatePage(typeof(T), _lifecycleManager, page);
}