namespace CliScape.Core.Slayer;

/// <summary>
///     Represents a possible slayer task assignment.
/// </summary>
public record SlayerAssignment
{
    /// <summary>
    ///     The category of NPCs for this assignment.
    /// </summary>
    public required string Category { get; init; }

    /// <summary>
    ///     The minimum number of kills that can be assigned.
    /// </summary>
    public required int MinKills { get; init; }

    /// <summary>
    ///     The maximum number of kills that can be assigned.
    /// </summary>
    public required int MaxKills { get; init; }

    /// <summary>
    ///     The weight of this assignment (higher = more likely).
    /// </summary>
    public int Weight { get; init; } = 1;

    /// <summary>
    ///     The required slayer level for this assignment.
    /// </summary>
    public int RequiredSlayerLevel { get; init; }
}
