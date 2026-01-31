namespace CliScape.Core.Items;

/// <summary>
///     Actions that can be performed on items in the inventory.
/// </summary>
public enum ItemAction
{
    /// <summary>
    ///     Examine the item to see its description.
    /// </summary>
    Examine,

    /// <summary>
    ///     Generic use action for items with custom behavior.
    /// </summary>
    Use,

    /// <summary>
    ///     Eat the item to restore hitpoints (food items).
    /// </summary>
    Eat,

    /// <summary>
    ///     Bury the item to gain prayer experience (bones).
    /// </summary>
    Bury,

    /// <summary>
    ///     Drink the item (potions).
    /// </summary>
    Drink,

    /// <summary>
    ///     Read the item (books, scrolls).
    /// </summary>
    Read,

    /// <summary>
    ///     Equip the item (weapons, armor, accessories).
    /// </summary>
    Equip,

    /// <summary>
    ///     Wield the item (alias for Equip, typically used for weapons).
    /// </summary>
    Wield
}