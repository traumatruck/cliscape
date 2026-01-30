using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Content.Npcs;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Lumbridge : ILocation
{
    public static LocationName Name => new("Lumbridge");

    public IReadOnlyList<Shop> Shops { get; } =
    [
        LumbridgeShops.BobsAxes,
        LumbridgeShops.GeneralStore
    ];

    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Cow.Instance,
        Chicken.Instance,
        Goblin.Instance
    };
}