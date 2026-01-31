namespace CliScape.Core.Items;

/// <summary>
///     The player's inventory, holding up to 28 item slots (matching OSRS).
/// </summary>
public sealed class Inventory
{
    public const int MaxSlots = 28;

    private readonly InventorySlot[] _slots;

    public Inventory()
    {
        _slots = new InventorySlot[MaxSlots];
        for (var i = 0; i < MaxSlots; i++)
        {
            _slots[i] = new InventorySlot();
        }
    }

    /// <summary>
    ///     Gets all inventory slots.
    /// </summary>
    public IReadOnlyList<InventorySlot> Slots => _slots;

    /// <summary>
    ///     Gets the number of free slots available.
    /// </summary>
    public int FreeSlots => _slots.Count(s => s.IsEmpty);

    /// <summary>
    ///     Gets the number of used slots.
    /// </summary>
    public int UsedSlots => MaxSlots - FreeSlots;

    /// <summary>
    ///     Checks if the inventory is full.
    /// </summary>
    public bool IsFull => FreeSlots == 0;

    /// <summary>
    ///     Attempts to add an item to the inventory.
    ///     Returns true if successful, false if no space available.
    /// </summary>
    public bool TryAdd(IItem item, int quantity = 1)
    {
        if (quantity <= 0)
        {
            return false;
        }

        if (item.IsStackable)
        {
            // For stackable items, find existing stack or empty slot
            var existingSlot = FindSlotWithItem(item);
            if (existingSlot is not null)
            {
                existingSlot.AddQuantity(quantity);
                return true;
            }
        }

        // For non-stackable items or new stackable items, need empty slots
        if (item.IsStackable)
        {
            var emptySlot = FindFirstEmptySlot();
            if (emptySlot is null)
            {
                return false;
            }

            emptySlot.Set(item, quantity);
            return true;
        }

        // Non-stackable: need one slot per item
        for (var i = 0; i < quantity; i++)
        {
            var emptySlot = FindFirstEmptySlot();
            if (emptySlot is null)
            {
                return false;
            }

            emptySlot.Set(item, 1);
        }

        return true;
    }

    /// <summary>
    ///     Checks if there's space to add the specified item and quantity.
    /// </summary>
    public bool CanAdd(IItem item, int quantity = 1)
    {
        if (quantity <= 0)
        {
            return true;
        }

        if (item.IsStackable)
        {
            // Stackable items only need one slot (existing or new)
            return FindSlotWithItem(item) is not null || FreeSlots > 0;
        }

        // Non-stackable items need one slot per item
        return FreeSlots >= quantity;
    }

    /// <summary>
    ///     Removes a quantity of the specified item from the inventory.
    ///     Returns the amount actually removed.
    /// </summary>
    public int Remove(IItem item, int quantity = 1)
    {
        var remaining = quantity;

        foreach (var slot in _slots.Where(s => !s.IsEmpty && s.Item?.Id == item.Id))
        {
            if (remaining <= 0)
            {
                break;
            }

            var removed = slot.RemoveQuantity(remaining);
            remaining -= removed;
        }

        return quantity - remaining;
    }

    /// <summary>
    ///     Gets the total quantity of a specific item in the inventory.
    /// </summary>
    public int GetQuantity(IItem item)
    {
        return _slots
            .Where(s => !s.IsEmpty && s.Item?.Id == item.Id)
            .Sum(s => s.Quantity);
    }

    /// <summary>
    ///     Gets the total quantity of an item by its ID.
    /// </summary>
    public int GetQuantity(ItemId itemId)
    {
        return _slots
            .Where(s => !s.IsEmpty && s.Item?.Id == itemId)
            .Sum(s => s.Quantity);
    }

    /// <summary>
    ///     Checks if the inventory contains at least one of the specified item.
    /// </summary>
    public bool Contains(IItem item)
    {
        return GetQuantity(item) > 0;
    }

    /// <summary>
    ///     Checks if the inventory contains at least the specified quantity of an item.
    /// </summary>
    public bool Contains(IItem item, int quantity)
    {
        return GetQuantity(item) >= quantity;
    }

