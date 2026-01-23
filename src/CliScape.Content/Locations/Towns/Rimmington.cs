using CliScape.Content.Locations.Dungeons;
using CliScape.Game.World;

namespace CliScape.Content.Locations.Towns;

public class Rimmington : ITown
{
    public static LocationName Name => new("Rimmington");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, PortSarim.Name },
        { Direction.South, RimmingtonDungeon.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}