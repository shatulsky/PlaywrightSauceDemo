namespace Common.Playwright.Browser.Context.Contracts;

public class OriginData
{
    public string? Origin { get; set; }
    public List<LocalStorageData>? LocalStorage { get; set; }
}