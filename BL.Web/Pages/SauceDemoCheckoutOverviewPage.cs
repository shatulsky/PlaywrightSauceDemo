using BL.Web.Fragments;
using Common.Infrastructure;
using Common.Playwright.PageFragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Pages;

public class SauceDemoCheckoutOverviewPage : SauceDemoBasePage, IOpenPage
{
    public string Url => "checkout-step-two.html";

    public ILocator CartItems { get; }
    public ILocator PaymentInformation { get; }
    public ILocator ShippingInformation { get; }
    public ILocator ItemTotal { get; }
    public ILocator Tax { get; }
    public ILocator Total { get; }
    public ILocator Finish { get; }

    public SauceDemoCheckoutOverviewPage(PlaywrightLifecycleManager lifecycleManager, IPage page) : base(lifecycleManager, page)
    {
        CartItems = Locator(".cart_item");
        PaymentInformation = Locator("data-test=payment-info-value");
        ShippingInformation = Locator("data-test=shipping-info-value");
        ItemTotal = Locator("data-test=subtotal-label");
        Tax = Locator("data-test=tax-label");
        Total = Locator("data-test=total-label");
        Finish = Locator("#finish");
    }

    public async Task<CartProductUiData[]> GetProductsData()
    {
        Logger.Information("Getting all cart items data");
        var products = await GetCartItems();
        var data = await Task.WhenAll(products.Select(p => p.GetProductData()));
        Logger.Information("Cart items data - {Product}", data.ToBeautifiedJsonString());
        return data;
    }

    private async Task<IReadOnlyList<CartItemBlock>> GetCartItems()
    {
        var locators = await CartItems.WaitForAnyAsync();
        return locators.ToFragment<CartItemBlock>(LifecycleManager);
    }

    public async Task<SauceDemoCheckoutCompletePage> ClickFinish()
    {
        Logger.Information("Clicking the Finish button");
        await Finish.ClickAsync();
        return await NewPage<SauceDemoCheckoutCompletePage>();
    }
}