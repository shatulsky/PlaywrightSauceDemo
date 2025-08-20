using BL.Contracts;
using BL.Web.Data;
using BL.Web.Fragments;
using BL.Web.Pages;
using Common.Infrastructure;
using Common.Playwright.PageFragments;
using FluentAssertions;
using Tests.Configuration;
using Tests.Tests;

namespace Tests.Verifications;

public static class InventoryPageVerifications
{
    public static async Task VerifyProductsAreSorter(this SauceDemoInventoryPage page, EProductsSortOrder order)
    {
        var actualData = await page.GetProductsData();
        var allProducts = ServiceProvider.GetService<ProductsDataProvider>().AllProducts;
        var expectedSorted = (order switch
        {
            EProductsSortOrder.NameAscending => allProducts.OrderBy(p => p.Title),
            EProductsSortOrder.NameDescending => allProducts.OrderByDescending(p => p.Title),
            EProductsSortOrder.PriceAscending => allProducts.OrderBy(p => p.Price),
            EProductsSortOrder.PriceDescending => allProducts.OrderByDescending(p => p.Price),
            _ => throw new ArgumentOutOfRangeException(nameof(order), order, null)
        }).Select(ConvertProduct).ToArray();

        page.Logger.Debug("Expected sorted products data - {Products}", expectedSorted.ToBeautifiedJsonString());

        actualData.Should().BeEquivalentTo(expectedSorted, o => o.WithoutStrictOrderingFor(p => p));
    }

    public static async Task VerifySelectedSortingOrder(this SauceDemoInventoryPage page, string expected)
    {
        var actual = await page.GetSelectedSortingOrder();
        actual.Should().Be(expected);
    }

    public static async Task VerifyDisplayedProductData(this SauceDemoInventoryPage page, ProductData[] expected)
    {
        var actualProducts = await page.GetProductsData();
        var expectedProducts = expected.Select(ConvertProduct).ToArray();

        actualProducts.Should().BeEquivalentTo(expectedProducts);
    }

    private static ProductUiData ConvertProduct(ProductData p)
    {
        return new ProductUiData(p.Title, p.Description, p.Currency + p.Price.ToString("F2"), p.Image);
    }

    public static async Task VerifyProductsAreLoaded(this SauceDemoInventoryPage page)
    {
        await page.Products.WaitForAnyAsync();
    }
}