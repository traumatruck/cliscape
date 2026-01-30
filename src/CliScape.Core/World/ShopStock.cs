using CliScape.Core.Items;

namespace CliScape.Core.World;

/// <summary>
///     Represents an item available for sale in a shop with its stock information.
/// </summary>
public sealed class ShopStock
{
    public required IItem Item { get; init; }

    /// <summary>
    ///     The base stock level that the shop maintains.
    ///     The shop will restock to this level over time.
    /// </summary>
    public required int BaseStock { get; init; }

    /// <summary>
    ///     The current stock level available.
    /// </summary>
    public int CurrentStock { get; set; }

    /// <summary>
    ///     Creates a new shop stock entry with current stock set to base stock.
    /// </summary>
    public static ShopStock Create(IItem item, int baseStock)
    {
        return new ShopStock
        {
            Item = item,
            BaseStock = baseStock,
            CurrentStock = baseStock
        };
    }
}