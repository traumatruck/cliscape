using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Ammunition items for ranged combat.
///     Stats sourced from OSRS Wiki.
/// </summary>
public static class Ammunition
{
    public static readonly IEquippable BronzeArrow = new EquippableItem
    {
        Id = ItemIds.BronzeArrow,
        Name = new ItemName("Bronze arrow"),
        ExamineText = "Arrows with bronze heads.",
        BaseValue = 1,
        IsStackable = true,
        Slot = EquipmentSlot.Ammo,
        Stats = new EquipmentStats
        {
            RangedStrengthBonus = 7
        }
    };
}