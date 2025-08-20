using System.Reflection;
using Common.Playwright.PageObjects;
using Microsoft.Playwright;

namespace Common.Playwright.PageFragments;

public static class PageFragmentExtensions
{
    public static IReadOnlyList<T> ToFragment<T>(this IReadOnlyList<ILocator> locators, PlaywrightLifecycleManager lifecycleManager) where T : PageFragment
    {
        return locators.Select(l => CreateFragment<T>(l, lifecycleManager)).ToArray();
    }
    
    public static T ToFragment<T>(this ILocator locator, PlaywrightLifecycleManager lifecycleManager) where T : PageFragment
    {
        return CreateFragment<T>(locator, lifecycleManager);
    }

    public static T LocatorFragment<T>(this ILocator locator, string selector, PlaywrightLifecycleManager lifecycleManager) where T : PageFragment
    {
        return CreateFragment<T>(locator.Locator(selector), lifecycleManager);
    }

    public static T LocatorFragment<T>(this IPage page, string selector, PlaywrightLifecycleManager lifecycleManager) where T : PageFragment
    {
        return CreateFragment<T>(page.Locator(selector), lifecycleManager);
    }

    private static T CreateFragment<T>(ILocator locator, PlaywrightLifecycleManager pageProvider) where T : PageFragment
    {
        return (T)typeof(T).GetTypeInfo().DeclaredConstructors.First().Invoke([pageProvider, locator]);
    }
}