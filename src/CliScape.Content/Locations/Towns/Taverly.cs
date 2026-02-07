using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Npcs;
using CliScape.Content.Resources;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Taverly : ILocation
{
    public static LocationName Name => new("Taverly");

    public IReadOnlyList<Shop> Shops { get; } =
    [
        TaverlyShops.GeneralStore
    ];

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        LesserDemon.Instance
    };

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.NormalTree,
        Resources.Trees.OakTree,
        Resources.Trees.MagicTree
    ];

    public IReadOnlyList<IThievingTarget> ThievingTargets { get; } =
    [
        Resources.ThievingTargets.Man,
        Resources.ThievingTargets.BakeryStall
    ];
}