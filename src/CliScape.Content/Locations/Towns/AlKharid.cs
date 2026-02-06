using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Resources;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class AlKharid : ILocation
{
    public static LocationName Name => new("Al Kharid");
    
    public IReadOnlyList<Shop> Shops { get; } =
    [
        AlKharidShops.ScimitarShop,
        AlKharidShops.LouiesLegs,
        AlKharidShops.GeneralStore
    ];

    public bool HasBank => true;

    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();

    public IReadOnlyList<IMiningRock> MiningRocks { get; } =
    [
        Resources.MiningRocks.CopperRock,
        Resources.MiningRocks.TinRock,
        Resources.MiningRocks.IronRock,
        Resources.MiningRocks.CoalRock
    ];

    public IReadOnlyList<IFurnace> Furnaces { get; } =
    [
        Resources.Furnaces.AlKharidFurnace
    ];

    public IReadOnlyList<ICookingRange> CookingRanges { get; } =
    [
        Resources.CookingRanges.StandardRange
    ];

    public IReadOnlyList<IThievingTarget> ThievingTargets { get; } =
    [
        Resources.ThievingTargets.Man,
        Resources.ThievingTargets.SilkStall
    ];
}