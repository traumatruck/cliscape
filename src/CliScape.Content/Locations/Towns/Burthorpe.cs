using CliScape.Content.Locations.DangerousAreas;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Burthorpe : ITown
{
    public static LocationName Name => new("Burthorpe");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.South, Taverly.Name },
        { Direction.North, DeathPlateau.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}