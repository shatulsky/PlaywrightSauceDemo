using BL.Contracts;
using BL.Web.Fragments;
using BL.Web.Pages;
using Common.Playwright.Browser;
using FluentAssertions;
using Tests.Configuration;

namespace Tests.Verifications;

public static class ProductPageVerifications
{
    public static async Task VerifyProductDetails(this SauceDemoProductPage page, ProductData expected)
    {
        var actualProduct = await page.GetProductDetails();
        var expectedProduct = ConvertProduct(expected);

        actualProduct.Should().BeEquivalentTo(expectedProduct);
    }

    private static ProductUiData ConvertProduct(ProductData p)
    {
        return new ProductUiData(p.Title, p.Description, p.Currency + p.Price.ToString("F2"), p.Image);
    }

    public static void VerifyProductPageUrl(this SauceDemoProductPage page, int expectedId)
    {
        var actual = page.Page.Url.TrimEnd('/');
        var baseUri = new Uri(ServiceProvider.GetService<PlaywrightConfigurationData>().BaseUrl);
        var expected = new Uri(baseUri, page.Url).ToString().TrimEnd('/') + "?id=" + expectedId;

        actual.Should().Be(expected);
    }
}