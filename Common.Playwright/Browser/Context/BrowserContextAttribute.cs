using System.Diagnostics.CodeAnalysis;

namespace Common.Playwright.Browser.Context;

[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Used with reflection")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class BrowserContextAttribute<T> : Attribute where T : BrowserContextCreationStrategy
{
}