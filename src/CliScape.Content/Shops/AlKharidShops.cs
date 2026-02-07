using CliScape.Content.Items;
using CliScape.Content.Items.Equippables;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
/// Shop definitions for Al Kharid.
/// </summary>
public static class AlKharidShops
{
    /// <summary>
    /// Al Kharid Scimitar Shop - best place for scimitars.
    /// </summary>
    public static Shop ScimitarShop { get; } = CreateScimitarShop();

    /// <summary>
    /// Al Kharid General Store.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    /// <summary>
    /// Louie's Armoured Legs - sells platelegs.
    /// </summary>
    public static Shop LouiesLegs { get; } = CreateLouiesLegs();

    private static Shop CreateScimitarShop()
    {
        var shop = new Shop
        {
            Name = new ShopName("Al Kharid Scimitar Shop"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.15,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(BronzeEquipment.Scimitar, 10);
        shop.AddStock(IronEquipment.Scimitar, 5);
        shop.AddStock(SteelEquipment.Scimitar, 3);
        shop.AddStock(MithrilEquipment.Scimitar, 1);

        return shop;
    }

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Al Kharid General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 10);
        shop.AddStock(Food.Shrimp, 5);

        return shop;
    }

    private static Shop CreateLouiesLegs()
    {
        var shop = new Shop
        {
            Name = new ShopName("Louie's Armoured Legs"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.3,
            BuyPriceMultiplier = 0.3
        };

        shop.AddStock(BronzeEquipment.Platelegs, 5);
        shop.AddStock(IronEquipment.Platelegs, 3);
        shop.AddStock(SteelEquipment.Platelegs, 2);
        shop.AddStock(MithrilEquipment.Platelegs, 1);

        return shop;
    }
}
