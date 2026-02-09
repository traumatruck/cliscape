namespace CliScape.Core.Items;

/// <summary>
///     Represents a slot in the player's inventory, holding an item and its quantity.
/// </summary>
public sealed class InventorySlot
{
    public IItem? Item { get; private set; }

    public int Quantity { get; private set; }

    public bool IsEmpty => Item is null || Quantity <= 0;

    /// <summary>
    ///     Sets this slot to contain the specified item and quantity.
    /// </summary>
    public void Set(IItem item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }

    /// <summary>
    ///     Adds to the quantity of a stackable item.
    /// </summary>
    public void AddQuantity(int amount)
    {
        Quantity += amount;
    }

    /// <summary>
    ///     Removes a quantity from this slot. Returns the amount actually removed.
    /// </summary>
    public int RemoveQuantity(int amount)
    {
        var removed = Math.Min(amount, Quantity);
        Quantity -= removed;

        if (Quantity <= 0)
        {
            Clear();
        }

        return removed;
    }

    /// <summary>
    ///     Clears this slot.
    /// </summary>
    public void Clear()
    {
        Item = null;
        Quantity = 0;
    }
}