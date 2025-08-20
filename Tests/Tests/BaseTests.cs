using Common.Playwright.PageObjects;
using NUnit.Framework;
using Tests.Configuration;
using Tests.Configuration.Runner;

namespace Tests.Tests;

public class BaseTests
{
    [SetUp]
    public async Task SetUp()
    {
        NUnitHelper.RegisterContextCreationAttributes(this);
        await ServiceProvider.GetService<NunitPlaywrightReportManager>().StartTraceRecord();
    }

    [TearDown]
    public async Task TearDown()
    {
        var reportManager = ServiceProvider.GetService<NunitPlaywrightReportManager>();

        if (NUnitHelper.IsTestFailed())
        {
            await reportManager.SaveTraceRecord();
            await reportManager.AttachPageScreenshot();
        }
        else
        {
            await reportManager.StopTraceRecord();
        }

        await ServiceProvider.GetService<PlaywrightLifecycleManager>().DisposeInstanceAsync();
    }

    protected async Task<T> OpenPage<T>() where T : PageObject, IOpenPage
    {
        var page = await ServiceProvider.GetService<PlaywrightLifecycleManager>().CreatePageObjectAsync<T>();
        await page.OpenPage();
        return page;
    }
}