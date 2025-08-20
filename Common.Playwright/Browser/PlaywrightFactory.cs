using Microsoft.Playwright;

namespace Common.Playwright.Browser;

using Playwright = Microsoft.Playwright.Playwright;

public class PlaywrightFactory
{
    public async Task<IPlaywright> Create()
    {
        return await Playwright.CreateAsync();
    }
}