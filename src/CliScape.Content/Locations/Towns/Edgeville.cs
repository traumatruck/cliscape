using CliScape.Content.Locations.DangerousAreas;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Edgeville : ITown
{
    public static LocationName Name => new("Edgeville");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.South, Varrock.Name },
        { Direction.West, IceMountain.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}