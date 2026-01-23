using CliScape.Content.Locations.DangerousAreas;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Taverly : ITown
{
    public static LocationName Name => new("Taverly");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.South, Falador.Name },
        { Direction.North, Burthorpe.Name },
        { Direction.West, WhiteWolfMountain.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}