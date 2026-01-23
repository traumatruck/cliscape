using CliScape.Content.Locations.Towns;
using CliScape.Game.World;

namespace CliScape.Content.Locations.Dungeons;

public class BarbarianVillageDungeon : IDungeon
{
    public static LocationName Name => new("Barbarian Village Dungeon");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, BarbarianVillage.Name }
    };
}