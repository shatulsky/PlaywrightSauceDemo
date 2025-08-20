using BL.Web.Pages;
using Common.Playwright.PageFragments;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Tests.Verifications;

public static class LoginPageVerifications
{
    public static async Task VerifyLoginFieldsEmpty(this SauceDemoLoginPage page)
    {
        var actualUsername = await page.LoginButton.TextContentAsync();
        var actualPassword = await page.LoginButton.TextContentAsync();

        using (new AssertionScope())
        {
            actualUsername.Should().BeEmpty();
            actualPassword.Should().BeEmpty();
        }
    }

    public static async Task VerifyLoginButtonEnabled(this SauceDemoLoginPage page)
    {
        await page.LoginButton.Expect().ToBeVisibleAsync();
        await page.LoginButton.Expect().ToBeEnabledAsync();
    }

    public static async Task VerifyLoginErrorNotVisible(this SauceDemoLoginPage page)
    {
        await page.Error.Text.Expect().ToBeHiddenAsync();
        await page.Error.Close.Expect().ToBeHiddenAsync();
    }

    public static async Task VerifyLoginError(this SauceDemoLoginPage page, string expected)
    {
        var actual = await page.Error.GetText();
        actual.Should().Be(expected);
    }
}