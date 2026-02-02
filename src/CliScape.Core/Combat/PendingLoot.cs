using CliScape.Core.Items;

namespace CliScape.Core.Combat;

/// <summary>
///     Represents loot available from defeated enemies.
/// </summary>
public class PendingLoot
{
    private readonly List<LootItem> _items = [];

    /// <summary>
    ///     Gets all items available for looting.
    /// </summary>
    public IReadOnlyList<LootItem> Items => _items;

    /// <summary>
    ///     Whether there is any loot to pick up.
    /// </summary>
    public bool HasItems => _items.Count > 0;

    /// <summary>
    ///     Adds an item to the pending loot.
    /// </summary>
    public void Add(ItemId itemId, int quantity)
    {
        // Check if item already exists (for stackables)
        var existing = _items.FirstOrDefault(i => i.ItemId == itemId);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            _items.Add(new LootItem(itemId, quantity));
        }
    }

    /// <summary>
    ///     Removes an item from pending loot.
    /// </summary>
    public bool Remove(ItemId itemId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.ItemId == itemId);
        if (item == null)
        {
            return false;
        }

        if (quantity >= item.Quantity)
        {
            _items.Remove(item);
        }
        else
        {
            item.Quantity -= quantity;
        }

        return true;
    }

    /// <summary>
    ///     Clears all pending loot.
    /// </summary>
    public void Clear()
    {
        _items.Clear();
    }
}
