using CliScape.Content.Locations.Towns;
using CliScape.Core.World;

namespace CliScape.Content.Locations.DangerousAreas;

public class DeathPlateau : IDangerousArea
{
    public static LocationName Name => new("Death Plateau");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.South, Burthorpe.Name }
    };
}