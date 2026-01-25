using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Catherby : ILocation
{
    public static LocationName Name => new("Catherby");
    
    public Shop? Shop { get; }
    
    public Bank? Bank { get; }
}