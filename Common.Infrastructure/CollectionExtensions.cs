namespace Common.Infrastructure;

public static class CollectionExtensions
{
    public static T TakeRandom<T>(this IEnumerable<T> source)
    {
        IEnumerable<T> enumerable = source as T[] ?? source.ToArray();
        if (!enumerable.Any())
        {
            throw new ArgumentException("Collection is empty", nameof(source));
        }

        return enumerable.ElementAt(RandomHelper.GetRandomNumber(0, enumerable.Count() - 1));
    }

    public static T TakeRandom<T>(this IEnumerable<T> source, Func<T, bool> condition)
    {
        var conditionResult = source.Where(condition).ToArray();
        return conditionResult.ElementAt(RandomHelper.GetRandomNumber(0, conditionResult.Length - 1));
    }
}