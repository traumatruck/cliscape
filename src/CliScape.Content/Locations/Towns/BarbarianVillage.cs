using CliScape.Content.Locations.Dungeons;
using CliScape.Game.World;

namespace CliScape.Content.Locations.Towns;

public class BarbarianVillage : ITown
{
    public static LocationName Name => new("Barbarian Village");

    public IDictionary<Direction, LocationName> AdjacentLocations => new Dictionary<Direction, LocationName>
    {
        { Direction.East, Varrock.Name },
        { Direction.West, BarbarianVillageDungeon.Name }
    };

    public Shop? Shop { get; }
    public Bank? Bank { get; }
}