using BL.Web.Data;
using BL.Web.Pages;
using Common.Playwright.Browser;
using Common.Playwright.Browser.Context;
using Common.Playwright.PageFragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Context;

public class StandardUserUiLogin : BrowserContextCreationStrategy
{
    private readonly UserCredentialsProvider _userCredentialsProvider;

    public StandardUserUiLogin(PageObjectsFactory pageObjectsFactory,
        PlaywrightConfigurationData playwrightConfiguration,
        PlaywrightLifecycleManager lifecycleManager,
        UserCredentialsProvider userCredentialsProvider)
        : base(pageObjectsFactory, playwrightConfiguration, lifecycleManager)
    {
        _userCredentialsProvider = userCredentialsProvider;
    }

    protected override string FileName => nameof(StandardUserUiLogin);

    public override async Task AfterContextCreation(IBrowserContext context)
    {
        var credentials = _userCredentialsProvider.GetCredentials(ETestUserType.Standard);
        var page = await context.NewPageAsync();
        var loginPage = CreatePageObject<SauceDemoLoginPage>(page);
        await loginPage.OpenPage();
        await loginPage.Username.FillAsync(credentials.Username);
        await loginPage.Password.FillAsync(credentials.Password);
        await loginPage.LoginButton.ClickAsync();

        var inventoryPage = CreatePageObject<SauceDemoInventoryPage>(page);
        await inventoryPage.Products.WaitForAnyAsync();

        await context.StorageStateAsync(new()
        {
            Path = GetContextFilePath()
        });

        await page.CloseAsync();
    }
}