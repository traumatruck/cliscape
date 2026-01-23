using CliScape.Content.Locations.DangerousAreas;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Dungeons;

public class LumbridgeSwampDungeon : IDungeon
{
    public static LocationName Name => new("Lumbridge Swamp Dungeon");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.North, LumbridgeSwamp.Name }
    };
}