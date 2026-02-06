using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Npcs;
using CliScape.Content.Resources;

namespace CliScape.Content.Locations.Towns;

public class BarbarianVillage : ILocation
{
    public static LocationName Name => new("Barbarian Village");

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Hobgoblin.Instance
    };

    public IReadOnlyList<IFishingSpot> FishingSpots { get; } =
    [
        Resources.FishingSpots.LureSpot
    ];

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.NormalTree,
        Resources.Trees.WillowTree,
        Resources.Trees.MapleTree
    ];
}