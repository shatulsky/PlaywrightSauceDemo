namespace Common.Infrastructure;

public static class StringExtensions
{
    private const string SpaceString = " ";


    public static string RemoveFormatting(this string s)
    {
        return s.Replace("\r", SpaceString)
            .Replace("\n", SpaceString)
            .Replace("\t", SpaceString)
            .Trim();
    }
}