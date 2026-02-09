using CliScape.Content.Npcs;
using CliScape.Content.Shops;
using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;

namespace CliScape.Content.Locations.Towns;

public class Edgeville : ILocation
{
    public static LocationName Name => new("Edgeville");

    public IReadOnlyList<Shop> Shops { get; } =
    [
        EdgevilleShops.GeneralStore
    ];

    public bool HasBank => true;

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Man.Instance,
        HillGiant.Instance
    };

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.NormalTree,
        Resources.Trees.YewTree
    ];

    public IReadOnlyList<IMiningRock> MiningRocks { get; } =
    [
        Resources.MiningRocks.IronRock,
        Resources.MiningRocks.CoalRock,
        Resources.MiningRocks.AdamantiteRock
    ];

    public IReadOnlyList<IThievingTarget> ThievingTargets { get; } =
    [
        Resources.ThievingTargets.Man
    ];
}