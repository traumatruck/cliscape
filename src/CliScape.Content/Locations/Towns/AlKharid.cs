using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class AlKharid : ILocation
{
    public static LocationName Name => new("Al Kharid");
    
    public Shop? Shop { get; }

    public Bank? Bank { get; }
}