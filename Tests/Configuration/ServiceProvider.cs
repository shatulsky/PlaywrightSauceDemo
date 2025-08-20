using BL.Web.Data;
using BL.Web.Pages;
using Common.Infrastructure;
using Common.Playwright.Browser;
using Common.Playwright.Browser.Context;
using Common.Playwright.PageObjects;
using Common.Tests.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Configuration.Runner;
using ConfigurationProvider = Common.Tests.Configuration.ConfigurationProvider;

namespace Tests.Configuration;

public static class ServiceProvider
{
    private static readonly Lazy<IServiceProvider> Lazy = new(InitServices);

    public static IServiceProvider Instance => Lazy.Value;

    private static IServiceProvider InitServices()
    {
        var serviceCollection = new ServiceCollection()
            .AddConfiguration()
            .AddPlaywright()
            .AddSingleton(LoggerProvider.Instance)
            .AddSingleton(s => s)
            .AddSingleton<IScopeIdentifier, NUnitScopeIdentifier>()
            .AddSingleton<NunitPlaywrightReportManager>()
            .AddSingleton<UserCredentialsProvider>()
            .AddSingleton<ProductsDataProvider>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        serviceProvider.GetRequiredService<PageObjectsFactory>().RegisterAllPageObjects(typeof(SauceDemoLoginPage).Assembly);

        return serviceProvider;
    }

    private static IServiceCollection AddPlaywright(this IServiceCollection s)
    {
        return s.AddSingleton<PlaywrightFactory>()
            .AddSingleton<PlaywrightContextFactory>()
            .AddSingleton<PlaywrightBrowserFactory>()
            .AddSingleton<PlaywrightBrowserProvider>()
            .AddSingleton<PlaywrightLifecycleManager>()
            .AddSingleton<PageObjectsFactory>()
            .AddSingleton<PlaywrightPageProvider>()
            .AddSingleton<ContextCreationStrategyStorage>();
    }

    private static IServiceCollection AddConfiguration(this IServiceCollection s)
    {
        var config = ConfigurationProvider.Instance;

        return s.AddSingleton(config)
            .AddSingleton(config.GetSection("Logger").Get<LoggerConfigurationData>()!)
            .AddSingleton(config.GetSection("Playwright").Get<PlaywrightConfigurationData>()!);
    }

    public static T GetService<T>() where T : notnull
    {
        return Instance.GetRequiredService<T>();
    }
}