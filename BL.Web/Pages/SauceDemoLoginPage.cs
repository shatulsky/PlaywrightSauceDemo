using BL.Web.Fragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Pages;

public class SauceDemoLoginPage : SauceDemoBasePage, IOpenPage
{
    public string Url => "";
    public ILocator Username { get; }
    public ILocator Password { get; }
    public ILocator LoginButton { get; }
    public ErrorBlock Error { get; set; }

    public SauceDemoLoginPage(PlaywrightLifecycleManager lifecycleManager, IPage page) : base(lifecycleManager, page)
    {
        Username = Locator("#user-name");
        Password = Locator("#password");
        LoginButton = Locator("#login-button");
        Error = LocatorFragment<ErrorBlock>(".error-message-container");
    }

    public async Task FillLoginCredentials(string login, string password)
    {
        Logger.Information("Filling credentials - username: {Login}, password: {Password}", login, password);
        await Username.FillAsync(login);
        await Password.FillAsync(password);
    }

    public async Task<SauceDemoInventoryPage> ClickLogin()
    {
        await LoginButton.ClickAsync();
        return await NewPage<SauceDemoInventoryPage>();
    }
}