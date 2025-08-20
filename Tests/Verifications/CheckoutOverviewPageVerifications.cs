using BL.Contracts;
using BL.Web.Fragments;
using BL.Web.Pages;
using FluentAssertions;

namespace Tests.Verifications;

public static class CheckoutOverviewPageVerifications
{
    public static async Task VerifyItemTotalPrice(this SauceDemoCheckoutOverviewPage page, ProductData[] products)
    {
        var actual = await page.ItemTotal.TextContentAsync();
        var totalPrice = products.Sum(p => p.Price);
        var currency = products.First().Currency;
        var expected = "Item total: " + currency + totalPrice.ToString("F2");
        actual.Should().BeEquivalentTo(expected);
    }

    public static async Task VerifyTax(this SauceDemoCheckoutOverviewPage page, ProductData[] products)
    {
        var actual = await page.Tax.TextContentAsync();
        var totalTax = products.Sum(CalculateTax);
        var currency = products.First().Currency;
        var expected = "Tax: " + currency + totalTax.ToString("F2");
        actual.Should().BeEquivalentTo(expected);
    }

    public static async Task VerifyTotal(this SauceDemoCheckoutOverviewPage page, ProductData[] products)
    {
        var actual = await page.Total.TextContentAsync();
        var totalPrice = products.Sum(p => p.Price);
        var totalTax = products.Sum(CalculateTax);
        var currency = products.First().Currency;
        var expected = "Total: " + currency + Math.Round(totalPrice + totalTax, 2).ToString("F2");
        actual.Should().BeEquivalentTo(expected);
    }

    private static double CalculateTax(ProductData product)
    {
        return Math.Round(product.Price * 0.08, 2);
    }

    public static async Task VerifyShippingInformation(this SauceDemoCheckoutOverviewPage page, string expected)
    {
        var actual = await page.ShippingInformation.TextContentAsync();
        actual.Should().BeEquivalentTo(expected);
    }

    public static async Task VerifyPaymentInformation(this SauceDemoCheckoutOverviewPage page, string expected)
    {
        var actual = await page.PaymentInformation.TextContentAsync();
        actual.Should().BeEquivalentTo(expected);
    }

    public static async Task VerifyProductsData(this SauceDemoCheckoutOverviewPage page, ProductData[] expected)
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