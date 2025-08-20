namespace Common.Playwright.PageObjects;

public static class PageObjectExtensions
{
    public static async Task OpenPage<T>(this T page) where T : PageObject, IOpenPage
    {
        await page.Page.GotoAsync(page.Url);
    }
}