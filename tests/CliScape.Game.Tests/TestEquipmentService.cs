using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players.Skills;
using CliScape.Game.Items;
using CliScape.Tests.Shared;

namespace CliScape.Game.Tests;

public sealed class TestEquipmentService
{
    private readonly StubEventDispatcher _events = new();
    private readonly EquipmentService _sut;

    public TestEquipmentService()
    {
        _sut = new EquipmentService(_events);
    }

    [Fact]
    public void CanEquip_MeetsAllRequirements_Succeeds()
    {
        var player = TestFactory.CreatePlayer();
        var item = TestFactory.CreateStubEquippable(
            new ItemId(100), "Bronze sword", EquipmentSlot.Weapon);

        var result = _sut.CanEquip(player, item);

        Assert.True(result.Success);
    }

    [Fact]
    public void CanEquip_InsufficientAttack_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var item = TestFactory.CreateStubEquippable(
            new ItemId(100), "Rune sword", EquipmentSlot.Weapon, reqAttack: 40);

        var result = _sut.CanEquip(player, item);

        Assert.False(result.Success);
        Assert.Contains("Attack", result.Message);
    }

    [Fact]
    public void CanEquip_InsufficientDefence_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var item = TestFactory.CreateStubEquippable(
            new ItemId(100), "Rune platebody", EquipmentSlot.Body, reqDefence: 40);

        var result = _sut.CanEquip(player, item);

        Assert.False(result.Success);
        Assert.Contains("Defence", result.Message);
    }

    [Fact]
    public void CanEquip_InsufficientStrength_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var item = TestFactory.CreateStubEquippable(
            new ItemId(100), "Some item", EquipmentSlot.Weapon, reqStrength: 50);

        var result = _sut.CanEquip(player, item);

        Assert.False(result.Success);
        Assert.Contains("Strength", result.Message);
    }

    [Fact]
    public void CanEquip_InsufficientRanged_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var item = TestFactory.CreateStubEquippable(
            new ItemId(100), "Bow", EquipmentSlot.Weapon, reqRanged: 30);

        var result = _sut.CanEquip(player, item);

        Assert.False(result.Success);
        Assert.Contains("Ranged", result.Message);
    }

    [Fact]
    public void CanEquip_InsufficientMagic_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var item = TestFactory.CreateStubEquippable(
            new ItemId(100), "Staff", EquipmentSlot.Weapon, reqMagic: 20);

        var result = _sut.CanEquip(player, item);

        Assert.False(result.Success);
        Assert.Contains("Magic", result.Message);
    }

    [Fact]
    public void Equip_Success_EquipsItemAndRemovesFromInventory()
    {
        var player = TestFactory.CreatePlayer();
        var sword = TestFactory.CreateStubEquippable(
            new ItemId(100), "Bronze sword", EquipmentSlot.Weapon);
        player.Inventory.TryAdd(sword);

        var result = _sut.Equip(player, sword);

        Assert.True(result.Success);
        Assert.Equal(sword, result.EquippedItem);
        Assert.Null(result.UnequippedItem);
        Assert.Same(sword, player.Equipment.GetEquipped(EquipmentSlot.Weapon));
    }

    [Fact]
    public void Equip_SwapsWithPreviousEquipment()
    {
        var player = TestFactory.CreatePlayer();
        var oldSword = TestFactory.CreateStubEquippable(
            new ItemId(100), "Bronze sword", EquipmentSlot.Weapon);
        var newSword = TestFactory.CreateStubEquippable(
            new ItemId(101), "Iron sword", EquipmentSlot.Weapon);

        player.Equipment.Equip(oldSword);
        player.Inventory.TryAdd(newSword);

        var result = _sut.Equip(player, newSword);

        Assert.True(result.Success);
        Assert.Equal(newSword, result.EquippedItem);
        Assert.Equal(oldSword, result.UnequippedItem);
        Assert.Same(newSword, player.Equipment.GetEquipped(EquipmentSlot.Weapon));
        // Old sword should be back in inventory
        Assert.True(player.Inventory.Contains(oldSword));
    }

    [Fact]
    public void Unequip_Success_MovesToInventory()
    {
        var player = TestFactory.CreatePlayer();
        var helm = TestFactory.CreateStubEquippable(
            new ItemId(100), "Bronze helm", EquipmentSlot.Head);
        player.Equipment.Equip(helm);

        var result = _sut.Unequip(player, EquipmentSlot.Head);

        Assert.True(result.Success);
        Assert.Null(player.Equipment.GetEquipped(EquipmentSlot.Head));
        Assert.True(player.Inventory.Contains(helm));
    }

    [Fact]
    public void Unequip_EmptySlot_Fails()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.Unequip(player, EquipmentSlot.Head);

        Assert.False(result.Success);
        Assert.Contains("Nothing", result.Message);
    }

    [Fact]
    public void Unequip_FullInventory_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var helm = TestFactory.CreateStubEquippable(
            new ItemId(100), "Bronze helm", EquipmentSlot.Head);
        player.Equipment.Equip(helm);

        // Fill inventory to 28 slots
        for (var i = 0; i < 28; i++)
        {
            player.Inventory.TryAdd(
                TestFactory.CreateStubItem(new ItemId(200 + i), $"Junk {i}"));
        }

        var result = _sut.Unequip(player, EquipmentSlot.Head);

        Assert.False(result.Success);
        Assert.Contains("full", result.Message);
    }
}
