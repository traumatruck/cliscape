using CliScape.Content.Items;
using CliScape.Content.Items.Equippables;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
///     Shop definitions for Varrock.
/// </summary>
public static class VarrockShops
{
    /// <summary>
    ///     Zaff's Superior Staves - sells staves and magic equipment.
    ///     (Currently sells melee gear until magic is implemented)
    /// </summary>
    public static Shop ZaffsStaves { get; } = CreateZaffsStaves();

    /// <summary>
    ///     Varrock Sword Shop - Specialty sword shop.
    /// </summary>
    public static Shop SwordShop { get; } = CreateSwordShop();

    /// <summary>
    ///     Thessalia's Fine Clothes - sells basic armor.
    /// </summary>
    public static Shop ThessaliasClothes { get; } = CreateThessaliasClothes();

    /// <summary>
    ///     Varrock General Store.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    private static Shop CreateZaffsStaves()
    {
        var shop = new Shop
        {
            Name = new ShopName("Zaff's Superior Staves"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.3,
            BuyPriceMultiplier = 0.25
        };

        // Placeholder items until magic staves are added
        shop.AddStock(BronzeEquipment.Mace, 5);
        shop.AddStock(IronEquipment.Sword, 3);

        return shop;
    }

    private static Shop CreateSwordShop()
    {
        var shop = new Shop
        {
            Name = new ShopName("Varrock Sword Shop"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.2,
            BuyPriceMultiplier = 0.35
        };

        shop.AddStock(BronzeEquipment.Sword, 10);
        shop.AddStock(BronzeEquipment.Scimitar, 5);
        shop.AddStock(IronEquipment.Sword, 5);
        shop.AddStock(IronEquipment.Scimitar, 3);
        shop.AddStock(SteelEquipment.Sword, 3);
        shop.AddStock(SteelEquipment.Scimitar, 2);
        shop.AddStock(MithrilEquipment.Sword, 1);
        shop.AddStock(MithrilEquipment.Scimitar, 1);

        return shop;
    }

    private static Shop CreateThessaliasClothes()
    {
        var shop = new Shop
        {
            Name = new ShopName("Thessalia's Fine Clothes"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.3,
            BuyPriceMultiplier = 0.3
        };

        shop.AddStock(LeatherEquipment.Body, 10);
        shop.AddStock(LeatherEquipment.Chaps, 10);
        shop.AddStock(LeatherEquipment.Gloves, 10);
        shop.AddStock(LeatherEquipment.Boots, 10);

        return shop;
    }

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Varrock General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 15);
        shop.AddStock(MiscEquipment.WoodenShield, 3);

        return shop;
    }
}