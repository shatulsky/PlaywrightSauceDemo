namespace Common.Infrastructure;

public static class RandomHelper
{
    public static Random Random { get; } = new(DateTime.UtcNow.Ticks.GetHashCode());
    public static int GetRandomNumber(int minValue, int maxValue) => Random.Next(minValue, maxValue + 1);
}