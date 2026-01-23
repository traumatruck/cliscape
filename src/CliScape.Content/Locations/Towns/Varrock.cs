using CliScape.Game.World;

namespace CliScape.Content.Locations.Towns;

public class Varrock : ITown
{
    public static LocationName Name => new("Varrock");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.South, Lumbridge.Name },
        { Direction.North, Edgeville.Name },
        { Direction.West, BarbarianVillage.Name }
    };

    public Shop? Shop { get; }

    public Bank? Bank { get; }
}