using BL.Web.Fragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Pages;

public class SauceDemoCheckoutInformationPage : SauceDemoBasePage, IOpenPage
{
    public string Url => "checkout-step-one.html";
    public ILocator FirstName { get; }
    public ILocator LastName { get; }
    public ILocator PostalCode { get; }
    public ErrorBlock Error { get; set; }
    public ILocator Continue { get; }

    public SauceDemoCheckoutInformationPage(PlaywrightLifecycleManager lifecycleManager, IPage page) : base(lifecycleManager, page)
    {
        FirstName = Locator("#first-name");
        LastName = Locator("#last-name");
        PostalCode = Locator("#postal-code");
        Error = LocatorFragment<ErrorBlock>(".error-message-container");
        Continue = Locator("#continue");
    }

    public async Task FillData(string? firstName, string? lastName, string? postalCode)
    {
        Logger.Information("Filling checkout data- First name: {FirstName}, Last Name: {LastName}, Postal code: {PostalCode}", firstName, lastName, postalCode);
        if (firstName != null)
        {
            await FirstName.FillAsync(firstName);
        }

        if (lastName != null)
        {
            await LastName.FillAsync(lastName);
        }

        if (postalCode != null)
        {
            await PostalCode.FillAsync(postalCode);
        }
    }

    public async Task<SauceDemoCheckoutOverviewPage> ClickContinue()
    {
        Logger.Information("Clicking the Continue button.");
        await Continue.ClickAsync();
        return await NewPage<SauceDemoCheckoutOverviewPage>();
    }
}