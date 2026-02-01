using CliScape.Content.Items;
using CliScape.Core.World.Resources;

namespace CliScape.Content.Resources;

/// <summary>
///     Fishing spot definitions for the game world.
/// </summary>
public static class FishingSpots
{
    /// <summary>
    ///     Small net fishing spot for shrimp and anchovies.
    ///     Found in Lumbridge, Draynor, and other coastal areas.
    /// </summary>
    public static readonly FishingSpot SmallNetSpot = new()
    {
        Name = "Net fishing spot",
        SpotType = FishingSpotType.SmallNet,
        RequiredLevel = 1,
        RequiredTool = ItemIds.SmallFishingNet,
        PossibleCatches =
        [
            new FishingYield(ItemIds.RawShrimps, 1, 10),
            new FishingYield(ItemIds.RawAnchovies, 15, 40)
        ],
        MaxActions = 10
    };

    /// <summary>
    ///     Lure/fly fishing spot for trout and salmon.
    ///     Found in Lumbridge and Barbarian Village.
    /// </summary>
    public static readonly FishingSpot LureSpot = new()
    {
        Name = "Lure fishing spot",
        SpotType = FishingSpotType.Lure,
        RequiredLevel = 20,
        RequiredTool = ItemIds.SmallFishingNet, // TODO: Add fly fishing rod item
        PossibleCatches =
        [
            new FishingYield(ItemIds.RawTrout, 20, 50),
            new FishingYield(ItemIds.RawSalmon, 30, 70)
        ],
        MaxActions = 12
    };

    /// <summary>
    ///     Cage fishing spot for lobsters.
    ///     Found in Karamja and other fishing platforms.
    /// </summary>
    public static readonly FishingSpot CageSpot = new()
    {
        Name = "Cage fishing spot",
        SpotType = FishingSpotType.Cage,
        RequiredLevel = 40,
        RequiredTool = ItemIds.SmallFishingNet, // TODO: Add lobster pot item
        PossibleCatches =
        [
            new FishingYield(ItemIds.RawLobster, 40, 90)
        ],
        MaxActions = 15
    };

    /// <summary>
    ///     Harpoon fishing spot for tuna and swordfish.
    ///     Found in Karamja and the Fishing Guild.
    /// </summary>
    public static readonly FishingSpot HarpoonSpot = new()
    {
        Name = "Harpoon fishing spot",
        SpotType = FishingSpotType.Harpoon,
        RequiredLevel = 35,
        RequiredTool = ItemIds.SmallFishingNet, // TODO: Add harpoon item
        PossibleCatches =
        [
            new FishingYield(ItemIds.RawTuna, 35, 80),
            new FishingYield(ItemIds.RawSwordfish, 50, 100)
        ],
        MaxActions = 15
    };
}