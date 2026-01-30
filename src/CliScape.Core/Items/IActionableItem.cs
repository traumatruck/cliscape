namespace CliScape.Core.Items;

/// <summary>
///     Interface for items that support inventory actions beyond examine/drop.
/// </summary>
public interface IActionableItem : IItem
{
    /// <summary>
    ///     The actions available for this item.
    /// </summary>
    IReadOnlyList<ItemAction> AvailableActions { get; }

    /// <summary>
    ///     Checks if this item supports the specified action.
    /// </summary>
    bool SupportsAction(ItemAction action);
}
