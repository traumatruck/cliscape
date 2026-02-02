namespace CliScape.Core;

/// <summary>
///     Represents a damage amount in combat.
///     This is a value object that ensures non-negative values and provides domain-specific operations.
/// </summary>
public readonly record struct Damage : IComparable<Damage>
{
    /// <summary>
    ///     Creates a new Damage value.
    /// </summary>
    /// <param name="amount">The amount of damage. Must be non-negative.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when amount is negative.</exception>
    public Damage(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Amount = amount;
    }

    /// <summary>
    ///     The damage amount.
    /// </summary>
    public int Amount { get; }

    /// <summary>
    ///     Zero damage (miss).
    /// </summary>
    public static Damage Zero => new(0);

    /// <summary>
    ///     Whether this is a hit (non-zero damage).
    /// </summary>
    public bool IsHit => Amount > 0;

    /// <summary>
    ///     Whether this is a miss (zero damage).
    /// </summary>
    public bool IsMiss => Amount == 0;

    /// <summary>
    ///     Implicit conversion from int.
    /// </summary>
    public static implicit operator Damage(int amount) => new(amount);

    /// <summary>
    ///     Implicit conversion to int.
    /// </summary>
    public static implicit operator int(Damage damage) => damage.Amount;

    public static Damage operator +(Damage left, Damage right) => new(left.Amount + right.Amount);

    public static bool operator <(Damage left, Damage right) => left.Amount < right.Amount;
    public static bool operator >(Damage left, Damage right) => left.Amount > right.Amount;
    public static bool operator <=(Damage left, Damage right) => left.Amount <= right.Amount;
    public static bool operator >=(Damage left, Damage right) => left.Amount >= right.Amount;

    public int CompareTo(Damage other) => Amount.CompareTo(other.Amount);

    public override string ToString() => Amount.ToString();
}
