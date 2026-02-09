namespace CliScape.Core;

/// <summary>
///     Default implementation of <see cref="IRandomProvider" /> using <see cref="Random.Shared" />.
/// </summary>
public sealed class RandomProvider : IRandomProvider
{
    /// <summary>
    ///     Singleton instance for simple access. For DI scenarios, register as a service instead.
    /// </summary>
    public static readonly RandomProvider Instance = new();

    private RandomProvider()
    {
    }

    /// <inheritdoc />
    public int Next(int maxValue)
    {
        return Random.Shared.Next(maxValue);
    }

    /// <inheritdoc />
    public int Next(int minValue, int maxValue)
    {
        return Random.Shared.Next(minValue, maxValue);
    }

    /// <inheritdoc />
    public double NextDouble()
    {
        return Random.Shared.NextDouble();
    }

    /// <inheritdoc />
    public int NextPercent()
    {
        return Random.Shared.Next(100);
    }
}