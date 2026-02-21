using CliScape.Core;

namespace CliScape.Content.Tests.Helpers;

/// <summary>
///     Deterministic random provider for testing.
///     Always returns the configured value from Next().
/// </summary>
public sealed class StubRandomProvider : IRandomProvider
{
    private readonly Queue<int> _nextValues = new();
    private int _defaultValue;

    public StubRandomProvider(int defaultValue = 0)
    {
        _defaultValue = defaultValue;
    }

    /// <summary>
    ///     Enqueue a specific value to be returned by the next call to Next().
    /// </summary>
    public StubRandomProvider Returns(int value)
    {
        _nextValues.Enqueue(value);
        return this;
    }

    /// <summary>
    ///     Enqueue one or more values to be returned by subsequent Next() calls.
    /// </summary>
    public StubRandomProvider EnqueueRange(params int[] values)
    {
        foreach (var v in values)
            _nextValues.Enqueue(v);
        return this;
    }

    /// <summary>
    ///     Set the default value returned when no queued values remain.
    /// </summary>
    public StubRandomProvider WithDefault(int value)
    {
        _defaultValue = value;
        return this;
    }

    public int Next(int maxValue)
    {
        var value = _nextValues.Count > 0 ? _nextValues.Dequeue() : _defaultValue;
        return Math.Clamp(value, 0, maxValue - 1);
    }

    public int Next(int minValue, int maxValue)
    {
        var value = _nextValues.Count > 0 ? _nextValues.Dequeue() : _defaultValue;
        return Math.Clamp(value, minValue, maxValue - 1);
    }

    public double NextDouble()
    {
        return _nextValues.Count > 0 ? _nextValues.Dequeue() / 100.0 : _defaultValue / 100.0;
    }

    public int NextPercent()
    {
        var value = _nextValues.Count > 0 ? _nextValues.Dequeue() : _defaultValue;
        return Math.Clamp(value, 0, 99);
    }
}
