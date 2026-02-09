using CliScape.Core.Items;

namespace CliScape.Content.Items.Equippables;

/// <summary>
///     Mithril tier equipment - requires level 20 Attack/Defence.
///     Stats sourced from OSRS Wiki.
/// </summary>
public static class MithrilEquipment
{
    public static readonly IEquippable Dagger = new EquippableItem
    {
        Id = ItemIds.MithrilDagger,
        Name = new ItemName("Mithril dagger"),
        ExamineText = "A mithril dagger.",
        BaseValue = 195,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 20,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 11,
            SlashAttackBonus = 5,
            CrushAttackBonus = -4,
            MeleeStrengthBonus = 10,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Sword = new EquippableItem
    {
        Id = ItemIds.MithrilSword,
        Name = new ItemName("Mithril sword"),
        ExamineText = "A mithril sword.",
        BaseValue = 520,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 20,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 14,
            SlashAttackBonus = 9,
            CrushAttackBonus = -2,
            MeleeStrengthBonus = 16,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Scimitar = new EquippableItem
    {
        Id = ItemIds.MithrilScimitar,
        Name = new ItemName("Mithril scimitar"),
        ExamineText = "A curved mithril sword.",
        BaseValue = 1040,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 20,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 3,
            SlashAttackBonus = 21,
            CrushAttackBonus = -2,
            MeleeStrengthBonus = 20,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Axe = new EquippableItem
    {
        Id = ItemIds.MithrilAxe,
        Name = new ItemName("Mithril axe"),
        ExamineText = "A mithril axe.",
        BaseValue = 520,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 20,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = 13,
            CrushAttackBonus = 11,
            MeleeStrengthBonus = 16,
            AttackSpeed = 5
        }
    };

    public static readonly IEquippable Mace = new EquippableItem
    {
        Id = ItemIds.MithrilMace,
        Name = new ItemName("Mithril mace"),
        ExamineText = "A mithril mace.",
        BaseValue = 585,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 20,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = -2,
            CrushAttackBonus = 17,
            MeleeStrengthBonus = 16,
            PrayerBonus = 1,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Hatchet = new EquippableItem
    {
        Id = ItemIds.MithrilHatchet,
        Name = new ItemName("Mithril hatchet"),
        ExamineText = "A mithril hatchet for cutting trees.",
        BaseValue = 520,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 20,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = 13,
            CrushAttackBonus = 11,
            MeleeStrengthBonus = 16,
            AttackSpeed = 5
        }
    };

    public static readonly IEquippable FullHelm = new EquippableItem
    {
        Id = ItemIds.MithrilFullHelm,
        Name = new ItemName("Mithril full helm"),
        ExamineText = "A full mithril helmet.",
        BaseValue = 1430,
        Slot = EquipmentSlot.Head,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 12,
            SlashDefenceBonus = 13,
            CrushDefenceBonus = 10,
            RangedDefenceBonus = -1,
            MagicDefenceBonus = -3
        }
    };

    public static readonly IEquippable MedHelm = new EquippableItem
    {
        Id = ItemIds.MithrilMedHelm,
        Name = new ItemName("Mithril med helm"),
        ExamineText = "A medium mithril helmet.",
        BaseValue = 780,
        Slot = EquipmentSlot.Head,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 9,
            SlashDefenceBonus = 10,
            CrushDefenceBonus = 7,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Platebody = new EquippableItem
    {
        Id = ItemIds.MithrilPlatebody,
        Name = new ItemName("Mithril platebody"),
        ExamineText = "Provides excellent protection.",
        BaseValue = 5200,
        Slot = EquipmentSlot.Body,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 46,
            SlashDefenceBonus = 44,
            CrushDefenceBonus = 36,
            RangedDefenceBonus = -6,
            MagicDefenceBonus = -15
        }
    };

    public static readonly IEquippable Chainbody = new EquippableItem
    {
        Id = ItemIds.MithrilChainbody,
        Name = new ItemName("Mithril chainbody"),
        ExamineText = "A mithril chainbody.",
        BaseValue = 1950,
        Slot = EquipmentSlot.Body,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 27,
            SlashDefenceBonus = 32,
            CrushDefenceBonus = 21,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Platelegs = new EquippableItem
    {
        Id = ItemIds.MithrilPlatelegs,
        Name = new ItemName("Mithril platelegs"),
        ExamineText = "Mithril platelegs.",
        BaseValue = 2600,
        Slot = EquipmentSlot.Legs,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 22,
            SlashDefenceBonus = 21,
            CrushDefenceBonus = 18,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Plateskirt = new EquippableItem
    {
        Id = ItemIds.MithrilPlateskirt,
        Name = new ItemName("Mithril plateskirt"),
        ExamineText = "A mithril plateskirt.",
        BaseValue = 2600,
        Slot = EquipmentSlot.Legs,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 22,
            SlashDefenceBonus = 21,
            CrushDefenceBonus = 18,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Kiteshield = new EquippableItem
    {
        Id = ItemIds.MithrilKiteshield,
        Name = new ItemName("Mithril kiteshield"),
        ExamineText = "A mithril kiteshield.",
        BaseValue = 2210,
        Slot = EquipmentSlot.Shield,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 14,
            SlashDefenceBonus = 18,
            CrushDefenceBonus = 17,
            RangedDefenceBonus = -1,
            MagicDefenceBonus = -3
        }
    };

    public static readonly IEquippable SqShield = new EquippableItem
    {
        Id = ItemIds.MithrilSqShield,
        Name = new ItemName("Mithril sq shield"),
        ExamineText = "A mithril square shield.",
        BaseValue = 1105,
        Slot = EquipmentSlot.Shield,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 10,
            SlashDefenceBonus = 12,
            CrushDefenceBonus = 11,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Boots = new EquippableItem
    {
        Id = ItemIds.MithrilBoots,
        Name = new ItemName("Mithril boots"),
        ExamineText = "A pair of mithril boots.",
        BaseValue = 520,
        Slot = EquipmentSlot.Feet,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 6,
            SlashDefenceBonus = 7,
            CrushDefenceBonus = 6,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Gloves = new EquippableItem
    {
        Id = ItemIds.MithrilGloves,
        Name = new ItemName("Mithril gloves"),
        ExamineText = "A pair of mithril gloves.",
        BaseValue = 520,
        Slot = EquipmentSlot.Hands,
        RequiredDefenceLevel = 20,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 5,
            SlashDefenceBonus = 5,
            CrushDefenceBonus = 5,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };
}