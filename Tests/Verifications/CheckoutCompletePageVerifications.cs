using BL.Web.Pages;
using Common.Playwright.PageFragments;

namespace Tests.Verifications;

public static class CheckoutCompletePageVerifications
{
    public static async Task VerifyCheckoutSuccessfulTitleDisplayed(this SauceDemoCheckoutCompletePage page)
    {
        await page.CompleteHeader.Expect().ToBeVisibleAsync();
    }
}