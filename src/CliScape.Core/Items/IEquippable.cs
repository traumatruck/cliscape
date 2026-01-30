namespace CliScape.Core.Items;

/// <summary>
///     Interface for items that can be equipped by a player.
/// </summary>
public interface IEquippable : IItem
{
    /// <summary>
    ///     The equipment slot this item occupies.
    /// </summary>
    EquipmentSlot Slot { get; }

    /// <summary>
    ///     The combat stats provided by this equipment.
    /// </summary>
    EquipmentStats Stats { get; }

    /// <summary>
    ///     Required Attack level to equip (for weapons).
    /// </summary>
    int RequiredAttackLevel { get; }

    /// <summary>
    ///     Required Strength level to equip.
    /// </summary>
    int RequiredStrengthLevel { get; }

    /// <summary>
    ///     Required Defence level to equip (for armor).
    /// </summary>
    int RequiredDefenceLevel { get; }

    /// <summary>
    ///     Required Ranged level to equip.
    /// </summary>
    int RequiredRangedLevel { get; }

    /// <summary>
    ///     Required Magic level to equip.
    /// </summary>
    int RequiredMagicLevel { get; }
}