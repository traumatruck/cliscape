using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Falador : ILocation
{
    public static LocationName Name => new("Falador");
    
    public Shop? Shop { get; }
    
    public Bank? Bank { get; }
}