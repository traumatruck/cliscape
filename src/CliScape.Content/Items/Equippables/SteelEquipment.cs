using CliScape.Core.Items;

namespace CliScape.Content.Items.Equippables;

/// <summary>
///     Steel tier equipment - requires level 5 Attack/Defence.
///     Stats sourced from OSRS Wiki.
/// </summary>
public static class SteelEquipment
{
    public static readonly IEquippable Dagger = new EquippableItem
    {
        Id = ItemIds.SteelDagger,
        Name = new ItemName("Steel dagger"),
        ExamineText = "A steel dagger.",
        BaseValue = 75,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 5,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 8,
            SlashAttackBonus = 4,
            CrushAttackBonus = -4,
            MeleeStrengthBonus = 7,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Sword = new EquippableItem
    {
        Id = ItemIds.SteelSword,
        Name = new ItemName("Steel sword"),
        ExamineText = "A steel sword.",
        BaseValue = 195,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 5,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 10,
            SlashAttackBonus = 6,
            CrushAttackBonus = -2,
            MeleeStrengthBonus = 12,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Scimitar = new EquippableItem
    {
        Id = ItemIds.SteelScimitar,
        Name = new ItemName("Steel scimitar"),
        ExamineText = "A curved steel sword.",
        BaseValue = 400,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 5,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 3,
            SlashAttackBonus = 15,
            CrushAttackBonus = -2,
            MeleeStrengthBonus = 14,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Axe = new EquippableItem
    {
        Id = ItemIds.SteelAxe,
        Name = new ItemName("Steel axe"),
        ExamineText = "A steel axe.",
        BaseValue = 200,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 5,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = 9,
            CrushAttackBonus = 7,
            MeleeStrengthBonus = 10,
            AttackSpeed = 5
        }
    };

    public static readonly IEquippable FullHelm = new EquippableItem
    {
        Id = ItemIds.SteelFullHelm,
        Name = new ItemName("Steel full helm"),
        ExamineText = "A full steel helmet.",
        BaseValue = 550,
        Slot = EquipmentSlot.Head,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 9,
            SlashDefenceBonus = 10,
            CrushDefenceBonus = 7,
            RangedDefenceBonus = -1,
            MagicDefenceBonus = -3
        }
    };

    public static readonly IEquippable Platebody = new EquippableItem
    {
        Id = ItemIds.SteelPlatebody,
        Name = new ItemName("Steel platebody"),
        ExamineText = "Provides good protection.",
        BaseValue = 2000,
        Slot = EquipmentSlot.Body,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 32,
            SlashDefenceBonus = 31,
            CrushDefenceBonus = 24,
            RangedDefenceBonus = -6,
            MagicDefenceBonus = -15
        }
    };

    public static readonly IEquippable Chainbody = new EquippableItem
    {
        Id = ItemIds.SteelChainbody,
        Name = new ItemName("Steel chainbody"),
        ExamineText = "A steel chainbody.",
        BaseValue = 750,
        Slot = EquipmentSlot.Body,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 19,
            SlashDefenceBonus = 23,
            CrushDefenceBonus = 14,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Platelegs = new EquippableItem
    {
        Id = ItemIds.SteelPlatelegs,
        Name = new ItemName("Steel platelegs"),
        ExamineText = "Steel platelegs.",
        BaseValue = 1000,
        Slot = EquipmentSlot.Legs,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 16,
            SlashDefenceBonus = 15,
            CrushDefenceBonus = 13,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Kiteshield = new EquippableItem
    {
        Id = ItemIds.SteelKiteshield,
        Name = new ItemName("Steel kiteshield"),
        ExamineText = "A steel kiteshield.",
        BaseValue = 850,
        Slot = EquipmentSlot.Shield,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 10,
            SlashDefenceBonus = 14,
            CrushDefenceBonus = 13,
            RangedDefenceBonus = -1,
            MagicDefenceBonus = -3
        }
    };
}