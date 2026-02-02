namespace CliScape.Core;

/// <summary>
///     Represents a gold amount (coins) in the game.
///     This is a value object that ensures non-negative values and provides domain-specific operations.
/// </summary>
public readonly record struct Gold : IComparable<Gold>
{
    /// <summary>
    ///     Creates a new Gold value.
    /// </summary>
    /// <param name="amount">The amount of gold. Must be non-negative.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when amount is negative.</exception>
    public Gold(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Amount = amount;
    }

    /// <summary>
    ///     The amount of gold pieces.
    /// </summary>
    public int Amount { get; }

    /// <summary>
    ///     Zero gold.
    /// </summary>
    public static Gold Zero => new(0);

    /// <summary>
    ///     Implicit conversion from int.
    /// </summary>
    public static implicit operator Gold(int amount) => new(amount);

    /// <summary>
    ///     Implicit conversion to int.
    /// </summary>
    public static implicit operator int(Gold gold) => gold.Amount;

    public static Gold operator +(Gold left, Gold right) => new(left.Amount + right.Amount);
    public static Gold operator -(Gold left, Gold right) => new(Math.Max(0, left.Amount - right.Amount));
    public static Gold operator *(Gold left, int multiplier) => new(left.Amount * multiplier);
    public static Gold operator /(Gold left, int divisor) => new(left.Amount / divisor);

    public static bool operator <(Gold left, Gold right) => left.Amount < right.Amount;
    public static bool operator >(Gold left, Gold right) => left.Amount > right.Amount;
    public static bool operator <=(Gold left, Gold right) => left.Amount <= right.Amount;
    public static bool operator >=(Gold left, Gold right) => left.Amount >= right.Amount;

    public int CompareTo(Gold other) => Amount.CompareTo(other.Amount);

    /// <summary>
    ///     Formats the gold amount for display (e.g., "1,234 gp" or "1.2M").
    /// </summary>
    public string ToDisplayString()
    {
        return Amount switch
        {
            >= 10_000_000 => $"{Amount / 1_000_000.0:0.#}M",
            >= 100_000 => $"{Amount / 1_000.0:0.#}K",
            _ => $"{Amount:N0} gp"
        };
    }

    public override string ToString() => ToDisplayString();
}
