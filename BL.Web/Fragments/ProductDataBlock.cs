using Common.Playwright.PageFragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Fragments;

public class ProductDataBlock : PageFragment
{
    public ILocator Title { get; }
    public ILocator Description { get; }
    public ILocator Price { get; }
    public ILocator Image { get; }
    public ILocator AddToCart { get; }
    public ILocator RemoveFromCart { get; }

    public ProductDataBlock(PlaywrightLifecycleManager lifecycleManager, ILocator root) : base(lifecycleManager, root)
    {
        Title = Locator(".inventory_item_name");
        Description = Locator(".inventory_item_desc");
        Price = Locator(".inventory_item_price");
        Image = Locator("img.inventory_item_img");
        AddToCart = Locator("button[id^=add-to-cart]");
        RemoveFromCart = Locator("button[id^=remove]");
    }

    public async Task<ProductUiData> GetProductData() =>
        new(await Title.TextContentAsync(), await Description.TextContentAsync(), await Price.TextContentAsync(), await Image.GetAttributeAsync("src"));

    public async Task ClickImage() => await Image.ClickAsync();
    public async Task ClickTitle() => await Image.ClickAsync();
    public async Task ClickAddToCard() => await AddToCart.ClickAsync();
    public async Task ClickRemoveFromCart() => await RemoveFromCart.ClickAsync();
}

public record ProductUiData(string? Title, string? Description, string? Price, string? Image);