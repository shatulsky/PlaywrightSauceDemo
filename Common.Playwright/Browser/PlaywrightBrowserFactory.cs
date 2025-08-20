using Microsoft.Playwright;
using Serilog;

namespace Common.Playwright.Browser;

public class PlaywrightBrowserFactory
{
    private readonly PlaywrightConfigurationData _config;
    private readonly ILogger _logger;

    public PlaywrightBrowserFactory(PlaywrightConfigurationData config, ILogger logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<IBrowser> Create(IPlaywright playwright)
    {
        var browserType = Environment.GetEnvironmentVariable("PlaywrightBrowserName") ?? "chromium";

        _logger.Information("Initializing {BrowserType} playwright browser", browserType);
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