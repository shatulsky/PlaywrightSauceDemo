using BL.Web.Pages;
using Common.Playwright.PageFragments;
using FluentAssertions;

namespace Tests.Verifications;

public static class CheckoutInformationPageVerifications
{
    public static async Task VerifyShipmentInformationError(this SauceDemoCheckoutInformationPage page, string expected)
    {
        var actual = await page.Error.GetText();
        actual.Should().Be(expected);
    }

    public static async Task VerifyShipmentInformationErrorNotVisible(this SauceDemoCheckoutInformationPage page)
    {
        await page.Error.Text.Expect().ToBeHiddenAsync();
        await page.Error.Close.Expect().ToBeHiddenAsync();
    }
}