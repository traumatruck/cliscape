namespace CliScape.Core;

/// <summary>
///     Abstraction for random number generation to enable testability
///     and centralized random state management.
/// </summary>
public interface IRandomProvider
{
    /// <summary>
    ///     Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
    int Next(int maxValue);

    /// <summary>
    ///     Returns a random integer that is within a specified range.
    /// </summary>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
    int Next(int minValue, int maxValue);

    /// <summary>
    ///     Returns a random floating-point number that is greater than or equal to 0.0 and less than 1.0.
    /// </summary>
    double NextDouble();

    /// <summary>
    ///     Returns a random integer in the range [0, 99] for percentage-based checks.
    /// </summary>
    int NextPercent();
}
