using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class AlKharid : ITown
{
    public static LocationName Name => new("Al Kharid");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.West, Lumbridge.Name }
    };

    public Shop? Shop { get; }

    public Bank? Bank { get; }
}