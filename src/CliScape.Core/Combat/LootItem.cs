using CliScape.Core.Items;

namespace CliScape.Core.Combat;

/// <summary>
///     Represents a lootable item with quantity.
/// </summary>
public class LootItem
{
    public LootItem(ItemId itemId, int quantity)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    public ItemId ItemId { get; }
    public int Quantity { get; set; }
}