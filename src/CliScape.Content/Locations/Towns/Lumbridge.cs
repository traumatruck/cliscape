using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Content.Npcs;

namespace CliScape.Content.Locations.Towns;

public class Lumbridge : ILocation
{
    public static LocationName Name => new("Lumbridge");

    public Shop? Shop { get; }

    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Cow.Instance
    };
}