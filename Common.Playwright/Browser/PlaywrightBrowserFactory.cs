using Microsoft.Playwright;

namespace Common.Playwright.Browser;

public class PlaywrightBrowserFactory
{
    private readonly PlaywrightConfigurationData _config;

    public PlaywrightBrowserFactory(PlaywrightConfigurationData config)
    {
        _config = config;
    }

    public async Task<IBrowser> Create(IPlaywright playwright)
    {
        return await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = _config.Headless,
            SlowMo = _config.SlowMo
        });
    }
}