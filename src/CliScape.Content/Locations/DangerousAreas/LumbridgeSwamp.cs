using CliScape.Content.Locations.Dungeons;
using CliScape.Content.Locations.Towns;
using CliScape.Core.World;

namespace CliScape.Content.Locations.DangerousAreas;

public class LumbridgeSwamp : IDangerousArea
{
    public static LocationName Name => new("Lumbridge Swamp");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.North, Lumbridge.Name },
        { Direction.South, LumbridgeSwampDungeon.Name }
    };
}