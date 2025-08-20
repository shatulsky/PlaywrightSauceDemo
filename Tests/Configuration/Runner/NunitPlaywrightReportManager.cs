using Common.Infrastructure;
using Common.Playwright.Browser;
using NUnit.Framework;

namespace Tests.Configuration.Runner;

public class NunitPlaywrightReportManager
{
    private readonly PlaywrightBrowserProvider _browserProvider;
    private readonly IScopeIdentifier _scopeIdentifier;
    private readonly PlaywrightPageProvider _playwrightPageProvider;
    private readonly string _reportBasePath;

    public NunitPlaywrightReportManager(PlaywrightBrowserProvider browserProvider,
        IScopeIdentifier scopeIdentifier,
        PlaywrightConfigurationData playwrightConfiguration,
        PlaywrightPageProvider playwrightPageProvider)
    {
        _browserProvider = browserProvider;
        _scopeIdentifier = scopeIdentifier;
        _playwrightPageProvider = playwrightPageProvider;
        _reportBasePath = Path.Combine(PathHelper.GetAssemblyPath(), playwrightConfiguration.TraceReportDirectory);
    }

    public async Task StartTraceRecord()
    {
        var identifier = _scopeIdentifier.Identifier;
        var context = await _browserProvider.GetInstanceAsync(identifier);

        await context.Tracing.StartAsync(new()
        {
            Title = GetTestName(),
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    public async Task SaveTraceRecord()
    {
        var identifier = _scopeIdentifier.Identifier;
        var context = await _browserProvider.GetInstanceAsync(identifier);

        Directory.CreateDirectory(_reportBasePath);
        var reportPath = Path.Combine(_reportBasePath, $"Report_{GetTestName()}.zip");

        await context.Tracing.StopAsync(new()
        {
            Path = reportPath
        });

        TestContext.AddTestAttachment(reportPath);
    }

    public async Task StopTraceRecord()
    {
        var identifier = _scopeIdentifier.Identifier;
        var context = await _browserProvider.GetInstanceAsync(identifier);
        await context.Tracing.StopAsync();
    }

    public async Task AttachPageScreenshot()
    {
        var identifier = _scopeIdentifier.Identifier;
        var page = await _playwrightPageProvider.GetPageAsync(identifier);
        var screenshotPath = Path.Combine(_reportBasePath, $"Screenshot_{GetTestName()}.png");
        await page.ScreenshotAsync(new()
        {
            Path = screenshotPath
        });
        
        TestContext.AddTestAttachment(screenshotPath);
    }

    private static string GetTestName() => $"{TestContext.CurrentContext.Test.FullName}";
}