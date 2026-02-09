using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Resources;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Catherby : ILocation
{
    public static LocationName Name => new("Catherby");

    public IReadOnlyList<Shop> Shops { get; } =
    [
        CatherbyShops.FishingShop,
        CatherbyShops.GeneralStore
    ];

    public bool HasBank => true;

    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();

    public IReadOnlyList<IFishingSpot> FishingSpots { get; } =
    [
        Resources.FishingSpots.SmallNetSpot,
        Resources.FishingSpots.CageSpot,
        Resources.FishingSpots.HarpoonSpot
    ];

    public IReadOnlyList<ITree> Trees { get; } =
    [
        Resources.Trees.NormalTree,
        Resources.Trees.MapleTree,
        Resources.Trees.YewTree
    ];
}