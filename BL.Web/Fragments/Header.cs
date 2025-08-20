using BL.Web.Pages;
using Common.Playwright.PageFragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Fragments;

public class Header : PageFragment
{
    public ILocator ShoppingCart { get; }
    public ILocator ShoppingCartItemsNumber { get; }

    public Header(PlaywrightLifecycleManager lifecycleManager, ILocator root) : base(lifecycleManager, root)
    {
        ShoppingCart = Locator(".shopping_cart_link");
        ShoppingCartItemsNumber = Locator(".shopping_cart_badge");
    }

    public async Task<SauceDemoCartPage> OpeCart()
    {
        Logger.Information("Clicking header shopping cart icon");
        await ShoppingCart.ClickAsync();
        return await NewPage<SauceDemoCartPage>();
    }
}