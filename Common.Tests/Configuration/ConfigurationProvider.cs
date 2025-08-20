using Common.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Common.Tests.Configuration;

public static class ConfigurationProvider
{
    private static readonly Lazy<IConfiguration> Lazy = new(InitConfiguration);

    public static IConfiguration Instance => Lazy.Value;

    private static IConfiguration InitConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(PathHelper.GetAssemblyPath())
            .AddJsonFile("appsettings.json", false, false)
            .AddJsonFile("appsettings.Development.json", true, false)
            .Build();
    }
}