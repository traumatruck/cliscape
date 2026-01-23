using CliScape.Content.Locations.Dungeons;
using CliScape.Content.Locations.Towns;
using CliScape.Game.World;

namespace CliScape.Content.Locations.DangerousAreas;

public class WestLumbridge : IDangerousArea
{
    public static LocationName Name => new("West Lumbridge");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, Lumbridge.Name },
        { Direction.West, DraynorVillage.Name },
        { Direction.North, WestLumbridgeDungeon.Name }
    };
}