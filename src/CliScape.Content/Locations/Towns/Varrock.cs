using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Npcs;
using CliScape.Content.Resources;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Varrock : ILocation
{
    public static LocationName Name => new("Varrock");
    
    public IReadOnlyList<Shop> Shops { get; } =
    [
        VarrockShops.SwordShop,
        VarrockShops.ThessaliasClothes,
        VarrockShops.ZaffsStaves,
        VarrockShops.GeneralStore
    ];

    public bool HasBank => true;

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Man.Instance,
        Guard.Instance,
        Imp.Instance
    };

    public IReadOnlyList<IMiningRock> MiningRocks { get; } =
    [
        Resources.MiningRocks.CopperRock,
        Resources.MiningRocks.TinRock,
        Resources.MiningRocks.IronRock
    ];

    public IReadOnlyList<IFurnace> Furnaces { get; } =
    [
        Resources.Furnaces.VarrockFurnace
    ];

    public IReadOnlyList<IAnvil> Anvils { get; } =
    [
        Resources.Anvils.VarrockAnvil
    ];

    public IReadOnlyList<IThievingTarget> ThievingTargets { get; } =
    [
        Resources.ThievingTargets.Man,
        Resources.ThievingTargets.BakeryStall,
        Resources.ThievingTargets.TeaStall,
        Resources.ThievingTargets.SilkStall,
        Resources.ThievingTargets.FurStall
    ];
}