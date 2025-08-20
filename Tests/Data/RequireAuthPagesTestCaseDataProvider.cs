using BL.Web.Pages;
using Common.Playwright.PageObjects;
using Common.Tests.Data;
using NUnit.Framework;

namespace Tests.Data;

public class RequireAuthPagesTestCaseDataProvider : TestCaseDataProvider
{
    private static readonly Type[] ExcludedPages = [typeof(SauceDemoLoginPage)];

    protected override IEnumerable<TestCaseData> GetTestCaseData()
    {
        var pageTypes = typeof(SauceDemoBasePage).Assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && t.IsSubclassOf(typeof(SauceDemoBasePage)) && typeof(IOpenPage).IsAssignableFrom(t));

        foreach (var pageType in pageTypes)
        {
            if (ExcludedPages.Contains(pageType))
            {
                continue;
            }

            yield return CreateTestCaseData(pageType.Name, pageType);
        }
    }
}