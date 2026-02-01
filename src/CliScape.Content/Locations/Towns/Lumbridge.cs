using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Npcs;
using CliScape.Content.Resources;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Lumbridge : ILocation
{
    public static LocationName Name => new("Lumbridge");

    public IReadOnlyList<Shop> Shops { get; } =
    [
        LumbridgeShops.BobsAxes,
        LumbridgeShops.GeneralStore
    ];

    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Cow.Instance,
        Chicken.Instance,
        Goblin.Instance
    };

    public IReadOnlyList<IFishingSpot> FishingSpots { get; } =
    [
        Resources.FishingSpots.SmallNetSpot,
        Resources.FishingSpots.LureSpot
    ];

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.NormalTree,
        Resources.Trees.OakTree
    ];

    public IReadOnlyList<IMiningRock> MiningRocks { get; } =
    [
        Resources.MiningRocks.CopperRock,
        Resources.MiningRocks.TinRock
    ];

    public IReadOnlyList<IFurnace> Furnaces { get; } =
    [
        Resources.Furnaces.LumbridgeFurnace
    ];

    public IReadOnlyList<IAnvil> Anvils { get; } =
    [
        Resources.Anvils.LumbridgeAnvil
    ];

    public IReadOnlyList<ICookingRange> CookingRanges { get; } =
    [
        Resources.CookingRanges.LumbridgeRange
    ];

    public IReadOnlyList<IThievingTarget> ThievingTargets { get; } =
    [
        Resources.ThievingTargets.Man,
        Resources.ThievingTargets.Farmer
    ];
}