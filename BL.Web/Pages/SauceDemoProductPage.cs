using BL.Web.Fragments;
using Common.Infrastructure;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Pages;

public class SauceDemoProductPage : SauceDemoBasePage, IOpenPage
{
    public string Url => "inventory-item.html";

    public ILocator Title { get; }
    public ILocator Description { get; }
    public ILocator Price { get; }
    public ILocator Image { get; }
    public ILocator AddToCart { get; }
    public ILocator RemoveFromCart { get; }
    public ILocator BackButton { get; }

    public SauceDemoProductPage(PlaywrightLifecycleManager lifecycleManager, IPage page) : base(lifecycleManager, page)
    {
        Title = Locator(".inventory_details_name");
        Description = Locator(".inventory_details_desc");
        Price = Locator(".inventory_details_price");
        Image = Locator(".inventory_details_img");
        AddToCart = Locator("button[id^=add-to-cart]");
        RemoveFromCart = Locator("button[id^=remove]");
        BackButton = Locator(".inventory_details_back_button");
    }

    public async Task<ProductUiData> GetProductDetails()
    {
        Logger.Information("Getting product data");
        var data = new ProductUiData(await Title.TextContentAsync(), await Description.TextContentAsync(), await Price.TextContentAsync(), await Image.GetAttributeAsync("src"));
        Logger.Information($"Product data is - {data.ToBeautifiedJsonString()}");
        return data;
    }

    public async Task ClickAddToCart()
    {
        Logger.Information("Clicking 'Add to Cart' button");
        await AddToCart.ClickAsync();
    }

    public async Task ClickRemoveFromCart()
    {
        Logger.Information("Clicking 'Remove from Cart' button");
        await RemoveFromCart.ClickAsync();
    }


    public async Task ClickBackToProducts()
    {
        Logger.Information("Clicking back to products button");
        await BackButton.ClickAsync();
    }
}