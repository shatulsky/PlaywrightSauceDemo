using Common.Infrastructure;
using NUnit.Framework;

namespace Tests.Configuration.Runner;

public class NUnitScopeIdentifier : IScopeIdentifier
{
    public string Identifier => TestContext.CurrentContext.Test.FullName;
}