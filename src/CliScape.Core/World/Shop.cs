using CliScape.Core.Items;

namespace CliScape.Core.World;

/// <summary>
///     A shop where players can buy and sell items.
///     Implements OSRS-style pricing based on stock levels.
/// </summary>
public class Shop
{
    private readonly List<ShopStock> _stock = [];

    /// <summary>
    ///     The name of this shop.
    /// </summary>
    public required ShopName Name { get; init; }

    /// <summary>
    ///     Whether this shop is a general store that buys any tradeable item.
    /// </summary>
    public bool IsGeneralStore { get; init; }

    /// <summary>
    ///     The percentage of base value that this shop pays when buying items from players.
    ///     Default is 40% (0.4) like OSRS general stores.
    /// </summary>
    public double BuyPriceMultiplier { get; init; } = 0.4;

    /// <summary>
    ///     The percentage of base value that this shop charges when selling items to players.
    ///     Default is 130% (1.3) matching OSRS specialty shops at base stock.
    /// </summary>
    public double SellPriceMultiplier { get; init; } = 1.3;

    /// <summary>
    ///     Gets all items currently in stock.
    /// </summary>
    public IReadOnlyList<ShopStock> Stock => _stock;

    /// <summary>
    ///     Adds an item to the shop's base inventory.
    /// </summary>
    public void AddStock(IItem item, int baseStock)
    {
        var existing = _stock.FirstOrDefault(s => s.Item.Id == item.Id);
        if (existing is not null)
        {
            existing.CurrentStock += baseStock;
        }
        else
        {
            _stock.Add(ShopStock.Create(item, baseStock));
        }
    }

    /// <summary>
    ///     Calculates the price to buy an item from the shop.
    ///     Price increases when stock is low, decreases when stock is high.
    /// </summary>
    public int GetBuyPrice(IItem item)
    {
        var stock = _stock.FirstOrDefault(s => s.Item.Id == item.Id);
        if (stock is null || stock.CurrentStock <= 0)
        {
            return 0; // Item not available
        }

        // OSRS-style pricing: price varies based on current vs base stock
        // When stock is at base level, use base multiplier
        // When stock is lower, price increases; when higher, price decreases
        var stockRatio = stock.BaseStock > 0
            ? (double)stock.CurrentStock / stock.BaseStock
            : 1.0;

        // Adjust multiplier based on stock (inverse relationship)
        var adjustedMultiplier = SellPriceMultiplier / Math.Max(0.5, stockRatio);

        var price = (int)Math.Ceiling(item.BaseValue * adjustedMultiplier);
        return Math.Max(1, price); // Minimum price of 1 gp
    }

    /// <summary>
    ///     Calculates the price the shop will pay for an item.
    ///     General stores pay for any tradeable item.
    ///     Specialty shops only buy items they stock.
    /// </summary>
    public int GetSellPrice(IItem item)
    {
        if (!item.IsTradeable)
        {
            return 0;
        }

        var stock = _stock.FirstOrDefault(s => s.Item.Id == item.Id);

        // Non-general stores only buy items they stock
        if (!IsGeneralStore && stock is null)
        {
            return 0;
        }

        // OSRS-style: price decreases as shop has more of the item
        var currentStock = stock?.CurrentStock ?? 0;
        var baseStock = stock?.BaseStock ?? 10; // Default base for general store new items

        var stockRatio = baseStock > 0
            ? (double)currentStock / baseStock
            : 0.0;

        // Price decreases as stock increases above base
        var adjustedMultiplier = BuyPriceMultiplier * Math.Max(0.1, 1.0 - stockRatio * 0.3);

        var price = (int)Math.Floor(item.BaseValue * adjustedMultiplier);
        return Math.Max(0, price); // Can be 0 for worthless items
    }

    /// <summary>
    ///     Attempts to sell an item to a player (player buys from shop).
    ///     Returns true if successful.
    /// </summary>
    public bool TryBuy(IItem item, IItem coinsItem, int quantity, Inventory playerInventory)
    {
        var stock = _stock.FirstOrDefault(s => s.Item.Id == item.Id);
        if (stock is null || stock.CurrentStock < quantity)
        {
            return false;
        }

        var totalPrice = GetBuyPrice(item) * quantity;
        var playerGold = playerInventory.GetQuantity(coinsItem);
        if (playerGold < totalPrice)
        {
            return false;
        }

        if (!playerInventory.CanAdd(item, quantity))
        {
            return false;
        }

        // Process transaction
        stock.CurrentStock -= quantity;
        playerInventory.Remove(coinsItem, totalPrice);
        playerInventory.TryAdd(item, quantity);

        return true;
    }

    /// <summary>
    ///     Attempts to buy an item from a player (player sells to shop).
    ///     Returns true if successful.
    /// </summary>
    public bool TrySell(IItem item, IItem coinsItem, int quantity, Inventory playerInventory)
    {
        var sellPrice = GetSellPrice(item);
        if (sellPrice <= 0 && !IsGeneralStore)
        {
            return false;
        }

        if (playerInventory.GetQuantity(item) < quantity)
        {
            return false;
        }

        // Process transaction
        var totalPrice = sellPrice * quantity;
        playerInventory.Remove(item, quantity);
        playerInventory.TryAdd(coinsItem, totalPrice);

        // Add to shop stock
        var stock = _stock.FirstOrDefault(s => s.Item.Id == item.Id);
        if (stock is not null)
        {
            stock.CurrentStock += quantity;
        }
        else if (IsGeneralStore)
        {
            // General stores accept any item
            _stock.Add(new ShopStock
            {
                Item = item,
                BaseStock = 0, // Player-sold items have 0 base stock
                CurrentStock = quantity
            });
        }

        return true;
    }

    /// <summary>
    ///     Gets a stock entry by item ID.
    /// </summary>
    public ShopStock? GetStock(ItemId itemId)
    {
        return _stock.FirstOrDefault(s => s.Item.Id == itemId);
    }

    /// <summary>
    ///     Gets a stock entry by item name (case-insensitive).
    /// </summary>
    public ShopStock? GetStockByName(string itemName)
    {
        return _stock.FirstOrDefault(s =>
            s.Item.Name.Value.Equals(itemName, StringComparison.OrdinalIgnoreCase));
    }
}