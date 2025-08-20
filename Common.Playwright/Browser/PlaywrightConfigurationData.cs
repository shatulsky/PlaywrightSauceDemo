namespace Common.Playwright.Browser;

public class PlaywrightConfigurationData
{
    public string BaseUrl { get; set; } = null!;
    public string Browser { get; set; } = null!;
    public bool Headless { get; set; }
    public int SlowMo { get; set; }
    public string StorageStateDirectory { get; set; } = null!;
    public string TraceReportDirectory { get; set; } = null!;
}