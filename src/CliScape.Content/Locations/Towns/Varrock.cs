using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Varrock : ILocation
{
    public static LocationName Name => new("Varrock");
    
    public IReadOnlyList<Shop> Shops { get; } =
    [
        VarrockShops.SwordShop,
        VarrockShops.ThessaliasClothes,
        VarrockShops.ZaffsStaves,
        VarrockShops.GeneralStore
    ];

    public Bank? Bank { get; }
    
    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();
}