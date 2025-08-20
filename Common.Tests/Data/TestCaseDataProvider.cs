using System.Collections;
using System.Text.RegularExpressions;
using Common.Infrastructure;
using NUnit.Framework;

namespace Common.Tests.Data;

public abstract class TestCaseDataProvider : IEnumerable
{
    protected abstract IEnumerable<TestCaseData> GetTestCaseData();

    public IEnumerator GetEnumerator() => GetTestCaseData().GetEnumerator();

    protected TestCaseData CreateTestCaseData(string parameterName, params object[] args) => new(args)
    {
        TestName = $"{{m}}({ClearName(parameterName)})"
    };

    private string ClearName(string? str)
    {
        if (str is null)
        {
            return "";
        }

        if (string.IsNullOrWhiteSpace(str))
        {
            return "";
        }

        return Regex.Replace(str, "[^a-zA-Z0-9]", " ")
            .RemoveFormatting()
            .Replace(" ", "")
            .Trim();
    }
}