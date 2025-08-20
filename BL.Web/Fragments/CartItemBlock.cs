using Common.Playwright.PageFragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Fragments;

public class CartItemBlock : PageFragment
{
    public ILocator Title { get; }
    public ILocator Quantity { get; }
    public ILocator Description { get; }
    public ILocator Price { get; }
    public ILocator Remove { get; }

    public CartItemBlock(PlaywrightLifecycleManager lifecycleManager, ILocator root) : base(lifecycleManager, root)
    {
        Title = Locator(".inventory_item_name");
        Quantity = Locator(".cart_quantity");
        Description = Locator(".inventory_item_desc");
        Price = Locator(".inventory_item_price");
        Remove = Locator("button[id^=remove]");
    }

    public async Task<CartProductUiData> GetProductData() =>
        new(await Title.TextContentAsync(),
            await Description.TextContentAsync(),
            await Price.TextContentAsync(),
            await Quantity.TextContentAsync());

    public async Task ClickTitle() => await Title.ClickAsync();
    public async Task ClickRemove() => await Remove.ClickAsync();
}

public record CartProductUiData(string? Title, string? Description, string? Price, string? Quantity);