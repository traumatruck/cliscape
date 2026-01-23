using CliScape.Content.Locations.Dungeons;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Falador : ITown
{
    public static LocationName Name => new("Falador");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, DraynorVillage.Name },
        { Direction.North, Taverly.Name },
        { Direction.South, PortSarim.Name },
        { Direction.West, FaladorDungeon.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}