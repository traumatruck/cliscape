using CliScape.Core.Items;

namespace CliScape.Content.Items.Equippables;

/// <summary>
///     Miscellaneous equipment items.
/// </summary>
public static class MiscEquipment
{
    public static readonly IEquippable WoodenShield = new EquippableItem
    {
        Id = ItemIds.WoodenShield,
        Name = new ItemName("Wooden shield"),
        ExamineText = "A basic wooden shield.",
        BaseValue = 20,
        Slot = EquipmentSlot.Shield,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 2,
            SlashDefenceBonus = 3,
            CrushDefenceBonus = 2,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = 0
        }
    };

    public static readonly IEquippable Shortbow = new EquippableItem
    {
        Id = ItemIds.Shortbow,
        Name = new ItemName("Shortbow"),
        ExamineText = "Short and effective.",
        BaseValue = 50,
        Slot = EquipmentSlot.Weapon,
        Stats = new EquipmentStats
        {
            RangedAttackBonus = 8,
            AttackSpeed = 5
        }
    };

    public static readonly IEquippable BronzePickaxe = new EquippableItem
    {
        Id = ItemIds.BronzePickaxe,
        Name = new ItemName("Bronze pickaxe"),
        ExamineText = "A pickaxe for mining.",
        BaseValue = 1,
        Slot = EquipmentSlot.Weapon,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 1,
            CrushAttackBonus = 1,
            MeleeStrengthBonus = 2,
            AttackSpeed = 5
        }
    };
}