using Microsoft.Playwright;

namespace Common.Playwright.Browser.Context;

public class PlaywrightContextFactory
{
    private readonly PlaywrightConfigurationData _config;

    public PlaywrightContextFactory(PlaywrightConfigurationData config)
    {
        _config = config;
    }

    public async Task<IBrowserContext> Create(IBrowser browser, BrowserContextCreationStrategy? creationStrategy)
    {
        creationStrategy?.BeforeContextCreation();

        var filePath = creationStrategy?.GetContextFilePath();
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            filePath = null;
        }
        
        var browserContext = await browser.NewContextAsync(new BrowserNewContextOptions()
        {
            ViewportSize = new ViewportSize
            {
                Width = 1920,
                Height = 1080
            },
            BaseURL = _config.BaseUrl,
            StorageStatePath = filePath 
        });

        if (creationStrategy != null)
        {
            await creationStrategy.AfterContextCreation(browserContext);
        }

        return browserContext;
    }
}