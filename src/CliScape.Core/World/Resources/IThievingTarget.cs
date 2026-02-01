using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents a thieving target in the world that players can steal from.
/// </summary>
public interface IThievingTarget
{
    /// <summary>
    ///     The display name of this thieving target.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The type of thieving target.
    /// </summary>
    ThievingTargetType TargetType { get; }

    /// <summary>
    ///     The minimum thieving level required to steal from this target.
    /// </summary>
    int RequiredLevel { get; }

    /// <summary>
    ///     The thieving experience gained when successfully stealing.
    /// </summary>
    int Experience { get; }

    /// <summary>
    ///     The possible loot that can be obtained from this target.
    /// </summary>
    IReadOnlyList<ThievingLoot> PossibleLoot { get; }

    /// <summary>
    ///     The base success chance at level 1 (0.0 to 1.0).
    ///     Higher levels improve the chance.
    /// </summary>
    double BaseSuccessChance { get; }

    /// <summary>
    ///     The damage taken when caught stealing.
    /// </summary>
    int FailureDamage { get; }
}

/// <summary>
///     The type of thieving target.
/// </summary>
public enum ThievingTargetType
{
    /// <summary>
    ///     A market stall to steal from.
    /// </summary>
    Stall,

    /// <summary>
    ///     An NPC to pickpocket.
    /// </summary>
    Npc
}

/// <summary>
///     Represents possible loot from a thieving target.
/// </summary>
/// <param name="ItemId">The item ID of the loot.</param>
/// <param name="MinQuantity">The minimum quantity of items.</param>
/// <param name="MaxQuantity">The maximum quantity of items.</param>
/// <param name="Weight">The relative weight of this loot (higher = more common).</param>
public readonly record struct ThievingLoot(ItemId ItemId, int MinQuantity, int MaxQuantity, int Weight);

/// <summary>
///     A standard thieving target implementation.
/// </summary>
public class ThievingTarget : IThievingTarget
{
    public required string Name { get; init; }

    public required ThievingTargetType TargetType { get; init; }

    public required int RequiredLevel { get; init; }

    public required int Experience { get; init; }

    public required IReadOnlyList<ThievingLoot> PossibleLoot { get; init; }

    public required double BaseSuccessChance { get; init; }

    public required int FailureDamage { get; init; }
}