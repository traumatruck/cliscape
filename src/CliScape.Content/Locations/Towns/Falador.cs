using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Falador : ILocation
{
    public static LocationName Name => new("Falador");
    
    public IReadOnlyList<Shop> Shops { get; } =
    [
        FaladorShops.ShieldShop,
        FaladorShops.WaynesChains,
        FaladorShops.GeneralStore
    ];
    
    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();
}