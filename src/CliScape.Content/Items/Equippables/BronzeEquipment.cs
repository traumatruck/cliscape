using CliScape.Core.Items;

namespace CliScape.Content.Items.Equippables;

/// <summary>
///     Bronze tier equipment - no requirements, lowest stats.
///     Stats sourced from OSRS Wiki.
/// </summary>
public static class BronzeEquipment
{
    public static readonly IEquippable Dagger = new EquippableItem
    {
        Id = ItemIds.BronzeDagger,
        Name = new ItemName("Bronze dagger"),
        ExamineText = "A bronze dagger.",
        BaseValue = 10,
        Slot = EquipmentSlot.Weapon,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 4,
            SlashAttackBonus = 2,
            CrushAttackBonus = -4,
            MeleeStrengthBonus = 3,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Sword = new EquippableItem
    {
        Id = ItemIds.BronzeSword,
        Name = new ItemName("Bronze sword"),
        ExamineText = "A bronze sword.",
        BaseValue = 26,
        Slot = EquipmentSlot.Weapon,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 4,
            SlashAttackBonus = 3,
            CrushAttackBonus = -2,
            MeleeStrengthBonus = 5,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Scimitar = new EquippableItem
    {
        Id = ItemIds.BronzeScimitar,
        Name = new ItemName("Bronze scimitar"),
        ExamineText = "A curved bronze sword.",
        BaseValue = 32,
        Slot = EquipmentSlot.Weapon,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 1,
            SlashAttackBonus = 7,
            CrushAttackBonus = -2,
            MeleeStrengthBonus = 6,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Axe = new EquippableItem
    {
        Id = ItemIds.BronzeAxe,
        Name = new ItemName("Bronze axe"),
        ExamineText = "A bronze axe.",
        BaseValue = 16,
        Slot = EquipmentSlot.Weapon,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = 4,
            CrushAttackBonus = 2,
            MeleeStrengthBonus = 4,
            AttackSpeed = 5
        }
    };

    public static readonly IEquippable Mace = new EquippableItem
    {
        Id = ItemIds.BronzeMace,
        Name = new ItemName("Bronze mace"),
        ExamineText = "A bronze mace.",
        BaseValue = 18,
        Slot = EquipmentSlot.Weapon,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = -2,
            CrushAttackBonus = 7,
            MeleeStrengthBonus = 5,
            PrayerBonus = 1,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable FullHelm = new EquippableItem
    {
        Id = ItemIds.BronzeFullHelm,
        Name = new ItemName("Bronze full helm"),
        ExamineText = "A full bronze helmet.",
        BaseValue = 44,
        Slot = EquipmentSlot.Head,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 4,
            SlashDefenceBonus = 5,
            CrushDefenceBonus = 3,
            RangedDefenceBonus = -1,
            MagicDefenceBonus = -3
        }
    };

    public static readonly IEquippable MedHelm = new EquippableItem
    {
        Id = ItemIds.BronzeMedHelm,
        Name = new ItemName("Bronze med helm"),
        ExamineText = "A medium bronze helmet.",
        BaseValue = 24,
        Slot = EquipmentSlot.Head,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 3,
            SlashDefenceBonus = 4,
            CrushDefenceBonus = 2,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Platebody = new EquippableItem
    {
        Id = ItemIds.BronzePlatebody,
        Name = new ItemName("Bronze platebody"),
        ExamineText = "Provides good protection.",
        BaseValue = 160,
        Slot = EquipmentSlot.Body,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 15,
            SlashDefenceBonus = 14,
            CrushDefenceBonus = 9,
            RangedDefenceBonus = -6,
            MagicDefenceBonus = -15
        }
    };

    public static readonly IEquippable Chainbody = new EquippableItem
    {
        Id = ItemIds.BronzeChainbody,
        Name = new ItemName("Bronze chainbody"),
        ExamineText = "A bronze chainbody.",
        BaseValue = 60,
        Slot = EquipmentSlot.Body,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 8,
            SlashDefenceBonus = 11,
            CrushDefenceBonus = 5,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Platelegs = new EquippableItem
    {
        Id = ItemIds.BronzePlatelegs,
        Name = new ItemName("Bronze platelegs"),
        ExamineText = "Bronze platelegs.",
        BaseValue = 80,
        Slot = EquipmentSlot.Legs,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 7,
            SlashDefenceBonus = 6,
            CrushDefenceBonus = 5,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Plateskirt = new EquippableItem
    {
        Id = ItemIds.BronzePlateskirt,
        Name = new ItemName("Bronze plateskirt"),
        ExamineText = "A bronze plateskirt.",
        BaseValue = 80,
        Slot = EquipmentSlot.Legs,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 7,
            SlashDefenceBonus = 6,
            CrushDefenceBonus = 5,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Kiteshield = new EquippableItem
    {
        Id = ItemIds.BronzeKiteshield,
        Name = new ItemName("Bronze kiteshield"),
        ExamineText = "A bronze kiteshield.",
        BaseValue = 68,
        Slot = EquipmentSlot.Shield,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 5,
            SlashDefenceBonus = 7,
            CrushDefenceBonus = 6,
            RangedDefenceBonus = -1,
            MagicDefenceBonus = -3
        }
    };

    public static readonly IEquippable SqShield = new EquippableItem
    {
        Id = ItemIds.BronzeSqShield,
        Name = new ItemName("Bronze sq shield"),
        ExamineText = "A bronze square shield.",
        BaseValue = 32,
        Slot = EquipmentSlot.Shield,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 3,
            SlashDefenceBonus = 5,
            CrushDefenceBonus = 4,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };
}