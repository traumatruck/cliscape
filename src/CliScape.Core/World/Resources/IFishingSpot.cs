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
