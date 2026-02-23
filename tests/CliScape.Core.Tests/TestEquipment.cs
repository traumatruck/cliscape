using CliScape.Core.Items;
using CliScape.Tests.Shared;

namespace CliScape.Core.Tests;

public sealed class TestEquipment
{
    private readonly Equipment _equipment = new();

    private static IEquippable MakeWeapon(int attackSpeed = 5, int meleeStr = 10) =>
        TestFactory.CreateStubEquippable(
            new ItemId(100), "Test Sword", EquipmentSlot.Weapon,
            new EquipmentStats
            {
                AttackSpeed = attackSpeed,
                MeleeStrengthBonus = meleeStr,
                StabAttackBonus = 20
            });

    private static IEquippable MakeBody(int stabDef = 15) =>
        TestFactory.CreateStubEquippable(
            new ItemId(200), "Test Platebody", EquipmentSlot.Body,
            new EquipmentStats { StabDefenceBonus = stabDef, SlashDefenceBonus = 12 });

    [Fact]
    public void Equip_StoresItemInCorrectSlot()
    {
        var weapon = MakeWeapon();
        _equipment.Equip(weapon);

        Assert.Equal(weapon, _equipment.GetEquipped(EquipmentSlot.Weapon));
        Assert.True(_equipment.IsSlotOccupied(EquipmentSlot.Weapon));
    }

    [Fact]
    public void Equip_ReturnsPreviousItem()
    {
        var weapon1 = TestFactory.CreateStubEquippable(new ItemId(100), "Sword1", EquipmentSlot.Weapon);
        var weapon2 = TestFactory.CreateStubEquippable(new ItemId(101), "Sword2", EquipmentSlot.Weapon);

        _equipment.Equip(weapon1);
        var previous = _equipment.Equip(weapon2);

        Assert.Equal(weapon1, previous);
        Assert.Equal(weapon2, _equipment.GetEquipped(EquipmentSlot.Weapon));
    }

    [Fact]
    public void Equip_ReturnsNull_WhenSlotWasEmpty()
    {
        var weapon = MakeWeapon();
        var previous = _equipment.Equip(weapon);
        Assert.Null(previous);
    }

    [Fact]
    public void Unequip_ReturnsItem_ClearsSlot()
    {
        var weapon = MakeWeapon();
        _equipment.Equip(weapon);

        var unequipped = _equipment.Unequip(EquipmentSlot.Weapon);

        Assert.Equal(weapon, unequipped);
        Assert.False(_equipment.IsSlotOccupied(EquipmentSlot.Weapon));
        Assert.Null(_equipment.GetEquipped(EquipmentSlot.Weapon));
    }

    [Fact]
    public void Unequip_EmptySlot_ReturnsNull()
    {
        var result = _equipment.Unequip(EquipmentSlot.Weapon);
        Assert.Null(result);
    }

    [Fact]
    public void AggregateBonuses_SumsFromMultipleSlots()
    {
        var weapon = MakeWeapon(meleeStr: 10);
        var body = MakeBody(stabDef: 15);

        _equipment.Equip(weapon);
        _equipment.Equip(body);

        Assert.Equal(10, _equipment.TotalMeleeStrengthBonus);
        Assert.Equal(20, _equipment.TotalStabAttackBonus); // from weapon
        Assert.Equal(15, _equipment.TotalStabDefenceBonus); // from body
        Assert.Equal(12, _equipment.TotalSlashDefenceBonus); // from body
    }

    [Fact]
    public void AggregateBonuses_EmptyEquipment_AllZero()
    {
        Assert.Equal(0, _equipment.TotalStabAttackBonus);
        Assert.Equal(0, _equipment.TotalMeleeStrengthBonus);
        Assert.Equal(0, _equipment.TotalStabDefenceBonus);
        Assert.Equal(0, _equipment.TotalPrayerBonus);
    }

    [Fact]
    public void WeaponAttackSpeed_DefaultsFist_Returns4()
    {
        Assert.Equal(4, _equipment.WeaponAttackSpeed);
    }

    [Fact]
    public void WeaponAttackSpeed_WithWeapon_ReturnsWeaponSpeed()
    {
        var weapon = MakeWeapon(attackSpeed: 6);
        _equipment.Equip(weapon);

        Assert.Equal(6, _equipment.WeaponAttackSpeed);
    }

    [Fact]
    public void GetAllEquipped_ReturnsAllItems()
    {
        var weapon = MakeWeapon();
        var body = MakeBody();

        _equipment.Equip(weapon);
        _equipment.Equip(body);

        var all = _equipment.GetAllEquipped().ToList();
        Assert.Equal(2, all.Count);
        Assert.Contains(weapon, all);
        Assert.Contains(body, all);
    }

    [Fact]
    public void GetAllEquippedWithSlots_IncludesSlotInfo()
    {
        var weapon = MakeWeapon();
        _equipment.Equip(weapon);

        var slots = _equipment.GetAllEquippedWithSlots().ToList();
        Assert.Single(slots);
        Assert.Equal(EquipmentSlot.Weapon, slots[0].Slot);
        Assert.Equal(weapon, slots[0].Item);
    }

    [Fact]
    public void UnequipAll_ReturnsAllAndClears()
    {
        var weapon = MakeWeapon();
        var body = MakeBody();
        _equipment.Equip(weapon);
        _equipment.Equip(body);

        var items = _equipment.UnequipAll();

        Assert.Equal(2, items.Count);
        Assert.False(_equipment.IsSlotOccupied(EquipmentSlot.Weapon));
        Assert.False(_equipment.IsSlotOccupied(EquipmentSlot.Body));
        Assert.Empty(_equipment.GetAllEquipped());
    }

    [Fact]
    public void IsSlotOccupied_FalseBeforeEquip_TrueAfter()
    {
        Assert.False(_equipment.IsSlotOccupied(EquipmentSlot.Head));

        var helm = TestFactory.CreateStubEquippable(new ItemId(300), "Helm", EquipmentSlot.Head);
        _equipment.Equip(helm);

        Assert.True(_equipment.IsSlotOccupied(EquipmentSlot.Head));
    }
}
