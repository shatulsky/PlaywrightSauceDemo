using System.Reflection;
using Common.Infrastructure;
using Common.Playwright.Browser.Context;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Tests.Configuration.Runner;

public static class NUnitHelper
{
    public static bool IsTestFailed()
    {
        var currentTestResult = TestContext.CurrentContext.Result;
        return currentTestResult.Outcome == ResultState.Error || currentTestResult.Outcome == ResultState.Failure;
    }

    public static void RegisterContextCreationAttributes(object testClass)
    {
        var attributeType = typeof(BrowserContextAttribute<>);
        var methodAttributes = GetContextAttributesFromMethod(TestContext.CurrentContext.Test.Method!, attributeType).ToArray();
        if (methodAttributes.Any())
        {
            RegisterAttribute(methodAttributes);
            return;
        }

        var classAttributes = GetAttributesFromClass(testClass, attributeType).ToArray();
        if (classAttributes.Any())
        {
            RegisterAttribute(classAttributes);
        }
    }

    private static void RegisterAttribute(CustomAttributeData[] attributes)
    {
        if (attributes.Length > 1)
        {
            throw new InvalidOperationException("Exactly one ContextAttribute must exist for class/method.");
        }

        var identifier = ServiceProvider.GetService<IScopeIdentifier>().Identifier;
        ServiceProvider.GetService<ContextCreationStrategyStorage>().RegisterStrategy(identifier, attributes.First());
    }

    private static IEnumerable<CustomAttributeData> GetAttributesFromClass(object testClass, Type attributeType)
    {
        return testClass.GetType().CustomAttributes.Where(a => FilterAttributes(attributeType, a));
    }

    private static IEnumerable<CustomAttributeData> GetContextAttributesFromMethod(IMethodInfo methodInfo, Type attributeType)
    {
        return methodInfo.MethodInfo.CustomAttributes.Where(a => FilterAttributes(attributeType, a));
    }

    private static bool FilterAttributes(Type attributeType, CustomAttributeData a)
    {
        return a.AttributeType == attributeType
               || (a.AttributeType.IsGenericType && a.AttributeType.GetGenericTypeDefinition() == attributeType);
    }
}