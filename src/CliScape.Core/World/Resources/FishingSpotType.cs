namespace CliScape.Core.World.Resources;

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