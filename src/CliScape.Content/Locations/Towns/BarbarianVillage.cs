using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class BarbarianVillage : ILocation
{
    public static LocationName Name => new("Barbarian Village");

    public Shop? Shop { get; }

    public Bank? Bank { get; }
}