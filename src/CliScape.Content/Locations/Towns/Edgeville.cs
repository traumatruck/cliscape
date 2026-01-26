using CliScape.Core.Npcs;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Edgeville : ILocation
{
    public static LocationName Name => new("Edgeville");
    
    public Shop? Shop { get; }
    
    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();
}