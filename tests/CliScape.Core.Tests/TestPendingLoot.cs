using CliScape.Core.Combat;
using CliScape.Core.Items;

namespace CliScape.Core.Tests;

public sealed class TestPendingLoot
{
    [Fact]
    public void NewPendingLoot_HasNoItems()
    {
        var loot = new PendingLoot();
        Assert.False(loot.HasItems);
        Assert.Empty(loot.Items);
    }

    [Fact]
    public void Add_CreatesEntry()
    {
        var loot = new PendingLoot();
        loot.Add(new ItemId(1), 5);

        Assert.True(loot.HasItems);
        Assert.Single(loot.Items);
        Assert.Equal(new ItemId(1), loot.Items[0].ItemId);
        Assert.Equal(5, loot.Items[0].Quantity);
    }

    [Fact]
    public void Add_SameItemId_MergesQuantity()
    {
        var loot = new PendingLoot();
        loot.Add(new ItemId(1), 5);
        loot.Add(new ItemId(1), 3);

        Assert.Single(loot.Items);
        Assert.Equal(8, loot.Items[0].Quantity);
    }

    [Fact]
    public void Add_DifferentItems_CreatesSeparateEntries()
    {
        var loot = new PendingLoot();
        loot.Add(new ItemId(1), 5);
        loot.Add(new ItemId(2), 3);

        Assert.Equal(2, loot.Items.Count);
    }

    [Fact]
    public void Remove_FullQuantity_RemovesEntry()
    {
        var loot = new PendingLoot();
        loot.Add(new ItemId(1), 5);

        var removed = loot.Remove(new ItemId(1), 5);

        Assert.True(removed);
        Assert.Empty(loot.Items);
        Assert.False(loot.HasItems);
    }

    [Fact]
    public void Remove_PartialQuantity_LeavesRemainder()
    {
        var loot = new PendingLoot();
        loot.Add(new ItemId(1), 10);

        var removed = loot.Remove(new ItemId(1), 3);

        Assert.True(removed);
        Assert.Single(loot.Items);
        Assert.Equal(7, loot.Items[0].Quantity);
    }

    [Fact]
    public void Remove_MoreThanAvailable_RemovesEntry()
    {
        var loot = new PendingLoot();
        loot.Add(new ItemId(1), 5);

        var removed = loot.Remove(new ItemId(1), 100);

        Assert.True(removed);
        Assert.Empty(loot.Items);
    }

    [Fact]
    public void Remove_NonexistentItem_ReturnsFalse()
    {
        var loot = new PendingLoot();
        var removed = loot.Remove(new ItemId(999), 1);
        Assert.False(removed);
    }

    [Fact]
    public void Clear_RemovesAllItems()
    {
        var loot = new PendingLoot();
        loot.Add(new ItemId(1), 5);
        loot.Add(new ItemId(2), 10);

        loot.Clear();

        Assert.False(loot.HasItems);
        Assert.Empty(loot.Items);
    }
}
