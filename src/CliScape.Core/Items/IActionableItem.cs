namespace CliScape.Core.Items;

/// <summary>
///     Interface for items that support inventory actions beyond examine/drop.
/// </summary>
public interface IActionableItem : IItem
{
    /// <summary>
    ///     The action instances available for this item.
    /// </summary>
    IReadOnlyList<IItemAction> Actions { get; }

    /// <summary>
    ///     The action types available for this item (derived from Actions).
    /// </summary>
    IReadOnlyList<ItemAction> AvailableActions { get; }

    /// <summary>
    ///     Checks if this item supports the specified action type.
    /// </summary>
    bool SupportsAction(ItemAction action);

    /// <summary>
    ///     Gets the action instance for the specified action type.
    /// </summary>
    /// <param name="action">The action type to retrieve.</param>
    /// <returns>The action instance, or null if not supported.</returns>
    IItemAction? GetAction(ItemAction action);
}
