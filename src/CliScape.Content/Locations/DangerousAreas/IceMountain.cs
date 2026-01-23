using CliScape.Content.Locations.Dungeons;
using CliScape.Content.Locations.Towns;
using CliScape.Game.World;

namespace CliScape.Content.Locations.DangerousAreas;

public class IceMountain : IDangerousArea
{
    public static LocationName Name => new("Ice Mountain");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, Edgeville.Name },
        { Direction.West, IceMountainDungeon.Name }
    };
}