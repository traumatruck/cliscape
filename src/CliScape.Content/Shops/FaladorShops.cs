using CliScape.Content.Items;
using CliScape.Content.Items.Equippables;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
/// Shop definitions for Falador.
/// </summary>
public static class FaladorShops
{
    /// <summary>
    /// Falador Shield Shop - specialty shop for shields.
    /// </summary>
    public static Shop ShieldShop { get; } = CreateShieldShop();

    /// <summary>
    /// Wayne's Chains - sells chainmail armor.
    /// </summary>
    public static Shop WaynesChains { get; } = CreateWaynesChains();

    /// <summary>
    /// Falador General Store.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    private static Shop CreateShieldShop()
    {
        var shop = new Shop
        {
            Name = new ShopName("Falador Shield Shop"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.2,
            BuyPriceMultiplier = 0.35
        };

        shop.AddStock(MiscEquipment.WoodenShield, 10);
        shop.AddStock(BronzeEquipment.SqShield, 5);
        shop.AddStock(BronzeEquipment.Kiteshield, 3);
        shop.AddStock(IronEquipment.Kiteshield, 2);
        shop.AddStock(SteelEquipment.Kiteshield, 1);

        return shop;
    }

    private static Shop CreateWaynesChains()
    {
        var shop = new Shop
        {
            Name = new ShopName("Wayne's Chains"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.25,
            BuyPriceMultiplier = 0.3
        };

        shop.AddStock(BronzeEquipment.Chainbody, 5);
        shop.AddStock(IronEquipment.Chainbody, 3);
        shop.AddStock(SteelEquipment.Chainbody, 2);

        return shop;
    }

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Falador General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 20);

        return shop;
    }
}
