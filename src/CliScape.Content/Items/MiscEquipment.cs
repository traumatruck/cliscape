using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
/// Miscellaneous equipment items.
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

    public static readonly IItem[] All =
    [
        WoodenShield
    ];
}
