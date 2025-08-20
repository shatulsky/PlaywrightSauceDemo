using BL.Web.Data;
using BL.Web.Pages;
using Common.Playwright.PageObjects;
using Common.Tests.Attributes;
using NUnit.Framework;
using Tests.Configuration;
using Tests.Data;
using Tests.Verifications;

namespace Tests.Tests;

public class LogInTests : BaseTests
{
    private readonly UserCredentialsProvider _credentialsProvider = ServiceProvider.GetService<UserCredentialsProvider>();

    [Test]
    public async Task Login_ValidUser()
    {
        var userCredentials = _credentialsProvider.GetCredentials(ETestUserType.Standard);

        var loginPage = await OpenPage<SauceDemoLoginPage>();
        await loginPage.VerifyLoginFieldsEmpty();
        await loginPage.VerifyLoginButtonEnabled();

        await loginPage.FillLoginCredentials(userCredentials.Username, userCredentials.Password);

        var inventoryPage = await loginPage.ClickLogin();
        inventoryPage.VerifyOpenedPageUrl();
        await inventoryPage.VerifyProductsAreLoaded();
    }

    [Test]
    public async Task Login_Error_FieldsRequired()
    {
        var loginPage = await OpenPage<SauceDemoLoginPage>();

        await loginPage.ClickLogin();
        await loginPage.VerifyLoginError("Epic sadface: Username is required");

        await loginPage.Error.ClickClose();
        await loginPage.VerifyLoginErrorNotVisible();

        await loginPage.FillLoginCredentials("test", "");
        
        await loginPage.ClickLogin();
        await loginPage.VerifyLoginError("Epic sadface: Password is required");
    }

    [Test]
    public async Task Login_Error_IncorrectUsername()
    {
        var userCredentials = _credentialsProvider.GetCredentials(ETestUserType.Standard);
        var nonexistentUserName = userCredentials.Username + "1";

        var loginPage = await OpenPage<SauceDemoLoginPage>();

        await loginPage.FillLoginCredentials(nonexistentUserName, userCredentials.Password);

        await loginPage.VerifyLoginErrorNotVisible();
        await loginPage.ClickLogin();

        await loginPage.VerifyLoginError("Epic sadface: Username and password do not match any user in this service");

        await loginPage.Error.ClickClose();
        await loginPage.VerifyLoginErrorNotVisible();
    }

    [Test]
    public async Task Login_Error_IncorrectPassword()
    {
        var userCredentials = _credentialsProvider.GetCredentials(ETestUserType.Standard);
        const string incorrectPassword = "incorrect_password";

        var loginPage = await OpenPage<SauceDemoLoginPage>();

        await loginPage.FillLoginCredentials(userCredentials.Username, incorrectPassword);

        await loginPage.VerifyLoginErrorNotVisible();
        await loginPage.ClickLogin();

        await loginPage.VerifyLoginError("Epic sadface: Username and password do not match any user in this service");

        await loginPage.Error.ClickClose();
        await loginPage.VerifyLoginErrorNotVisible();
    }

    [Test]
    public async Task Login_Error_LockedOutUser()
    {
        var userCredentials = _credentialsProvider.GetCredentials(ETestUserType.Locked);

        var loginPage = await OpenPage<SauceDemoLoginPage>();

        await loginPage.FillLoginCredentials(userCredentials.Username, userCredentials.Password);
        await loginPage.ClickLogin();

        await loginPage.VerifyLoginError("Epic sadface: Sorry, this user has been locked out.");
    }

    [TestCaseSource<RequireAuthPagesTestCaseDataProvider>]
    public async Task Login_Error_Unauthorized(Type pageType)
    {
        var playwrightLifecycleManager = ServiceProvider.GetService<PlaywrightLifecycleManager>();

        var page = await playwrightLifecycleManager.CreatePageObjectAsync(pageType);
        var pageUrl = ((IOpenPage)page).Url;
        await page.Page.GotoAsync(pageUrl);

        var loginPage = await playwrightLifecycleManager.CreatePageObjectAsync<SauceDemoLoginPage>();

        await loginPage.VerifyLoginError($"Epic sadface: You can only access '/{pageUrl}' when you are logged in.");
        loginPage.VerifyOpenedPageUrl();
    }
}