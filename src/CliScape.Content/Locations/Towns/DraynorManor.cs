using CliScape.Content.Npcs;
using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;

namespace CliScape.Content.Locations.Towns;

public class DraynorManor : ILocation
{
    public static LocationName Name => new("Draynor Manor");

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Spider.Instance,
        GiantSpider.Instance,
        Skeleton.Instance,
        Zombie.Instance
    };

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.NormalTree,
        Resources.Trees.OakTree
    ];
}