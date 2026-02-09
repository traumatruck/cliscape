using CliScape.Content.Items;
using CliScape.Content.Items.Equippables;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
///     Shop definitions for Edgeville.
/// </summary>
public static class EdgevilleShops
{
    /// <summary>
    ///     Edgeville General Store - supplies for adventurers near the wilderness.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Edgeville General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 15);
        shop.AddStock(Food.Lobster, 5);
        shop.AddStock(Tools.Tinderbox, 5);
        shop.AddStock(MiscEquipment.WoodenShield, 3);

        return shop;
    }
}
