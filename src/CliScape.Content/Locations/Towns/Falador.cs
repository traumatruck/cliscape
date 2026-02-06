using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Npcs;
using CliScape.Content.Resources;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Falador : ILocation
{
    public static LocationName Name => new("Falador");
    
    public IReadOnlyList<Shop> Shops { get; } =
    [
        FaladorShops.ShieldShop,
        FaladorShops.WaynesChains,
        FaladorShops.GeneralStore
    ];

    public bool HasBank => true;

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Guard.Instance,
        Imp.Instance
    };

    public IReadOnlyList<IMiningRock> MiningRocks { get; } =
    [
        Resources.MiningRocks.CopperRock,
        Resources.MiningRocks.TinRock,
        Resources.MiningRocks.IronRock,
        Resources.MiningRocks.CoalRock,
        Resources.MiningRocks.MithrilRock
    ];

    public IReadOnlyList<IFurnace> Furnaces { get; } =
    [
        Resources.Furnaces.FaladorFurnace
    ];

    public IReadOnlyList<IAnvil> Anvils { get; } =
    [
        Resources.Anvils.FaladorAnvil
    ];

    public IReadOnlyList<IThievingTarget> ThievingTargets { get; } =
    [
        Resources.ThievingTargets.Man,
        Resources.ThievingTargets.Guard,
        Resources.ThievingTargets.SilverStall
    ];
}