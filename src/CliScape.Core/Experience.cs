namespace CliScape.Core;

/// <summary>
///     Represents an experience amount for skills.
///     This is a value object that ensures valid XP values (0 to 200,000,000).
/// </summary>
public readonly record struct Experience : IComparable<Experience>
{
    /// <summary>
    ///     Maximum experience in any skill (200 million).
    /// </summary>
    public const int MaxExperience = 200_000_000;

    /// <summary>
    ///     Creates a new Experience value.
    /// </summary>
    /// <param name="value">The experience value. Must be between 0 and 200,000,000.</param>
    public Experience(int value)
    {
        Value = Math.Clamp(value, 0, MaxExperience);
    }

    /// <summary>
    ///     The experience value.
    /// </summary>
    public int Value { get; }

    /// <summary>
    ///     Zero experience.
    /// </summary>
    public static Experience Zero => new(0);

    /// <summary>
    ///     Maximum experience (200M).
    /// </summary>
    public static Experience Max => new(MaxExperience);

    /// <summary>
    ///     Whether the experience is at the maximum.
    /// </summary>
    public bool IsMaxed => Value >= MaxExperience;

    public int CompareTo(Experience other)
    {
        return Value.CompareTo(other.Value);
    }

    /// <summary>
    ///     Implicit conversion from int.
    /// </summary>
    public static implicit operator Experience(int value)
    {
        return new Experience(value);
    }

    /// <summary>
    ///     Implicit conversion to int.
    /// </summary>
    public static implicit operator int(Experience xp)
    {
        return xp.Value;
    }

    /// <summary>
    ///     Adds experience, capping at maximum.
    /// </summary>
    public Experience Add(int amount)
    {
        return new Experience(Value + amount);
    }

    public static Experience operator +(Experience left, int right)
    {
        return left.Add(right);
    }

    public static Experience operator +(Experience left, Experience right)
    {
        return new Experience(left.Value + right.Value);
    }

    public static bool operator <(Experience left, Experience right)
    {
        return left.Value < right.Value;
    }

    public static bool operator >(Experience left, Experience right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <=(Experience left, Experience right)
    {
        return left.Value <= right.Value;
    }

    public static bool operator >=(Experience left, Experience right)
    {
        return left.Value >= right.Value;
    }

    /// <summary>
    ///     Formats the experience for display.
    /// </summary>
    public string ToDisplayString()
    {
        return Value switch
        {
            >= 1_000_000 => $"{Value / 1_000_000.0:0.##}M",
            >= 10_000 => $"{Value / 1_000.0:0.#}K",
            _ => $"{Value:N0}"
        };
    }

    public override string ToString()
    {
        return ToDisplayString();
    }
}