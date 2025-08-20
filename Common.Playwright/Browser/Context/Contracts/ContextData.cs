namespace Common.Playwright.Browser.Context.Contracts;

public class ContextData
{
    public List<CookieData>? Cookies { get; set; }
    public List<OriginData>? Origins { get; set; }
}