using CliScape.Content.Items;
using CliScape.Content.Items.Equippables;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
///     Shop definitions for Burthorpe.
/// </summary>
public static class BurthorpeShops
{
    /// <summary>
    ///     Turael's Slayer Supplies - equipment for slayer tasks.
    /// </summary>
    public static Shop SlayerShop { get; } = CreateSlayerShop();

    /// <summary>
    ///     Burthorpe General Store.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    private static Shop CreateSlayerShop()
    {
        var shop = new Shop
        {
            Name = new ShopName("Turael's Slayer Supplies"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.3,
            BuyPriceMultiplier = 0.25
        };

        shop.AddStock(IronEquipment.Scimitar, 5);
        shop.AddStock(SteelEquipment.Scimitar, 3);
        shop.AddStock(Food.Trout, 10);
        shop.AddStock(Food.Lobster, 5);

        return shop;
    }

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Burthorpe General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 10);
        shop.AddStock(MiscEquipment.WoodenShield, 3);

        return shop;
    }
}
