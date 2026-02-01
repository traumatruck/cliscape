using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Resources;

namespace CliScape.Content.Locations.Towns;

public class DraynorVillage : ILocation
{
    public static LocationName Name => new("Draynor Village");
    
    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();

    public IReadOnlyList<IFishingSpot> FishingSpots { get; } =
    [
        Resources.FishingSpots.SmallNetSpot
    ];

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.WillowTree
    ];
}