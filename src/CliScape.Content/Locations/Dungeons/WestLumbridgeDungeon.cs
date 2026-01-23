using CliScape.Content.Locations.DangerousAreas;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Dungeons;

public class WestLumbridgeDungeon : IDungeon
{
    public static LocationName Name => new("West Lumbridge Dungeon");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.South, WestLumbridge.Name }
    };
}