using Microsoft.Extensions.Configuration;

namespace BL.Web.Data;

public class UserCredentialsProvider
{
    private readonly Dictionary<ETestUserType, TestUserCredentials> _instance;

    public UserCredentialsProvider(IConfiguration configuration)
    {
        _instance = configuration.GetSection("UserCredentials").Get<Dictionary<ETestUserType, TestUserCredentials>>()
                    ?? throw new InvalidOperationException("Failed to load user credentials from configuration.");
    }

    public TestUserCredentials GetCredentials(ETestUserType userType)
    {
        if (!_instance.TryGetValue(userType, out var credentials))
        {
            throw new KeyNotFoundException($"No credentials found for user type: {userType}");
        }

        return credentials;
    }
}

public enum ETestUserType
{
    Standard,
    Locked
}

public record TestUserCredentials(string Username, string Password);