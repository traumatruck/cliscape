using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents a fishing spot in the world where players can catch fish.
/// </summary>
public interface IFishingSpot
{
    /// <summary>
    ///     The display name of this fishing spot.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The type of fishing spot (e.g., "Net", "Lure", "Cage", "Harpoon").
    /// </summary>
    FishingSpotType SpotType { get; }

    /// <summary>
    ///     The fish that can be caught at this spot.
    /// </summary>
    IReadOnlyList<FishingYield> PossibleCatches { get; }

    /// <summary>
    ///     The minimum fishing level required to use this spot.
    /// </summary>
    int RequiredLevel { get; }

    /// <summary>
    ///     The item ID of the tool required to fish at this spot (e.g., small fishing net, fishing rod).
    /// </summary>
    ItemId RequiredTool { get; }

    /// <summary>
    ///     The maximum number of fish that can be caught in a single action batch.
    ///     Represents how productive this fishing spot is.
    /// </summary>
    int MaxActions { get; }
}

/// <summary>
///     Represents a fish that can be caught at a fishing spot.
/// </summary>
/// <param name="FishItemId">The item ID of the raw fish.</param>
/// <param name="RequiredLevel">The fishing level required to catch this fish.</param>
/// <param name="Experience">The fishing experience gained when catching this fish.</param>
public readonly record struct FishingYield(ItemId FishItemId, int RequiredLevel, int Experience);

/// <summary>
///     The type of fishing method at a spot.
/// </summary>
public enum FishingSpotType
{
    /// <summary>
    ///     Small net fishing for shrimp and anchovies.
    /// </summary>
    SmallNet,

    /// <summary>
    ///     Bait fishing for sardines and herring.
    /// </summary>
    Bait,

    /// <summary>
    ///     Lure/fly fishing for trout and salmon.
    /// </summary>
    Lure,

    /// <summary>
    ///     Cage fishing for lobsters.
    /// </summary>
    Cage,

    /// <summary>
    ///     Harpoon fishing for tuna and swordfish.
    /// </summary>
    Harpoon
}