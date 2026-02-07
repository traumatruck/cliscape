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

    public static readonly IEquippable Mace = new EquippableItem
    {
        Id = ItemIds.SteelMace,
        Name = new ItemName("Steel mace"),
        ExamineText = "A steel mace.",
        BaseValue = 230,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 5,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = -2,
            CrushAttackBonus = 13,
            MeleeStrengthBonus = 12,
            PrayerBonus = 1,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Hatchet = new EquippableItem
    {
        Id = ItemIds.SteelHatchet,
        Name = new ItemName("Steel hatchet"),
        ExamineText = "A steel hatchet for cutting trees.",
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

    public static readonly IEquippable MedHelm = new EquippableItem
    {
        Id = ItemIds.SteelMedHelm,
        Name = new ItemName("Steel med helm"),
        ExamineText = "A medium steel helmet.",
        BaseValue = 300,
        Slot = EquipmentSlot.Head,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 6,
            SlashDefenceBonus = 7,
            CrushDefenceBonus = 5,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Plateskirt = new EquippableItem
    {
        Id = ItemIds.SteelPlateskirt,
        Name = new ItemName("Steel plateskirt"),
        ExamineText = "A steel plateskirt.",
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

    public static readonly IEquippable SqShield = new EquippableItem
    {
        Id = ItemIds.SteelSqShield,
        Name = new ItemName("Steel sq shield"),
        ExamineText = "A steel square shield.",
        BaseValue = 425,
        Slot = EquipmentSlot.Shield,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 7,
            SlashDefenceBonus = 9,
            CrushDefenceBonus = 8,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Boots = new EquippableItem
    {
        Id = ItemIds.SteelBoots,
        Name = new ItemName("Steel boots"),
        ExamineText = "A pair of steel boots.",
        BaseValue = 200,
        Slot = EquipmentSlot.Feet,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 4,
            SlashDefenceBonus = 5,
            CrushDefenceBonus = 4,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Gloves = new EquippableItem
    {
        Id = ItemIds.SteelGloves,
        Name = new ItemName("Steel gloves"),
        ExamineText = "A pair of steel gloves.",
        BaseValue = 200,
        Slot = EquipmentSlot.Hands,
        RequiredDefenceLevel = 5,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 3,
            SlashDefenceBonus = 4,
            CrushDefenceBonus = 3,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };
}