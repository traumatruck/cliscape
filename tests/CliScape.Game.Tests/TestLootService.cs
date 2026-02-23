using CliScape.Core.Combat;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Game.Items;
using CliScape.Tests.Shared;

namespace CliScape.Game.Tests;

public sealed class TestLootService
{
    private readonly StubEventDispatcher _events = new();
    private readonly LootService _sut;

    public TestLootService()
    {
        _sut = new LootService(_events);
    }

    private static IItem MakeItem(int id, string name, bool stackable = false)
    {
        return TestFactory.CreateStubItem(new ItemId(id), name, stackable: stackable);
    }

    [Fact]
    public void LootAll_PicksUpAllItems()
    {
        var bones = MakeItem(1, "Bones");
        var coins = MakeItem(2, "Coins", stackable: true);
        IItem? Resolver(ItemId id) => id.Value == 1 ? bones : id.Value == 2 ? coins : null;

        var pending = new PendingLoot();
        pending.Add(new ItemId(1), 1);
        pending.Add(new ItemId(2), 10);

        var player = TestFactory.CreatePlayer();

        var result = _sut.LootAll(pending, player, Resolver);

        Assert.True(result.Success);
        Assert.Equal(2, result.ItemsLooted.Count);
        Assert.True(player.Inventory.Contains(bones));
        Assert.Empty(pending.Items);
    }

    [Fact]
    public void LootAll_SkipsWhenInventoryFull()
    {
        var bones = MakeItem(1, "Bones");
        IItem? Resolver(ItemId id) => id.Value == 1 ? bones : null;

        var pending = new PendingLoot();
        pending.Add(new ItemId(1), 1);

        var player = TestFactory.CreatePlayer();
        for (var i = 0; i < 28; i++)
            player.Inventory.TryAdd(MakeItem(100 + i, $"Junk {i}"));

        var result = _sut.LootAll(pending, player, Resolver);

        Assert.False(result.Success);
        Assert.Contains("full", result.Message);
    }

    [Fact]
    public void LootAll_EmptyLoot_ReturnsSuccess()
    {
        var pending = new PendingLoot();
        var player = TestFactory.CreatePlayer();

        var result = _sut.LootAll(pending, player, _ => null);

        Assert.True(result.Success);
        Assert.Empty(result.ItemsLooted);
    }

    [Fact]
    public void LootAll_RaisesItemPickedUpEvent()
    {
        var bones = MakeItem(1, "Bones");
        IItem? Resolver(ItemId id) => id.Value == 1 ? bones : null;

        var pending = new PendingLoot();
        pending.Add(new ItemId(1), 1);
        var player = TestFactory.CreatePlayer();

        _sut.LootAll(pending, player, Resolver);

        _events.AssertRaised<ItemPickedUpEvent>();
    }

    [Fact]
    public void LootItem_ByName_PicksUpMatchingItem()
    {
        var bones = MakeItem(1, "Bones");
        IItem? Resolver(ItemId id) => id.Value == 1 ? bones : null;

        var pending = new PendingLoot();
        pending.Add(new ItemId(1), 3);
        var player = TestFactory.CreatePlayer();

        var result = _sut.LootItem(pending, player, "Bones", null, Resolver);

        Assert.True(result.Success);
        Assert.Single(result.ItemsLooted);
        Assert.Equal(3, result.ItemsLooted[0].Quantity);
    }

    [Fact]
    public void LootItem_PartialMatch_Works()
    {
        var bones = MakeItem(1, "Big bones");
        IItem? Resolver(ItemId id) => id.Value == 1 ? bones : null;

        var pending = new PendingLoot();
        pending.Add(new ItemId(1), 1);
        var player = TestFactory.CreatePlayer();

        var result = _sut.LootItem(pending, player, "bone", null, Resolver);

        Assert.True(result.Success);
    }

    [Fact]
    public void LootItem_NotFound_Fails()
    {
        var bones = MakeItem(1, "Bones");
        IItem? Resolver(ItemId id) => id.Value == 1 ? bones : null;

        var pending = new PendingLoot();
        pending.Add(new ItemId(1), 1);
        var player = TestFactory.CreatePlayer();

        var result = _sut.LootItem(pending, player, "Dragon", null, Resolver);

        Assert.False(result.Success);
        Assert.Contains("Could not find", result.Message);
    }

    [Fact]
    public void LootItem_WithAmount_LootsPartialQuantity()
    {
        var bones = MakeItem(1, "Bones");
        IItem? Resolver(ItemId id) => id.Value == 1 ? bones : null;

        var pending = new PendingLoot();
        pending.Add(new ItemId(1), 10);
        var player = TestFactory.CreatePlayer();

        var result = _sut.LootItem(pending, player, "Bones", 3, Resolver);

        Assert.True(result.Success);
        Assert.Equal(3, result.ItemsLooted[0].Quantity);
        // 7 should remain in pending loot
        Assert.Single(pending.Items);
    }

    [Fact]
    public void LootItem_InventoryFull_Fails()
    {
        var bones = MakeItem(1, "Bones");
        IItem? Resolver(ItemId id) => id.Value == 1 ? bones : null;

        var pending = new PendingLoot();
        pending.Add(new ItemId(1), 1);
        var player = TestFactory.CreatePlayer();
        for (var i = 0; i < 28; i++)
            player.Inventory.TryAdd(MakeItem(100 + i, $"Junk {i}"));

        var result = _sut.LootItem(pending, player, "Bones", null, Resolver);

        Assert.False(result.Success);
        Assert.Contains("full", result.Message);
    }
}