    /// <summary>
    ///     Gets all non-empty slots as item/quantity pairs.
    /// </summary>
    public IEnumerable<(IItem Item, int Quantity, int SlotIndex)> GetItems()
    {
        for (var i = 0; i < _slots.Length; i++)
        {
            var slot = _slots[i];
            if (!slot.IsEmpty && slot.Item is not null)
            {
                yield return (slot.Item, slot.Quantity, i);
            }
        }
    }

    /// <summary>
    ///     Gets the slot at the specified index.
    /// </summary>
    public InventorySlot GetSlot(int index)
    {
        if (index < 0 || index >= MaxSlots)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _slots[index];
    }

    /// <summary>
    ///     Clears the slot at the specified index.
    /// </summary>
    public void ClearSlot(int index)
    {
        if (index < 0 || index >= MaxSlots)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _slots[index].Clear();
    }

    /// <summary>
    ///     Clears all items from the inventory.
    /// </summary>
    public void Clear()
    {
        foreach (var slot in _slots)
        {
            slot.Clear();
        }
    }

    /// <summary>
    ///     Swaps the contents of two inventory slots.
    /// </summary>
    public void SwapSlots(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= MaxSlots)
        {
            throw new ArgumentOutOfRangeException(nameof(fromIndex));
        }

        if (toIndex < 0 || toIndex >= MaxSlots)
        {
            throw new ArgumentOutOfRangeException(nameof(toIndex));
        }

        var fromSlot = _slots[fromIndex];
        var toSlot = _slots[toIndex];

        var tempItem = fromSlot.Item;
        var tempQuantity = fromSlot.Quantity;

        if (toSlot.IsEmpty)
        {
            fromSlot.Clear();
        }
        else
        {
            fromSlot.Set(toSlot.Item!, toSlot.Quantity);
        }

        if (tempItem is null)
        {
            toSlot.Clear();
        }
        else
        {
            toSlot.Set(tempItem, tempQuantity);
        }
    }

    /// <summary>
    ///     Moves an item from one slot to another, shifting other items as needed.
    /// </summary>
    public void MoveSlot(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= MaxSlots)
        {
            throw new ArgumentOutOfRangeException(nameof(fromIndex));
        }

        if (toIndex < 0 || toIndex >= MaxSlots)
        {
            throw new ArgumentOutOfRangeException(nameof(toIndex));
        }

        if (fromIndex == toIndex)
        {
            return;
        }

        var fromSlot = _slots[fromIndex];
        if (fromSlot.IsEmpty)
        {
            return;
        }

        var tempItem = fromSlot.Item!;
        var tempQuantity = fromSlot.Quantity;

        // Remove the item from its current position
        fromSlot.Clear();

        // Shift items to make room at the target position
        if (fromIndex < toIndex)
        {
            // Moving down: shift items up
            for (var i = fromIndex; i < toIndex; i++)
            {
                var nextSlot = _slots[i + 1];
                if (nextSlot.IsEmpty)
                {
                    _slots[i].Clear();
                }
                else
                {
                    _slots[i].Set(nextSlot.Item!, nextSlot.Quantity);
                }
            }
        }
        else
        {
            // Moving up: shift items down
            for (var i = fromIndex; i > toIndex; i--)
            {
                var prevSlot = _slots[i - 1];
                if (prevSlot.IsEmpty)
                {
                    _slots[i].Clear();
                }
                else
                {
                    _slots[i].Set(prevSlot.Item!, prevSlot.Quantity);
                }
            }
        }

        // Place the item at the target position
        _slots[toIndex].Set(tempItem, tempQuantity);
    }

    private InventorySlot? FindSlotWithItem(IItem item)
    {
        return _slots.FirstOrDefault(s => !s.IsEmpty && s.Item?.Id == item.Id);
    }

    private InventorySlot? FindFirstEmptySlot()
    {
        return _slots.FirstOrDefault(s => s.IsEmpty);
    }
}