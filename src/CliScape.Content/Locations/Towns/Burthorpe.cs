using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Core.World.Resources;
using CliScape.Content.Npcs;
using CliScape.Content.Resources;
using CliScape.Content.Shops;

namespace CliScape.Content.Locations.Towns;

public class Burthorpe : ILocation
{
    public static LocationName Name => new("Burthorpe");

    public IReadOnlyList<Shop> Shops { get; } =
    [
        BurthorpeShops.SlayerShop,
        BurthorpeShops.GeneralStore
    ];

    public bool HasBank => true;

    public IReadOnlyList<INpc> AvailableNpcs { get; } = new INpc[]
    {
        Guard.Instance,
        MossGiant.Instance
    };

    public IReadOnlyList<IMiningRock> MiningRocks { get; } =
    [
        Resources.MiningRocks.RuniteRock
    ];
}