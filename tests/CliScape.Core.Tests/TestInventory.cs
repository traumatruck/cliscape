using CliScape.Core.Items;
using CliScape.Tests.Shared;

namespace CliScape.Core.Tests;

public sealed class TestInventory
{
    private readonly Inventory _inventory = new();

    private static IItem MakeItem(int id = 1, bool stackable = false) =>
        TestFactory.CreateStubItem(new ItemId(id), $"Item{id}", stackable);

    [Fact]
    public void NewInventory_HasMaxFreeSlots()
    {
        Assert.Equal(Inventory.MaxSlots, _inventory.FreeSlots);
        Assert.Equal(0, _inventory.UsedSlots);
        Assert.False(_inventory.IsFull);
    }

    [Fact]
    public void TryAdd_NonStackable_OccupiesOneSlot()
    {
        var item = MakeItem();
        Assert.True(_inventory.TryAdd(item));
        Assert.Equal(Inventory.MaxSlots - 1, _inventory.FreeSlots);
    }

    [Fact]
    public void TryAdd_NonStackable_MultipleQuantity_OccupiesMultipleSlots()
    {
        var item = MakeItem();
        Assert.True(_inventory.TryAdd(item, 3));
        Assert.Equal(Inventory.MaxSlots - 3, _inventory.FreeSlots);
    }

    [Fact]
    public void TryAdd_NonStackable_FullInventory_ReturnsFalse()
    {
        var item = MakeItem();
        for (var i = 0; i < Inventory.MaxSlots; i++)
            _inventory.TryAdd(item);

        Assert.True(_inventory.IsFull);
        Assert.False(_inventory.TryAdd(item));
    }

    [Fact]
    public void TryAdd_Stackable_SameSlot()
    {
        var item = MakeItem(1, stackable: true);
        _inventory.TryAdd(item, 5);
        _inventory.TryAdd(item, 10);

        Assert.Equal(1, _inventory.UsedSlots);
        Assert.Equal(15, _inventory.GetQuantity(item));
    }

    [Fact]
    public void TryAdd_Stackable_NewItem_UsesOneSlot()
    {
        var item = MakeItem(1, stackable: true);
        _inventory.TryAdd(item, 100);

        Assert.Equal(1, _inventory.UsedSlots);
        Assert.Equal(100, _inventory.GetQuantity(item));
    }

    [Fact]
    public void TryAdd_ZeroQuantity_ReturnsFalse()
    {
        var item = MakeItem();
        Assert.False(_inventory.TryAdd(item, 0));
    }

    [Fact]
    public void CanAdd_NonStackable_ReflectsAvailableSlots()
    {
        var item = MakeItem();
        Assert.True(_inventory.CanAdd(item, 28));
        Assert.False(_inventory.CanAdd(item, 29));
    }

    [Fact]
    public void CanAdd_Stackable_AlwaysTrueIfSlotExists()
    {
        var item = MakeItem(1, stackable: true);
        _inventory.TryAdd(item, 1);
        Assert.True(_inventory.CanAdd(item, 999));
    }

    [Fact]
    public void Remove_ReducesQuantity()
    {
        var item = MakeItem(1, stackable: true);
        _inventory.TryAdd(item, 10);
        var removed = _inventory.Remove(item, 3);

        Assert.Equal(3, removed);
        Assert.Equal(7, _inventory.GetQuantity(item));
    }

    [Fact]
    public void Remove_ClearsSlotWhenEmpty()
    {
        var item = MakeItem();
        _inventory.TryAdd(item);
        _inventory.Remove(item);

        Assert.Equal(0, _inventory.GetQuantity(item));
        Assert.Equal(Inventory.MaxSlots, _inventory.FreeSlots);
    }

    [Fact]
    public void Remove_ReturnsActualAmountRemoved()
    {
        var item = MakeItem(1, stackable: true);
        _inventory.TryAdd(item, 5);
        var removed = _inventory.Remove(item, 10);

        Assert.Equal(5, removed);
        Assert.Equal(0, _inventory.GetQuantity(item));
    }

    [Fact]
    public void Contains_TrueWhenPresent()
    {
        var item = MakeItem();
        _inventory.TryAdd(item);
        Assert.True(_inventory.Contains(item));
    }

