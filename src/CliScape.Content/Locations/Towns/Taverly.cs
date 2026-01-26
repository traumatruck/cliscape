using CliScape.Core.Npcs;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Taverly : ILocation
{
    public static LocationName Name => new("Taverly");
    
    public Shop? Shop { get; }
    
    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();
}