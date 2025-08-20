using System.Diagnostics;
using Microsoft.Playwright;

namespace Common.Playwright.PageFragments;

public static class LocatorExtensions
{
    public static ILocatorAssertions Expect(this ILocator locator) => Assertions.Expect(locator);

    public static async Task<IReadOnlyList<ILocator>> WaitForAnyAsync(this ILocator locator, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(5);
        const int retryInterval = 100;
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < timeout)
        {
            var elements = await locator.AllAsync();
            if (elements.Count > 0)
            {
                return elements;
            }

            await Task.Delay(retryInterval);
        }

        throw new TimeoutException($"No elements found matching the locator within the timeout period of {timeout.Value.TotalMilliseconds}ms.");
    }
}