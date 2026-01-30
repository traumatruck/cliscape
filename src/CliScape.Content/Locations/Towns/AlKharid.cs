using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class AlKharid : ILocation
{
    public static LocationName Name => new("Al Kharid");
    
    public IReadOnlyList<Shop> Shops { get; } =
    [
        AlKharidShops.ScimitarShop,
        AlKharidShops.LouiesLegs,
        AlKharidShops.GeneralStore
    ];

    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();
}