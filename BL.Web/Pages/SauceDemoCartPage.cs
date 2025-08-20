using BL.Web.Fragments;
using Common.Infrastructure;
using Common.Playwright.PageFragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Pages;

public class SauceDemoCartPage : SauceDemoBasePage, IOpenPage
{
    public string Url => "cart.html";

    public ILocator CartItems { get; }
    public ILocator Checkout { get; }

    public SauceDemoCartPage(PlaywrightLifecycleManager lifecycleManager, IPage page) : base(lifecycleManager, page)
    {
        CartItems = Locator(".cart_item");
        Checkout = Locator("#checkout");
    }

    public async Task<CartProductUiData[]> GetProductsData()
    {
        Logger.Information("Getting all cart items data");
        var products = await GetCartItems();
        var data = await Task.WhenAll(products.Select(p => p.GetProductData()));
        Logger.Information("Cart items data - {Product}", data.ToBeautifiedJsonString());

        return data;
    }

    public async Task RemoveProduct(string title)
    {
        Logger.Information("Removing product with title: {Title}", title);
        await FindProductWithTitle(title).ClickRemove();
    }

    private async Task<IReadOnlyList<CartItemBlock>> GetCartItems()
    {
        var locators = await CartItems.WaitForAnyAsync();
        return locators.ToFragment<CartItemBlock>(LifecycleManager);
    }

    private CartItemBlock FindProductWithTitle(string title)
    {
        return CartItems
            .Filter(new() { HasText = title })
            .ToFragment<CartItemBlock>(LifecycleManager);
    }

    public async Task<SauceDemoCheckoutInformationPage> ClickCheckout()
    {
        Logger.Information("Clicking on the checkout button");
        await Checkout.ClickAsync();
        return await NewPage<SauceDemoCheckoutInformationPage>();
    }
}