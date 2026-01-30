namespace CliScape.Core.Items;

/// <summary>
///     Base interface for all items in the game.
/// </summary>
public interface IItem
{
    /// <summary>
    ///     The unique identifier for this item type.
    /// </summary>
    ItemId Id { get; }

    /// <summary>
    ///     The display name of the item.
    /// </summary>
    ItemName Name { get; }

    /// <summary>
    ///     The examine text shown when a player examines this item.
    /// </summary>
    string ExamineText { get; }

    /// <summary>
    ///     The base value of the item in gold pieces.
    ///     Used for shop pricing calculations.
    /// </summary>
    int BaseValue { get; }

    /// <summary>
    ///     Whether this item can stack in inventory (like coins, runes, arrows).
    /// </summary>
    bool IsStackable { get; }

    /// <summary>
    ///     Whether this item can be traded with other players or sold to shops.
    /// </summary>
    bool IsTradeable { get; }
}