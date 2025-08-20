using Common.Playwright.Browser;
using Common.Playwright.PageObjects;
using FluentAssertions;
using Tests.Configuration;

namespace Tests.Verifications;

public static class PageVerifications
{
    public static void VerifyOpenedPageUrl<T>(this T page) where T : PageObject, IOpenPage
    {
        var actual = page.Page.Url.TrimEnd('/');
        var baseUri = new Uri(ServiceProvider.GetService<PlaywrightConfigurationData>().BaseUrl);
        var expected = new Uri(baseUri, page.Url).ToString().TrimEnd('/');

        actual.Should().Be(expected);
    }
}