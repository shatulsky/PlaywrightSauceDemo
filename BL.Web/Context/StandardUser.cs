using System.Text.Json;
using BL.Web.Data;
using Common.Playwright.Browser;
using Common.Playwright.Browser.Context;
using Common.Playwright.Browser.Context.Contracts;
using Common.Playwright.PageObjects;

namespace BL.Web.Context;

public class StandardUser : BrowserContextCreationStrategy
{
    private readonly PlaywrightConfigurationData _playwrightConfiguration;
    private readonly UserCredentialsProvider _userCredentialsProvider;

    public StandardUser(PageObjectsFactory pageObjectsFactory,
        PlaywrightConfigurationData playwrightConfiguration,
        PlaywrightLifecycleManager lifecycleManager,
        UserCredentialsProvider userCredentialsProvider)
        : base(pageObjectsFactory, playwrightConfiguration, lifecycleManager)
    {
        _playwrightConfiguration = playwrightConfiguration;
        _userCredentialsProvider = userCredentialsProvider;
    }

    protected override string FileName => nameof(StandardUser);

    public override void BeforeContextCreation()
    {
        var filePath = GetContextFilePath();
        var username = _userCredentialsProvider.GetCredentials(ETestUserType.Standard).Username;
        var host = new Uri(_playwrightConfiguration.BaseUrl).Host;
        var data = new ContextData
        {
            Cookies =
            [
                new()
                {
                    Name = "session-username",
                    Value = username,
                    Domain = host,
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds() / 1000,
                    HttpOnly = false,
                    Secure = false,
                    SameSite = "Lax"
                }
            ],
            Origins =
            [
                new OriginData
                {
                    Origin = _playwrightConfiguration.BaseUrl,
                    LocalStorage =
                    [
                        new()
                        {
                            Name = "backtrace-guid",
                            Value = Guid.NewGuid().ToString()
                        },

                        new()
                        {
                            Name = "backtrace-last-active",
                            Value = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()
                        }
                    ]
                }
            ]
        };

        var serializedData = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        File.WriteAllText(filePath, serializedData);
    }
}