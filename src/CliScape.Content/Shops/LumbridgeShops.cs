using CliScape.Content.Items;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
/// Shop definitions for Lumbridge.
/// </summary>
public static class LumbridgeShops
{
    /// <summary>
    /// Bob's Brilliant Axes - sells axes and basic equipment.
    /// </summary>
    public static Shop BobsAxes { get; } = CreateBobsAxes();

    /// <summary>
    /// Lumbridge General Store - buys and sells various goods.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    private static Shop CreateBobsAxes()
    {
        var shop = new Shop
        {
            Name = new ShopName("Bob's Brilliant Axes"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.3,
            BuyPriceMultiplier = 0.3
        };

        shop.AddStock(BronzeEquipment.Axe, 5);
        shop.AddStock(IronEquipment.Sword, 3);
        shop.AddStock(SteelEquipment.Axe, 2);
        shop.AddStock(BronzeEquipment.Sword, 5);
        shop.AddStock(BronzeEquipment.Dagger, 5);

        return shop;
    }

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Lumbridge General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 10);
        shop.AddStock(MiscEquipment.WoodenShield, 5);

        return shop;
    }
}
