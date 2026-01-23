using CliScape.Content.Locations.Towns;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Dungeons;

public class RimmingtonDungeon : IDungeon
{
    public static LocationName Name => new("Rimmington Dungeon");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.North, Rimmington.Name }
    };
}