using BL.Web.Fragments;
using Common.Infrastructure;
using Common.Playwright.PageFragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Pages;

public class SauceDemoInventoryPage : SauceDemoBasePage, IOpenPage
{
    public string Url => "inventory.html";
    public ILocator SortingSelect { get; }
    public ILocator Products { get; }


    public SauceDemoInventoryPage(PlaywrightLifecycleManager lifecycleManager, IPage page) : base(lifecycleManager, page)
    {
        Products = Locator(".inventory_item");
        SortingSelect = Locator(".product_sort_container");
    }

    public async Task<ProductUiData[]> GetProductsData()
    {
        Logger.Information("Getting all inventory products data");
        var products = await GetProductCards();
        var data = await Task.WhenAll(products.Select(p => p.GetProductData()));
        Logger.Information("Products data - {Product}", data.ToBeautifiedJsonString());

        return data;
    }

    public async Task<SauceDemoProductPage> ClickProductTitle(string title)
    {
        Logger.Information("Clicking product title - {Title}", title);
        await FindProductWithTitle(title).ClickTitle();

        return await NewPage<SauceDemoProductPage>();
    }

    public async Task<SauceDemoProductPage> ClickProductImage(string title)
    {
        Logger.Information("Clicking product title - {Title}", title);

        await FindProductWithTitle(title).ClickImage();

        return await NewPage<SauceDemoProductPage>();
    }

    private async Task<IReadOnlyList<ProductDataBlock>> GetProductCards()
    {
        var locators = await Products.WaitForAnyAsync();
        return locators.ToFragment<ProductDataBlock>(LifecycleManager);
    }

    public async Task<string?> GetSelectedSortingOrder()
    {
        var selectedOption = await SortingSelect.EvaluateAsync<string>("select => select.options[select.selectedIndex].textContent");
        Logger.Information("Selected sorting order is - {Option}", selectedOption);
        return selectedOption;
    }

    public async Task SelectSortingOrder(string sortingOrder)
    {
        Logger.Information("Selecting sorting order - {SortingOrder}", sortingOrder);
        await SortingSelect.SelectOptionAsync(sortingOrder);
    }

    public async Task AddProductToCart(string title)
    {
        Logger.Information("Adding product to cart - {Title}", title);
        await FindProductWithTitle(title).ClickAddToCard();
    }

    public async Task RemoveProductFromCart(string title)
    {
        Logger.Information("Removing product from cart - {Title}", title);
        await FindProductWithTitle(title).ClickRemoveFromCart();
    }

    private ProductDataBlock FindProductWithTitle(string title)
    {
        return Products
            .Filter(new() { HasText = title })
            .ToFragment<ProductDataBlock>(LifecycleManager);
    }
}