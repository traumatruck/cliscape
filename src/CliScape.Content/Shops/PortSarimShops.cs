using CliScape.Content.Items;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
///     Shop definitions for Port Sarim.
/// </summary>
public static class PortSarimShops
{
    /// <summary>
    ///     Gerrant's Fishy Business - sells fishing supplies.
    /// </summary>
    public static Shop FishingShop { get; } = CreateFishingShop();

    /// <summary>
    ///     Port Sarim General Store.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    private static Shop CreateFishingShop()
    {
        var shop = new Shop
        {
            Name = new ShopName("Gerrant's Fishy Business"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.2,
            BuyPriceMultiplier = 0.3
        };

        shop.AddStock(Tools.SmallFishingNet, 10);
        shop.AddStock(Food.Shrimp, 10);
        shop.AddStock(Food.Trout, 5);
        shop.AddStock(Food.Salmon, 3);

        return shop;
    }

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Port Sarim General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 10);

        return shop;
    }
}
