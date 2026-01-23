using CliScape.Content.Locations.DangerousAreas;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Catherby : ITown
{
    public static LocationName Name => new("Catherby");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, WhiteWolfMountain.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}