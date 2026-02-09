using CliScape.Content.Npcs;
using CliScape.Content.Shops;
using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;

namespace CliScape.Content.Locations.Towns;

public class PortSarim : ILocation
{
    public static LocationName Name => new("Port Sarim");

    public IReadOnlyList<Shop> Shops { get; } =
    [
        PortSarimShops.FishingShop,
        PortSarimShops.GeneralStore
    ];

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Rat.Instance,
        Mugger.Instance,
        Hobgoblin.Instance
    };

    public IReadOnlyList<IFishingSpot> FishingSpots { get; } =
    [
        Resources.FishingSpots.SmallNetSpot,
        Resources.FishingSpots.HarpoonSpot
    ];

    public IReadOnlyList<IThievingTarget> ThievingTargets { get; } =
    [
        Resources.ThievingTargets.Man
    ];
}