using CliScape.Core.Items;

namespace CliScape.Core.ClueScrolls;

/// <summary>
///     Represents a possible reward from completing a clue scroll.
/// </summary>
public record ClueReward
{
    /// <summary>
    ///     The item ID of the reward.
    /// </summary>
    public required ItemId ItemId { get; init; }

    /// <summary>
    ///     The minimum quantity of this item that can be rewarded.
    /// </summary>
    public int MinQuantity { get; init; } = 1;

    /// <summary>
    ///     The maximum quantity of this item that can be rewarded.
    /// </summary>
    public int MaxQuantity { get; init; } = 1;

    /// <summary>
    ///     The relative weight of this reward in the reward table (higher = more common).
    /// </summary>
    public int Weight { get; init; } = 1;
}