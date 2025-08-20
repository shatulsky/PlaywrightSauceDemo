using Serilog.Events;

namespace Common.Tests.Configuration;

public class LoggerConfigurationData
{
    public string LogsPath { get; set; } = null!;
    public string LogFileName { get; set; } = null!;
    public LogEventLevel MinimalLogLevel { get; set; }
    public string FileLogOutputTemplate { get; set; } = null!;
    public string ConsoleLogOutputTemplate { get; set; } = null!;
}