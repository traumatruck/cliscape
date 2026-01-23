using CliScape.Content.Locations.Towns;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Dungeons;

public class FaladorDungeon : IDungeon
{
    public static LocationName Name => new("Falador Dungeon");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, Falador.Name }
    };
}