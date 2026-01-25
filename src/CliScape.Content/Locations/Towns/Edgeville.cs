using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Edgeville : ILocation
{
    public static LocationName Name => new("Edgeville");
    
    public Shop? Shop { get; }
    
    public Bank? Bank { get; }
}