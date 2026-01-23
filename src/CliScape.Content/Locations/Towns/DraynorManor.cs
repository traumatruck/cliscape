using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class DraynorManor : ITown
{
    public static LocationName Name => new("Draynor Manor");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.South, DraynorVillage.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}