using CliScape.Content.Npcs;
using CliScape.Content.Shops;
using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;

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

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Scorpion.Instance
    };

    public IReadOnlyList<IMiningRock> MiningRocks { get; } =
    [
        Resources.MiningRocks.CopperRock,
        Resources.MiningRocks.TinRock,
        Resources.MiningRocks.IronRock,
        Resources.MiningRocks.SilverRock,
        Resources.MiningRocks.CoalRock,
        Resources.MiningRocks.GoldRock
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
        Resources.ThievingTargets.SilkStall,
        Resources.ThievingTargets.GemStall
    ];
}