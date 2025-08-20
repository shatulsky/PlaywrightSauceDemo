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
        var browserType = Environment.GetEnvironmentVariable("PlaywrightBrowserName") ?? "chromium";

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = _config.Headless,
            SlowMo = _config.SlowMo
        };

        return browserType.ToLowerInvariant() switch
        {
            "chromium" => await playwright.Chromium.LaunchAsync(launchOptions),
            "firefox" => await playwright.Firefox.LaunchAsync(launchOptions),
            "webkit" => await playwright.Webkit.LaunchAsync(launchOptions),
            _ => throw new ArgumentException($"Unsupported browser type: {browserType}")
        };
    }
}