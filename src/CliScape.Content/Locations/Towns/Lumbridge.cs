using CliScape.Content.Locations.DangerousAreas;
using CliScape.Game.World;

namespace CliScape.Content.Locations.Towns;

public class Lumbridge : ITown
{
    public static LocationName Name => new("Lumbridge");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.North, Varrock.Name },
        { Direction.East, AlKharid.Name },
        { Direction.South, LumbridgeSwamp.Name },
        { Direction.West, WestLumbridge.Name }
    };

    public Shop? Shop { get; }

    public Bank? Bank { get; }
}