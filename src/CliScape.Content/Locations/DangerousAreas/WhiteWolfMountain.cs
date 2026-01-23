using CliScape.Content.Locations.Towns;
using CliScape.Core.World;

namespace CliScape.Content.Locations.DangerousAreas;

public class WhiteWolfMountain : IDangerousArea
{
    public static LocationName Name => new("White Wolf Mountain");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, Taverly.Name },
        { Direction.West, Catherby.Name }
    };
}