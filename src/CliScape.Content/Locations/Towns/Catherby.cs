using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Resources;

namespace CliScape.Content.Locations.Towns;

public class Catherby : ILocation
{
    public static LocationName Name => new("Catherby");
    
    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();

    public IReadOnlyList<IFishingSpot> FishingSpots { get; } =
    [
        Resources.FishingSpots.SmallNetSpot,
        Resources.FishingSpots.CageSpot,
        Resources.FishingSpots.HarpoonSpot
    ];

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.NormalTree,
        Resources.Trees.YewTree
    ];
}