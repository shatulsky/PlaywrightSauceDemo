using BL.Web.Pages;
using FluentAssertions;

namespace Tests.Verifications;

public static class SauceDemoCommonPageVerifications
{
    public static async Task VerifyCartBadgeNumber(this SauceDemoBasePage page, int expected)
    {
        var actual = await page.Header.ShoppingCartItemsNumber.TextContentAsync();
        actual.Should().Be(expected.ToString());
    }
}