    [Fact]
    public void Contains_FalseWhenAbsent()
    {
        var item = MakeItem();
        Assert.False(_inventory.Contains(item));
    }

    [Fact]
    public void Contains_WithQuantity_ChecksThreshold()
    {
        var item = MakeItem(1, stackable: true);
        _inventory.TryAdd(item, 5);

        Assert.True(_inventory.Contains(item, 5));
        Assert.False(_inventory.Contains(item, 6));
    }

    [Fact]
    public void GetQuantity_ById_MatchesByItem()
    {
        var item = MakeItem(42, stackable: true);
        _inventory.TryAdd(item, 7);

        Assert.Equal(7, _inventory.GetQuantity(new ItemId(42)));
    }

    [Fact]
    public void GetItems_ReturnsAllNonEmptySlots()
    {
        var item1 = MakeItem(1);
        var item2 = MakeItem(2);
        _inventory.TryAdd(item1);
        _inventory.TryAdd(item2);

        var items = _inventory.GetItems().ToList();
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public void SwapSlots_ExchangesContents()
    {
        var item1 = MakeItem(1);
        var item2 = MakeItem(2);
        _inventory.TryAdd(item1);
        _inventory.TryAdd(item2);

        _inventory.SwapSlots(0, 1);

        Assert.Equal(new ItemId(2), _inventory.GetSlot(0).Item!.Id);
        Assert.Equal(new ItemId(1), _inventory.GetSlot(1).Item!.Id);
    }

    [Fact]
    public void SwapSlots_WithEmptySlot_Moves()
    {
        var item = MakeItem(1);
        _inventory.TryAdd(item);

        _inventory.SwapSlots(0, 5);

        Assert.True(_inventory.GetSlot(0).IsEmpty);
        Assert.Equal(new ItemId(1), _inventory.GetSlot(5).Item!.Id);
    }

    [Fact]
    public void SwapSlots_InvalidIndex_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _inventory.SwapSlots(-1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => _inventory.SwapSlots(0, 28));
    }

    [Fact]
    public void MoveSlot_ShiftsItems()
    {
        var item1 = MakeItem(1);
        var item2 = MakeItem(2);
        var item3 = MakeItem(3);
        _inventory.TryAdd(item1);
        _inventory.TryAdd(item2);
        _inventory.TryAdd(item3);

        // Move slot 0 to slot 2
        _inventory.MoveSlot(0, 2);

        Assert.Equal(new ItemId(2), _inventory.GetSlot(0).Item!.Id);
        Assert.Equal(new ItemId(3), _inventory.GetSlot(1).Item!.Id);
        Assert.Equal(new ItemId(1), _inventory.GetSlot(2).Item!.Id);
    }

    [Fact]
    public void MoveSlot_SameIndex_NoChange()
    {
        var item = MakeItem(1);
        _inventory.TryAdd(item);

        _inventory.MoveSlot(0, 0);
        Assert.Equal(new ItemId(1), _inventory.GetSlot(0).Item!.Id);
    }

    [Fact]
    public void Clear_EmptiesAll()
    {
        var item = MakeItem(1, stackable: true);
        _inventory.TryAdd(item, 10);
        _inventory.Clear();

        Assert.Equal(Inventory.MaxSlots, _inventory.FreeSlots);
        Assert.Equal(0, _inventory.GetQuantity(item));
    }

    [Fact]
    public void ClearSlot_ClearsSpecificSlot()
    {
        var item = MakeItem(1);
        _inventory.TryAdd(item);
        _inventory.ClearSlot(0);

        Assert.True(_inventory.GetSlot(0).IsEmpty);
    }

    [Fact]
    public void TrySetSlot_SetsItemAtIndex()
    {
        var item = MakeItem(1);
        Assert.True(_inventory.TrySetSlot(5, item, 3));
        Assert.Equal(new ItemId(1), _inventory.GetSlot(5).Item!.Id);
        Assert.Equal(3, _inventory.GetSlot(5).Quantity);
    }

    [Fact]
    public void TrySetSlot_InvalidIndex_ReturnsFalse()
    {
        var item = MakeItem(1);
        Assert.False(_inventory.TrySetSlot(-1, item));
        Assert.False(_inventory.TrySetSlot(28, item));
    }
}
