using CliScape.Content.Items;
using CliScape.Core.World;

namespace CliScape.Content.Shops;

/// <summary>
///     Shop definitions for Taverly.
/// </summary>
public static class TaverlyShops
{
    /// <summary>
    ///     Taverly General Store.
    /// </summary>
    public static Shop GeneralStore { get; } = CreateGeneralStore();

    private static Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("Taverly General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.5,
            BuyPriceMultiplier = 0.4
        };

        shop.AddStock(Food.Bread, 10);
        shop.AddStock(Tools.Tinderbox, 5);

        return shop;
    }
}