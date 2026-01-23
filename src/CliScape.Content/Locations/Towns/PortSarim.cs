using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class PortSarim : ITown
{
    public static LocationName Name => new("Port Sarim");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.North, Falador.Name },
        { Direction.West, Rimmington.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}