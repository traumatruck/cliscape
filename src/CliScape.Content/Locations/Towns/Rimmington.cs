using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Npcs;
using CliScape.Content.Resources;

namespace CliScape.Content.Locations.Towns;

public class Rimmington : ILocation
{
    public static LocationName Name => new("Rimmington");

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Imp.Instance
    };

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.NormalTree,
        Resources.Trees.OakTree
    ];

    public IReadOnlyList<IMiningRock> MiningRocks { get; } =
    [
        Resources.MiningRocks.CopperRock,
        Resources.MiningRocks.TinRock,
        Resources.MiningRocks.IronRock,
        Resources.MiningRocks.GoldRock
    ];
}