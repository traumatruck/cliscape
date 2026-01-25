using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class Lumbridge : ILocation
{
    public static LocationName Name => new("Lumbridge");

    public Shop? Shop { get; }

    public Bank? Bank { get; }
}