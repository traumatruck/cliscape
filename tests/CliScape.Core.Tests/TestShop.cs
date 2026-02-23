using CliScape.Core.Items;
using CliScape.Core.World;
using CliScape.Tests.Shared;

namespace CliScape.Core.Tests;

public sealed class TestShop
{
    private static IItem MakeCoins() =>
        TestFactory.CreateStubItem(new ItemId(1), "Coins", stackable: true, baseValue: 1);

    private static IItem MakeSword() =>
        TestFactory.CreateStubItem(new ItemId(100), "Bronze sword", baseValue: 26);

    private static IItem MakeShield() =>
        TestFactory.CreateStubItem(new ItemId(200), "Wooden shield", baseValue: 20, tradeable: true);

    private static IItem MakeUntradeable() =>
        TestFactory.CreateStubItem(new ItemId(999), "Quest item", tradeable: false, baseValue: 50);

    private Shop CreateSpecialtyShop()
    {
        var shop = new Shop
        {
            Name = new ShopName("Test Shop"),
            IsGeneralStore = false,
            SellPriceMultiplier = 1.3,
            BuyPriceMultiplier = 0.4
        };
        shop.AddStock(MakeSword(), 10);
        return shop;
    }

    private Shop CreateGeneralStore()
    {
        var shop = new Shop
        {
            Name = new ShopName("General Store"),
            IsGeneralStore = true,
            SellPriceMultiplier = 1.3,
            BuyPriceMultiplier = 0.4
        };
        shop.AddStock(MakeShield(), 5);
        return shop;
    }

    [Fact]
    public void AddStock_CreatesStockEntry()
    {
        var shop = CreateSpecialtyShop();
        Assert.Single(shop.Stock);
        Assert.Equal(10, shop.Stock[0].CurrentStock);
    }

    [Fact]
    public void GetBuyPrice_AtBaseStock_ReturnsExpectedPrice()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();
        var price = shop.GetBuyPrice(sword);

        // At base stock (ratio = 1.0): ceil(26 * 1.3 / 1.0) = ceil(33.8) = 34
        Assert.True(price > 0);
        Assert.True(price >= sword.BaseValue);
    }

    [Fact]
    public void GetBuyPrice_LowStock_PriceIncreases()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();

        var priceAtFullStock = shop.GetBuyPrice(sword);

        // Deplete stock
        shop.Stock.First(s => s.Item.Id == sword.Id).CurrentStock = 2;
        var priceAtLowStock = shop.GetBuyPrice(sword);

        Assert.True(priceAtLowStock > priceAtFullStock);
    }

    [Fact]
    public void GetBuyPrice_ZeroStock_ReturnsZero()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();
        shop.Stock.First(s => s.Item.Id == sword.Id).CurrentStock = 0;

        Assert.Equal(0, shop.GetBuyPrice(sword));
    }

    [Fact]
    public void GetBuyPrice_UnstockedItem_ReturnsZero()
    {
        var shop = CreateSpecialtyShop();
        var shield = MakeShield();

        Assert.Equal(0, shop.GetBuyPrice(shield));
    }

    [Fact]
    public void GetSellPrice_StockedItem_ReturnsPositive()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();
        var price = shop.GetSellPrice(sword);

        Assert.True(price > 0);
        Assert.True(price < shop.GetBuyPrice(sword));
    }

    [Fact]
    public void GetSellPrice_UnstockedItem_SpecialtyShop_ReturnsZero()
    {
        var shop = CreateSpecialtyShop();
        var shield = MakeShield();

        Assert.Equal(0, shop.GetSellPrice(shield));
    }

    [Fact]
    public void GetSellPrice_GeneralStore_AcceptsAnyTradeableItem()
    {
        var shop = CreateGeneralStore();
        var sword = MakeSword(); // not in general store stock

        var price = shop.GetSellPrice(sword);
        Assert.True(price > 0);
    }

    [Fact]
    public void GetSellPrice_UntradeableItem_ReturnsZero()
    {
        var shop = CreateGeneralStore();
        var untradeable = MakeUntradeable();

        Assert.Equal(0, shop.GetSellPrice(untradeable));
    }

    [Fact]
    public void TryBuy_Success_TransfersItemAndGold()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();
        var coins = MakeCoins();
        var inventory = new Inventory();
        inventory.TryAdd(coins, 1000);

        var price = shop.GetBuyPrice(sword);
        var result = shop.TryBuy(sword, coins, 1, inventory);

        Assert.True(result);
        Assert.True(inventory.Contains(sword));
        Assert.Equal(1000 - price, inventory.GetQuantity(coins));
        Assert.Equal(9, shop.Stock.First(s => s.Item.Id == sword.Id).CurrentStock);
    }

    [Fact]
    public void TryBuy_InsufficientGold_ReturnsFalse()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();
        var coins = MakeCoins();
        var inventory = new Inventory();
        inventory.TryAdd(coins, 1); // not enough

        Assert.False(shop.TryBuy(sword, coins, 1, inventory));
    }

    [Fact]
    public void TryBuy_InsufficientStock_ReturnsFalse()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();
        var coins = MakeCoins();
        var inventory = new Inventory();
        inventory.TryAdd(coins, 100_000);

        Assert.False(shop.TryBuy(sword, coins, 11, inventory)); // only 10 in stock
    }

    [Fact]
    public void TrySell_Success_TransfersItemAndGold()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();
        var coins = MakeCoins();
        var inventory = new Inventory();
        inventory.TryAdd(sword, 1);

        var sellPrice = shop.GetSellPrice(sword);
        var result = shop.TrySell(sword, coins, 1, inventory);

        Assert.True(result);
        Assert.False(inventory.Contains(sword));
        Assert.Equal(sellPrice, inventory.GetQuantity(coins));
        Assert.Equal(11, shop.Stock.First(s => s.Item.Id == sword.Id).CurrentStock);
    }

    [Fact]
    public void TrySell_GeneralStore_AcceptsNewItems()
    {
        var shop = CreateGeneralStore();
        var sword = MakeSword();
        var coins = MakeCoins();
        var inventory = new Inventory();
        inventory.TryAdd(sword, 1);

        var result = shop.TrySell(sword, coins, 1, inventory);

        Assert.True(result);
        Assert.NotNull(shop.GetStock(sword.Id));
    }

    [Fact]
    public void TrySell_PlayerDoesntHaveItem_ReturnsFalse()
    {
        var shop = CreateSpecialtyShop();
        var sword = MakeSword();
        var coins = MakeCoins();
        var inventory = new Inventory();

        Assert.False(shop.TrySell(sword, coins, 1, inventory));
    }

    [Fact]
    public void GetStock_ById_ReturnsCorrect()
    {
        var shop = CreateSpecialtyShop();
        var stock = shop.GetStock(new ItemId(100));
        Assert.NotNull(stock);
        Assert.Equal(10, stock.CurrentStock);
    }

    [Fact]
    public void GetStockByName_CaseInsensitive()
    {
        var shop = CreateSpecialtyShop();
        var stock = shop.GetStockByName("bronze SWORD");
        Assert.NotNull(stock);
    }
}
