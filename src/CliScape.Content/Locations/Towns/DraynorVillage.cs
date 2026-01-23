using CliScape.Content.Locations.DangerousAreas;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class DraynorVillage : ITown
{
    public static LocationName Name => new("Draynor Village");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, WestLumbridge.Name },
        { Direction.North, DraynorManor.Name },
        { Direction.West, Falador.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}