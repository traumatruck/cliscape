using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Burthorpe : ILocation
{
    public static LocationName Name => new("Burthorpe");

    public Shop? Shop { get; }

    public Bank? Bank { get; }
}