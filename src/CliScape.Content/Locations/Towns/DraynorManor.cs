using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class DraynorManor : ILocation
{
    public static LocationName Name => new("Draynor Manor");

    public Shop? Shop { get; }
    
    public Bank? Bank { get; }
}