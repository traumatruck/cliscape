namespace CliScape.Core;

/// <summary>
///     Represents hitpoints (health points) for players or NPCs.
///     This is a value object that ensures valid hitpoint values.
/// </summary>
public readonly record struct Hitpoints : IComparable<Hitpoints>
{
    /// <summary>
    ///     Creates a new Hitpoints value.
    /// </summary>
    /// <param name="value">The hitpoints value. Must be non-negative.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value is negative.</exception>
    public Hitpoints(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    /// <summary>
    ///     The hitpoints value.
    /// </summary>
    public int Value { get; }

    /// <summary>
    ///     Zero hitpoints (dead).
    /// </summary>
    public static Hitpoints Zero => new(0);

    /// <summary>
    ///     Whether the entity is alive (has hitpoints).
    /// </summary>
    public bool IsAlive => Value > 0;

    /// <summary>
    ///     Whether the entity is dead (no hitpoints).
    /// </summary>
    public bool IsDead => Value == 0;

    public int CompareTo(Hitpoints other)
    {
        return Value.CompareTo(other.Value);
    }

    /// <summary>
    ///     Implicit conversion from int.
    /// </summary>
    public static implicit operator Hitpoints(int value)
    {
        return new Hitpoints(value);
    }

    /// <summary>
    ///     Implicit conversion to int.
    /// </summary>
    public static implicit operator int(Hitpoints hp)
    {
        return hp.Value;
    }

    /// <summary>
    ///     Subtracts damage from hitpoints, clamping at zero.
    /// </summary>
    public Hitpoints TakeDamage(Damage damage)
    {
        return new Hitpoints(Math.Max(0, Value - damage.Amount));
    }

    /// <summary>
    ///     Heals hitpoints up to a maximum.
    /// </summary>
    public Hitpoints Heal(int amount, Hitpoints maximum)
    {
        return new Hitpoints(Math.Min(maximum.Value, Value + amount));
    }

    public static bool operator <(Hitpoints left, Hitpoints right)
    {
        return left.Value < right.Value;
    }

    public static bool operator >(Hitpoints left, Hitpoints right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <=(Hitpoints left, Hitpoints right)
    {
        return left.Value <= right.Value;
    }

    public static bool operator >=(Hitpoints left, Hitpoints right)
    {
        return left.Value >= right.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}