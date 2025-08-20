using System.Reflection;

namespace Common.Infrastructure;

public class PathHelper
{
    public static string GetAssemblyPath() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
}