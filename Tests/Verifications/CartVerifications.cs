using BL.Contracts;
using BL.Web.Fragments;
using BL.Web.Pages;
using Common.Playwright.PageFragments;
using FluentAssertions;

namespace Tests.Verifications;

public static class CartVerifications
{
    public static async Task VerifyCartIsEmpty(this SauceDemoCartPage page)
    {
        await page.CartItems.Expect().Not.ToBeVisibleAsync();
    }

    public static async Task VerifyCartItemsData(this SauceDemoCartPage page, ProductData[] expected)
    {
        var actualProducts = await page.GetProductsData();
        var expectedProducts = expected.Select(ConvertProduct).ToArray();

        actualProducts.Should().BeEquivalentTo(expectedProducts);
    }

    private static CartProductUiData ConvertProduct(ProductData p)
    {
        return new CartProductUiData(p.Title, p.Description, p.Currency + p.Price.ToString("F2"), "1");
    }
}