using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class DraynorVillage : ILocation
{
    public static LocationName Name => new("Draynor Village");
    
    public Shop? Shop { get; }
    
    public Bank? Bank { get; }
}