using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Varrock : ILocation
{
    public static LocationName Name => new("Varrock");
    
    public Shop? Shop { get; }

    public Bank? Bank { get; }
}