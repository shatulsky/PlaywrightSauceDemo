using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.Infrastructure;

public static class ObjectExtensions
{
    private static readonly StringEnumConverter StringEnumConverted = new();
    
    public static string ToBeautifiedJsonString<T>(this T? obj) => JsonConvert.SerializeObject(obj, Formatting.Indented, StringEnumConverted);
}