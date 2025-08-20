using Common.Playwright.PageObjects;
using Microsoft.Playwright;
using Header = BL.Web.Fragments.Header;

namespace BL.Web.Pages;

public class SauceDemoBasePage : PageObject
{
    public Header Header { get; }

    public SauceDemoBasePage(PlaywrightLifecycleManager lifecycleManager, IPage page) : base(lifecycleManager, page)
    {
        Header = LocatorFragment<Header>(".primary_header");
    }
}