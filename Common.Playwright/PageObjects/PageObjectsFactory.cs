using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace Common.Playwright.PageObjects;

public class PageObjectsFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, Type> _types = new();

    public PageObjectsFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public PageObject CreatePage(Type type, PlaywrightLifecycleManager lifecycleManager, IPage page)
    {
        if (!type.IsAssignableTo(typeof(PageObject)))
        {
            throw new ArgumentException($"Type {type.FullName} must inherit from PageObject.", nameof(type));
        }

        if (!_types.TryGetValue(type, out var registeredType))
        {
            throw new ArgumentException($"{type.FullName} is not registered to page factory");
        }

        var constructors = registeredType.GetTypeInfo()
            .DeclaredConstructors
            .Where(c => !c.IsStatic)
            .ToArray();

        if (constructors.Length != 1)
        {
            throw new ArgumentException($"Single constructor should be defined for {type.FullName}");
        }

        var constructor = constructors.First();

        var parameters = constructor.GetParameters()
            .Select(p => p.ParameterType)
            .Select(t => t switch
            {
                not null when t == typeof(PlaywrightLifecycleManager) => lifecycleManager,
                not null when t == typeof(IPage) => page,
                _ => _serviceProvider.GetRequiredService(t!)
            })
            .ToArray();

        return (PageObject)constructor.Invoke(parameters);
    }

    public void RegisterAllPageObjects(Assembly assembly)
    {
        var pages = GetAllPageTypes(typeof(PageObject), assembly);
        foreach (var page in pages)
        {
            RegisterPage(page);
        }
    }

    private void RegisterPage(Type type)
    {
        if (!type.IsAssignableTo(typeof(PageObject)))
        {
            throw new ArgumentException($"Type {type.FullName} must inherit from PageObject.", nameof(type));
        }

        _types[type] = type;
    }

    private Type[] GetAllPageTypes(Type basePage, Assembly assembly)
    {
        return assembly.DefinedTypes
            .Where(t => t.IsAssignableTo(basePage) && IsValidPageObject(t))
            .ToArray<Type>();
    }

    private static bool IsValidPageObject(Type typeInfo) => typeInfo is
    {
        IsClass: true,
        IsAbstract: false,
        IsPublic: true,
        ContainsGenericParameters: false
    };
}