using Common.Playwright.PageFragments;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace BL.Web.Fragments;

public class ErrorBlock : PageFragment
{
    public ILocator Text { get; }
    public ILocator Close { get; }

    public ErrorBlock(PlaywrightLifecycleManager lifecycleManager, ILocator root) : base(lifecycleManager, root)
    {
        Text = Locator("data-test=error");
        Close = Locator("button");
    }

    public async Task<string> GetText()
    {
        var text = await Text.InnerTextAsync();
        Logger.Information("Login block error text is - {Text}", text);
        return text;
    }

    public async Task ClickClose()
    {
        Logger.Information("Closing the login error block");
        await Close.ClickAsync();
    }
}