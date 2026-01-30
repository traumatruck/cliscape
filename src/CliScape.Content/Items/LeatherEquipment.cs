using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
/// Leather armor - low requirements, good for ranged.
/// Stats sourced from OSRS Wiki.
/// </summary>
public static class LeatherEquipment
{
    public static readonly IEquippable Body = new EquippableItem
    {
        Id = ItemIds.LeatherBody,
        Name = new ItemName("Leather body"),
        ExamineText = "Leather body armour.",
        BaseValue = 21,
        Slot = EquipmentSlot.Body,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 8,
            SlashDefenceBonus = 9,
            CrushDefenceBonus = 7,
            RangedDefenceBonus = 8,
            MagicDefenceBonus = 8,
            RangedAttackBonus = 2
        }
    };

    public static readonly IEquippable Chaps = new EquippableItem
    {
        Id = ItemIds.LeatherChaps,
        Name = new ItemName("Leather chaps"),
        ExamineText = "Leather leg armour.",
        BaseValue = 14,
        Slot = EquipmentSlot.Legs,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 4,
            SlashDefenceBonus = 3,
            CrushDefenceBonus = 4,
            RangedDefenceBonus = 4,
            MagicDefenceBonus = 4,
            RangedAttackBonus = 2
        }
    };

    public static readonly IEquippable Gloves = new EquippableItem
    {
        Id = ItemIds.LeatherGloves,
        Name = new ItemName("Leather gloves"),
        ExamineText = "Leather gloves.",
        BaseValue = 6,
        Slot = EquipmentSlot.Hands,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 1,
            SlashDefenceBonus = 1,
            CrushDefenceBonus = 2,
            RangedDefenceBonus = 1,
            MagicDefenceBonus = 1
        }
    };

    public static readonly IEquippable Boots = new EquippableItem
    {
        Id = ItemIds.LeatherBoots,
        Name = new ItemName("Leather boots"),
        ExamineText = "Leather boots.",
        BaseValue = 6,
        Slot = EquipmentSlot.Feet,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 1,
            SlashDefenceBonus = 1,
            CrushDefenceBonus = 2,
            RangedDefenceBonus = 1,
            MagicDefenceBonus = 0
        }
    };

    public static readonly IEquippable Cowl = new EquippableItem
    {
        Id = ItemIds.LeatherCowl,
        Name = new ItemName("Leather cowl"),
        ExamineText = "A leather cowl.",
        BaseValue = 9,
        Slot = EquipmentSlot.Head,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 2,
            SlashDefenceBonus = 3,
            CrushDefenceBonus = 2,
            RangedDefenceBonus = 4,
            MagicDefenceBonus = 2,
            RangedAttackBonus = 1
        }
    };

    public static readonly IEquippable Vambraces = new EquippableItem
    {
        Id = ItemIds.LeatherVambraces,
        Name = new ItemName("Leather vambraces"),
        ExamineText = "Leather arm guards.",
        BaseValue = 10,
        Slot = EquipmentSlot.Hands,
        Stats = new EquipmentStats
        {
            RangedAttackBonus = 4,
            RangedDefenceBonus = 2
        }
    };

    public static readonly IEquippable HardleatherBody = new EquippableItem
    {
        Id = ItemIds.HardleatherBody,
        Name = new ItemName("Hardleather body"),
        ExamineText = "Hardened leather body armour.",
        BaseValue = 45,
        Slot = EquipmentSlot.Body,
        RequiredDefenceLevel = 10,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 12,
            SlashDefenceBonus = 14,
            CrushDefenceBonus = 10,
            RangedDefenceBonus = 10,
            MagicDefenceBonus = 10,
            RangedAttackBonus = 3
        }
    };

    public static readonly IItem[] All =
    [
        Body, Chaps, Gloves, Boots, Cowl, Vambraces, HardleatherBody
    ];
}
