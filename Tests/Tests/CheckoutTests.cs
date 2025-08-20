using BL.Contracts;
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
public class CheckoutTests : BaseTests
{
    private readonly ProductsDataProvider _productsDataProvider = ServiceProvider.GetService<ProductsDataProvider>();

    [Test]
    public async Task Checkout_SingleItem()
    {
        var product = _productsDataProvider.AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.AddProductToCart(product.Title);

        var cartPage = await inventoryPage.Header.OpeCart();
        var checkoutInformationPage = await cartPage.ClickCheckout();
        checkoutInformationPage.VerifyOpenedPageUrl();

        await checkoutInformationPage.FillData("TestFirstName", "TestLastName", "12345");
        var checkoutOverviewPage = await checkoutInformationPage.ClickContinue();
        checkoutOverviewPage.VerifyOpenedPageUrl();

        await Task.WhenAll(
            checkoutOverviewPage.VerifyPaymentInformation("SauceCard #31337"),
            checkoutOverviewPage.VerifyShippingInformation("Free Pony Express Delivery!"),
            checkoutOverviewPage.VerifyItemTotalPrice([product]),
            checkoutOverviewPage.VerifyTax([product]),
            checkoutOverviewPage.VerifyTotal([product]));

        var checkoutCompletePage = await checkoutOverviewPage.ClickFinish();
        checkoutCompletePage.VerifyOpenedPageUrl();
        await checkoutCompletePage.VerifyCheckoutSuccessfulTitleDisplayed();
        
        cartPage = await OpenPage<SauceDemoCartPage>();
        await cartPage.VerifyCartIsEmpty();
    }

    [Test]
    public async Task Checkout_MultipleItems()
    {
        var allProducts = _productsDataProvider.AllProducts;
        var product1 = allProducts.TakeRandom();
        var product2 = allProducts.TakeRandom(p => p != product1);
        ProductData[] products = [product1, product2];
        
        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.AddProductToCart(product1.Title);
        await inventoryPage.AddProductToCart(product2.Title);

        var cartPage = await inventoryPage.Header.OpeCart();
        var checkoutInformationPage = await cartPage.ClickCheckout();

        await checkoutInformationPage.FillData("TestFirstName", "TestLastName", "12345");
        var checkoutOverviewPage = await checkoutInformationPage.ClickContinue();

        await Task.WhenAll(
            checkoutOverviewPage.VerifyPaymentInformation("SauceCard #31337"),
            checkoutOverviewPage.VerifyShippingInformation("Free Pony Express Delivery!"),
            checkoutOverviewPage.VerifyItemTotalPrice(products),
            checkoutOverviewPage.VerifyTax(products),
            checkoutOverviewPage.VerifyTotal(products));

        var checkoutCompletePage = await checkoutOverviewPage.ClickFinish();
        await checkoutCompletePage.VerifyCheckoutSuccessfulTitleDisplayed();
        
        cartPage = await OpenPage<SauceDemoCartPage>();
        await cartPage.VerifyCartIsEmpty();
    }

    [Test]
    public async Task Checkout_InvalidShippingInformation()
    {
        var product = _productsDataProvider.AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.AddProductToCart(product.Title);

        var cartPage = await inventoryPage.Header.OpeCart();
        var checkoutInformationPage = await cartPage.ClickCheckout();
        await checkoutInformationPage.ClickContinue();
        await checkoutInformationPage.VerifyShipmentInformationError("Error: First Name is required");

        await checkoutInformationPage.FirstName.FillAsync("Name");
        await checkoutInformationPage.ClickContinue();
        await checkoutInformationPage.VerifyShipmentInformationError("Error: Last Name is required");

        await checkoutInformationPage.LastName.FillAsync("Surname");
        await checkoutInformationPage.ClickContinue();
        await checkoutInformationPage.VerifyShipmentInformationError("Error: Postal Code is required");

        await checkoutInformationPage.Error.ClickClose();
        await checkoutInformationPage.VerifyShipmentInformationErrorNotVisible();
        
        await checkoutInformationPage.PostalCode.FillAsync("123");
        var checkoutOverviewPage = await checkoutInformationPage.ClickContinue();
        checkoutOverviewPage.VerifyOpenedPageUrl();
    }
}