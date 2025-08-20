using System.Collections;
using Common.Tests.Data;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Common.Tests.Attributes;

public class TestCaseSource<T> : NUnitAttribute, ITestBuilder, IImplyFixture, IApplyToTest
    where T : TestCaseDataProvider
{
    private readonly NUnitTestCaseBuilder _builder = new();

    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test? suite)
    {
        var testCaseData = ((IEnumerable)Reflect.Construct(typeof(T), null)).Cast<TestCaseData>();

        foreach (var testCase in testCaseData)
        {
            yield return _builder.BuildTestMethod(method, suite, testCase);
        }
    }

    public void ApplyToTest(Test test)
    {
        if (test.Method == null)
        {
            throw new ArgumentException("This attribute must only be applied to tests that have an associated method.", nameof(test));
        }
    }
}