using CliScape.Core.Items;

namespace CliScape.Content.Items.Equippables;

/// <summary>
///     Iron tier equipment - requires level 1 Attack/Defence.
///     Stats sourced from OSRS Wiki.
/// </summary>
public static class IronEquipment
{
    public static readonly IEquippable Dagger = new EquippableItem
    {
        Id = ItemIds.IronDagger,
        Name = new ItemName("Iron dagger"),
        ExamineText = "An iron dagger.",
        BaseValue = 21,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 1,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 5,
            SlashAttackBonus = 3,
            CrushAttackBonus = -4,
            MeleeStrengthBonus = 4,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Sword = new EquippableItem
    {
        Id = ItemIds.IronSword,
        Name = new ItemName("Iron sword"),
        ExamineText = "An iron sword.",
        BaseValue = 56,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 1,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 6,
            SlashAttackBonus = 4,
            CrushAttackBonus = -2,
            MeleeStrengthBonus = 7,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Scimitar = new EquippableItem
    {
        Id = ItemIds.IronScimitar,
        Name = new ItemName("Iron scimitar"),
        ExamineText = "A curved iron sword.",
        BaseValue = 112,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 1,
        Stats = new EquipmentStats
        {
            StabAttackBonus = 2,
            SlashAttackBonus = 10,
            CrushAttackBonus = -2,
            MeleeStrengthBonus = 9,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable FullHelm = new EquippableItem
    {
        Id = ItemIds.IronFullHelm,
        Name = new ItemName("Iron full helm"),
        ExamineText = "A full iron helmet.",
        BaseValue = 154,
        Slot = EquipmentSlot.Head,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 6,
            SlashDefenceBonus = 7,
            CrushDefenceBonus = 5,
            RangedDefenceBonus = -1,
            MagicDefenceBonus = -3
        }
    };

    public static readonly IEquippable Platebody = new EquippableItem
    {
        Id = ItemIds.IronPlatebody,
        Name = new ItemName("Iron platebody"),
        ExamineText = "Provides good protection.",
        BaseValue = 560,
        Slot = EquipmentSlot.Body,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 21,
            SlashDefenceBonus = 20,
            CrushDefenceBonus = 14,
            RangedDefenceBonus = -6,
            MagicDefenceBonus = -15
        }
    };

    public static readonly IEquippable Chainbody = new EquippableItem
    {
        Id = ItemIds.IronChainbody,
        Name = new ItemName("Iron chainbody"),
        ExamineText = "An iron chainbody.",
        BaseValue = 210,
        Slot = EquipmentSlot.Body,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 13,
            SlashDefenceBonus = 16,
            CrushDefenceBonus = 9,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Platelegs = new EquippableItem
    {
        Id = ItemIds.IronPlatelegs,
        Name = new ItemName("Iron platelegs"),
        ExamineText = "Iron platelegs.",
        BaseValue = 280,
        Slot = EquipmentSlot.Legs,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 10,
            SlashDefenceBonus = 9,
            CrushDefenceBonus = 7,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable Kiteshield = new EquippableItem
    {
        Id = ItemIds.IronKiteshield,
        Name = new ItemName("Iron kiteshield"),
        ExamineText = "An iron kiteshield.",
        BaseValue = 238,
        Slot = EquipmentSlot.Shield,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 7,
            SlashDefenceBonus = 10,
            CrushDefenceBonus = 8,
            RangedDefenceBonus = -1,
            MagicDefenceBonus = -3
        }
    };

    public static readonly IEquippable Axe = new EquippableItem
    {
        Id = ItemIds.IronAxe,
        Name = new ItemName("Iron axe"),
        ExamineText = "An iron axe.",
        BaseValue = 56,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 1,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = 6,
            CrushAttackBonus = 4,
            MeleeStrengthBonus = 7,
            AttackSpeed = 5
        }
    };

    public static readonly IEquippable Mace = new EquippableItem
    {
        Id = ItemIds.IronMace,
        Name = new ItemName("Iron mace"),
        ExamineText = "An iron mace.",
        BaseValue = 63,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 1,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = -2,
            CrushAttackBonus = 9,
            MeleeStrengthBonus = 7,
            PrayerBonus = 1,
            AttackSpeed = 4
        }
    };

    public static readonly IEquippable Hatchet = new EquippableItem
    {
        Id = ItemIds.IronHatchet,
        Name = new ItemName("Iron hatchet"),
        ExamineText = "An iron hatchet for cutting trees.",
        BaseValue = 56,
        Slot = EquipmentSlot.Weapon,
        RequiredAttackLevel = 1,
        Stats = new EquipmentStats
        {
            StabAttackBonus = -2,
            SlashAttackBonus = 6,
            CrushAttackBonus = 4,
            MeleeStrengthBonus = 7,
            AttackSpeed = 5
        }
    };

    public static readonly IEquippable MedHelm = new EquippableItem
    {
        Id = ItemIds.IronMedHelm,
        Name = new ItemName("Iron med helm"),
        ExamineText = "A medium iron helmet.",
        BaseValue = 84,
        Slot = EquipmentSlot.Head,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 4,
            SlashDefenceBonus = 5,
            CrushDefenceBonus = 3,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Plateskirt = new EquippableItem
    {
        Id = ItemIds.IronPlateskirt,
        Name = new ItemName("Iron plateskirt"),
        ExamineText = "An iron plateskirt.",
        BaseValue = 280,
        Slot = EquipmentSlot.Legs,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 10,
            SlashDefenceBonus = 9,
            CrushDefenceBonus = 7,
            RangedDefenceBonus = -3,
            MagicDefenceBonus = -7
        }
    };

    public static readonly IEquippable SqShield = new EquippableItem
    {
        Id = ItemIds.IronSqShield,
        Name = new ItemName("Iron sq shield"),
        ExamineText = "An iron square shield.",
        BaseValue = 119,
        Slot = EquipmentSlot.Shield,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 5,
            SlashDefenceBonus = 7,
            CrushDefenceBonus = 6,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Boots = new EquippableItem
    {
        Id = ItemIds.IronBoots,
        Name = new ItemName("Iron boots"),
        ExamineText = "A pair of iron boots.",
        BaseValue = 56,
        Slot = EquipmentSlot.Feet,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 2,
            SlashDefenceBonus = 3,
            CrushDefenceBonus = 2,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };

    public static readonly IEquippable Gloves = new EquippableItem
    {
        Id = ItemIds.IronGloves,
        Name = new ItemName("Iron gloves"),
        ExamineText = "A pair of iron gloves.",
        BaseValue = 56,
        Slot = EquipmentSlot.Hands,
        RequiredDefenceLevel = 1,
        Stats = new EquipmentStats
        {
            StabDefenceBonus = 2,
            SlashDefenceBonus = 2,
            CrushDefenceBonus = 2,
            RangedDefenceBonus = 0,
            MagicDefenceBonus = -1
        }
    };
}