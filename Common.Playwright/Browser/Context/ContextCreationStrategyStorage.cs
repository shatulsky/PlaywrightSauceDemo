using System.Collections.Concurrent;
using System.Reflection;
using Common.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Playwright.Browser.Context;

public class ContextCreationStrategyStorage
{
    private readonly IScopeIdentifier _scopeIdentifier;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, BrowserContextCreationStrategy> _strategies = new();

    public ContextCreationStrategyStorage(IScopeIdentifier scopeIdentifier, IServiceProvider serviceProvider)
    {
        _scopeIdentifier = scopeIdentifier;
        _serviceProvider = serviceProvider;
    }

    public void RegisterStrategy(string identifier, CustomAttributeData attributeData)
    {
        var type = attributeData.AttributeType.GenericTypeArguments.First();
        if (!typeof(BrowserContextCreationStrategy).IsAssignableFrom(type))
        {
            throw new ArgumentException($"Type must implement {nameof(BrowserContextCreationStrategy)}", nameof(type));
        }

        var constructors = type.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsStatic).ToArray();

        if (constructors.Length != 1)
        {
            throw new ArgumentException($"Single constructor should be defined for {type.FullName}");
        }

        var constructor = constructors.First();

        var parameters = constructor.GetParameters()
            .Select(p => p.ParameterType)
            .Select(t => _serviceProvider.GetRequiredService(t))
            .ToArray();

        var strategy = (BrowserContextCreationStrategy)constructor.Invoke(parameters);
        
        _strategies.TryAdd(identifier, strategy);
    }

    public void CleanInstanceStrategy(string identifier)
    {
        _strategies.TryRemove(identifier, out _);
    }

    public BrowserContextCreationStrategy? GetStrategy()
    {
        return _strategies.GetValueOrDefault(_scopeIdentifier.Identifier);
    }
}