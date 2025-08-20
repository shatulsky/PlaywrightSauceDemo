using Common.Infrastructure;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Common.Tests.Configuration;

public class LoggerProvider
{
    private static readonly Lazy<ILogger> Lazy = new(InitLogger);

    public static ILogger Instance => Lazy.Value;

    private static ILogger InitLogger()
    {
        var testResultsConfig = ConfigurationProvider.Instance.GetSection("Logger").Get<LoggerConfigurationData>()!;
        var logFilePath = Path.Combine(PathHelper.GetAssemblyPath(), testResultsConfig.LogsPath, testResultsConfig.LogFileName);

        var loggerBuilder = new LoggerConfiguration()
            .MinimumLevel.Is(testResultsConfig.MinimalLogLevel)
            .Enrich.WithThreadId()
            .WriteTo.Console(outputTemplate: testResultsConfig.ConsoleLogOutputTemplate)
            .WriteTo.File(logFilePath, outputTemplate: testResultsConfig.FileLogOutputTemplate);

        return loggerBuilder.CreateLogger();
    }
}