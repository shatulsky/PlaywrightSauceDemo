using BL.Web.Context;
using BL.Web.Data;
using BL.Web.Pages;
using Common.Infrastructure;
using Common.Playwright.Browser.Context;
using NUnit.Framework;
using Tests.Configuration;
using Tests.Verifications;

namespace Tests.Tests;

[BrowserContext<StandardUser>]
public class ProductTests : BaseTests
{
    [Test]
    public async Task DisplayProducts()
    {
        var products = ServiceProvider.GetService<ProductsDataProvider>().AllProducts;

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        inventoryPage.VerifyOpenedPageUrl();

        await inventoryPage.VerifyDisplayedProductData(products);
    }

    /// <summary>
    /// Using an alternative method of authorization by utilizing the UI authentication flow with <see cref="StandardUserUiLogin"/>
    /// This just serves as an example to show that UI-based auth is a viable approach
    /// </summary>
    [Test]
    [BrowserContext<StandardUserUiLogin>]
    public async Task OpenProductDetails_ClickTitle()
    {
        var product = ServiceProvider.GetService<ProductsDataProvider>().AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        var itemPage = await inventoryPage.ClickProductTitle(product.Title);
        itemPage.VerifyProductPageUrl(product.ID);
        await itemPage.VerifyProductDetails(product);
    }

    [Test]
    public async Task OpenProductDetails_ClickImage()
    {
        var product = ServiceProvider.GetService<ProductsDataProvider>().AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        var itemPage = await inventoryPage.ClickProductTitle(product.Title);
        itemPage.VerifyProductPageUrl(product.ID);
        await itemPage.VerifyProductDetails(product);
    }

    [Test]
    public async Task Sorting_Default()
    {
        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.VerifySelectedSortingOrder("Name (A to Z)");
        await inventoryPage.VerifyProductsAreSorter(EProductsSortOrder.NameAscending);
    }

    [Test]
    public async Task Sorting_Name()
    {
        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();

        await inventoryPage.SelectSortingOrder(GetSortOrderText(EProductsSortOrder.NameDescending));
        await inventoryPage.VerifyProductsAreSorter(EProductsSortOrder.NameDescending);

        await inventoryPage.SelectSortingOrder(GetSortOrderText(EProductsSortOrder.NameAscending));
        await inventoryPage.VerifyProductsAreSorter(EProductsSortOrder.NameAscending);
    }

    [Test]
    public async Task Sorting_Name_Reversed() => await VerifyProductSorting(EProductsSortOrder.NameDescending);

    [Test]
    public async Task Sorting_Price_LowToHigh() => await VerifyProductSorting(EProductsSortOrder.PriceAscending);

    [Test]
    public async Task Sorting_Price_HighToLow() => await VerifyProductSorting(EProductsSortOrder.PriceDescending);

    private async Task VerifyProductSorting(EProductsSortOrder sortOrder)
    {
        var sortingOrderText = GetSortOrderText(sortOrder);

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.SelectSortingOrder(sortingOrderText);
        await inventoryPage.VerifySelectedSortingOrder(sortingOrderText);
        await inventoryPage.VerifyProductsAreSorter(sortOrder);
    }

    private static string GetSortOrderText(EProductsSortOrder sortOrder) => sortOrder switch
    {
        EProductsSortOrder.NameAscending => "Name (A to Z)",
        EProductsSortOrder.NameDescending => "Name (Z to A)",
        EProductsSortOrder.PriceAscending => "Price (low to high)",
        EProductsSortOrder.PriceDescending => "Price (high to low)",
        _ => throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, null)
    };
}

public enum EProductsSortOrder
{
    NameAscending,
    NameDescending,
    PriceAscending,
    PriceDescending
}