using CliScape.Content.Locations.DangerousAreas;
using CliScape.Game.World;

namespace CliScape.Content.Locations.Dungeons;

public class IceMountainDungeon : IDungeon
{
    public static LocationName Name => new("Ice Mountain Dungeon");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, IceMountain.Name }
    };
}