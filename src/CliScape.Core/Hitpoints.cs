namespace CliScape.Core;

/// <summary>
///     Represents hitpoints (health points) for players or NPCs.
///     This is a value object that ensures valid hitpoint values.
/// </summary>
public readonly record struct Hitpoints : IComparable<Hitpoints>
{
    private readonly int _value;

    /// <summary>
    ///     Creates a new Hitpoints value.
    /// </summary>
    /// <param name="value">The hitpoints value. Must be non-negative.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value is negative.</exception>
    public Hitpoints(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        _value = value;
    }

    /// <summary>
    ///     The hitpoints value.
    /// </summary>
    public int Value => _value;

    /// <summary>
    ///     Zero hitpoints (dead).
    /// </summary>
    public static Hitpoints Zero => new(0);

    /// <summary>
    ///     Whether the entity is alive (has hitpoints).
    /// </summary>
    public bool IsAlive => _value > 0;

    /// <summary>
    ///     Whether the entity is dead (no hitpoints).
    /// </summary>
    public bool IsDead => _value == 0;

    /// <summary>
    ///     Implicit conversion from int.
    /// </summary>
    public static implicit operator Hitpoints(int value) => new(value);

    /// <summary>
    ///     Implicit conversion to int.
    /// </summary>
    public static implicit operator int(Hitpoints hp) => hp._value;

    /// <summary>
    ///     Subtracts damage from hitpoints, clamping at zero.
    /// </summary>
    public Hitpoints TakeDamage(Damage damage)
    {
        return new Hitpoints(Math.Max(0, _value - damage.Amount));
    }

    /// <summary>
    ///     Heals hitpoints up to a maximum.
    /// </summary>
    public Hitpoints Heal(int amount, Hitpoints maximum)
    {
        return new Hitpoints(Math.Min(maximum._value, _value + amount));
    }

    public static bool operator <(Hitpoints left, Hitpoints right) => left._value < right._value;
    public static bool operator >(Hitpoints left, Hitpoints right) => left._value > right._value;
    public static bool operator <=(Hitpoints left, Hitpoints right) => left._value <= right._value;
    public static bool operator >=(Hitpoints left, Hitpoints right) => left._value >= right._value;

    public int CompareTo(Hitpoints other) => _value.CompareTo(other._value);

    public override string ToString() => _value.ToString();
}
