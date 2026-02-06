using CliScape.Content.Items;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
///     Shop definitions for Catherby.
/// </summary>
public static class CatherbyShops
{
    /// <summary>
    ///     Harry's Fishing Shop - sells fishing supplies.
    /// </summary>
    public static Shop FishingShop { get; } = CreateFishingShop();

    /// <summary>
    ///     Catherby General Store.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    private static Shop CreateFishingShop()
    {
        var shop = new Shop
        {
            Name = new ShopName("Harry's Fishing Shop"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.2,
            BuyPriceMultiplier = 0.3
        };

        shop.AddStock(Tools.SmallFishingNet, 10);
        shop.AddStock(Food.Lobster, 5);
        shop.AddStock(CookedFood.Swordfish, 3);

        return shop;
    }

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Catherby General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 10);

        return shop;
    }
}
