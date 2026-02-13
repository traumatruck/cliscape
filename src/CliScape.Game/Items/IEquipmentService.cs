using CliScape.Core;
using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Game.Items;

/// <summary>
///     Handles equipment logic.
/// </summary>
public interface IEquipmentService
{
    /// <summary>
    ///     Validates if the player meets requirements to equip an item.
    /// </summary>
    ServiceResult CanEquip(Player player, IEquippable item);

    /// <summary>
    ///     Equips an item from the player's inventory.
    /// </summary>
    EquipResult Equip(Player player, IEquippable item);

    /// <summary>
    ///     Unequips an item from the specified slot.
    /// </summary>
    EquipResult Unequip(Player player, EquipmentSlot slot);
}