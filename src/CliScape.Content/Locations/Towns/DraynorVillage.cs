using CliScape.Core.Npcs;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class DraynorVillage : ILocation
{
    public static LocationName Name => new("Draynor Village");
    
    public Shop? Shop { get; }
    
    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();
}