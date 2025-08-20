using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Pages;

public class SauceDemoCheckoutCompletePage : SauceDemoBasePage, IOpenPage
{
    public string Url => "checkout-complete.html";

    public ILocator CompleteHeader { get; }

    public SauceDemoCheckoutCompletePage(PlaywrightLifecycleManager lifecycleManager, IPage page) : base(lifecycleManager, page)
    {
        CompleteHeader = Locator(".complete-header");
    }
}