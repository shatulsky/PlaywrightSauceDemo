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
public class CartTests : BaseTests
{
    private readonly ProductsDataProvider _productsDataProvider = ServiceProvider.GetService<ProductsDataProvider>();

    [Test]
    public async Task AddToCart_SingleItem_FromInventory()
    {
        var product = _productsDataProvider.AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.AddProductToCart(product.Title);
        await inventoryPage.VerifyCartBadgeNumber(1);

        var cartPage = await inventoryPage.Header.OpeCart();
        cartPage.VerifyOpenedPageUrl();
        await cartPage.VerifyCartItemsData([product]);
    }

    [Test]
    public async Task AddToCart_SingleItem_FromProduct()
    {
        var product = _productsDataProvider.AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        var productPage = await inventoryPage.ClickProductTitle(product.Title);

        await productPage.ClickAddToCart();
        await productPage.VerifyCartBadgeNumber(1);

        var cartPage = await productPage.Header.OpeCart();
        cartPage.VerifyOpenedPageUrl();
        await cartPage.VerifyCartItemsData([product]);
    }

    [Test]
    public async Task AddToCart_MultipleItems_FromInventory()
    {
        var allProducts = _productsDataProvider.AllProducts;
        var product1 = allProducts.TakeRandom();
        var product2 = allProducts.TakeRandom(p => p != product1);
        var product3 = allProducts.TakeRandom(p => p != product1 && p != product2);

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.AddProductToCart(product1.Title);
        await inventoryPage.VerifyCartBadgeNumber(1);

        await inventoryPage.AddProductToCart(product2.Title);
        await inventoryPage.VerifyCartBadgeNumber(2);

        await inventoryPage.AddProductToCart(product3.Title);
        await inventoryPage.VerifyCartBadgeNumber(3);

        var cartPage = await inventoryPage.Header.OpeCart();
        cartPage.VerifyOpenedPageUrl();
        await cartPage.VerifyCartItemsData([product1, product2, product3]);
    }

    [Test]
    public async Task AddToCart_MultipleItems_FromProduct()
    {
        var allProducts = _productsDataProvider.AllProducts;
        var product1 = allProducts.TakeRandom();
        var product2 = allProducts.TakeRandom(p => p != product1);
        var product3 = allProducts.TakeRandom(p => p != product1 && p != product2);

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        var productPage = await inventoryPage.ClickProductTitle(product1.Title);
        await productPage.ClickAddToCart();
        await productPage.VerifyCartBadgeNumber(1);
        await productPage.ClickBackToProducts();

        await inventoryPage.ClickProductTitle(product2.Title);
        await productPage.ClickAddToCart();
        await productPage.VerifyCartBadgeNumber(2);
        await productPage.ClickBackToProducts();

        await inventoryPage.ClickProductTitle(product3.Title);
        await productPage.ClickAddToCart();
        await productPage.VerifyCartBadgeNumber(3);

        var cartPage = await productPage.Header.OpeCart();
        cartPage.VerifyOpenedPageUrl();
        await cartPage.VerifyCartItemsData([product1, product2, product3]);
    }

    [Test]
    public async Task RemoveFromCart_CartPage()
    {
        var product = _productsDataProvider.AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.AddProductToCart(product.Title);

        var cartPage = await OpenPage<SauceDemoCartPage>();
        await cartPage.RemoveProduct(product.Title);
        await cartPage.VerifyCartIsEmpty();
    }

    [Test]
    public async Task RemoveFromCart_InventoryPage()
    {
        var product = _productsDataProvider.AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.AddProductToCart(product.Title);

        await inventoryPage.RemoveProductFromCart(product.Title);

        var cartPage = await OpenPage<SauceDemoCartPage>();
        await cartPage.VerifyCartIsEmpty();
    }

    [Test]
    public async Task RemoveFromCart_ProductPage()
    {
        var product = _productsDataProvider.AllProducts.TakeRandom();

        var inventoryPage = await OpenPage<SauceDemoInventoryPage>();
        await inventoryPage.AddProductToCart(product.Title);
        var productPage = await inventoryPage.ClickProductTitle(product.Title);
        await productPage.ClickRemoveFromCart();

        var cartPage = await OpenPage<SauceDemoCartPage>();
        await cartPage.VerifyCartIsEmpty();
        
        // Intentionally failing the test to validate report attachment functionality
        await cartPage.VerifyCartBadgeNumber(100);
    }
}