using CliScape.Core.Items.Actions;

namespace CliScape.Core.Items;

/// <summary>
///     Base class for items that support dynamic actions.
/// </summary>
public class ActionableItem : Item, IActionableItem
{
    private readonly List<IItemAction> _actions = [ExamineAction.Instance];
    private IReadOnlyList<ItemAction>? _availableActions;

    /// <inheritdoc />
    public IReadOnlyList<IItemAction> Actions => _actions;

    /// <inheritdoc />
    public IReadOnlyList<ItemAction> AvailableActions =>
        _availableActions ??= _actions.Select(a => a.ActionType).ToList();

    /// <inheritdoc />
    public bool SupportsAction(ItemAction action)
    {
        return _actions.Any(a => a.ActionType == action);
    }

    /// <inheritdoc />
    public IItemAction? GetAction(ItemAction action)
    {
        return _actions.FirstOrDefault(a => a.ActionType == action);
    }

    /// <summary>
    ///     Adds an action to this item.
    /// </summary>
    /// <param name="action">The action to add.</param>
    /// <returns>This item for method chaining.</returns>
    public ActionableItem WithAction(IItemAction action)
    {
        _actions.Add(action);
        _availableActions = null; // Invalidate cache
        return this;
    }

    /// <summary>
    ///     Adds multiple actions to this item.
    /// </summary>
    /// <param name="actions">The actions to add.</param>
    /// <returns>This item for method chaining.</returns>
    public ActionableItem WithActions(params IItemAction[] actions)
    {
        _actions.AddRange(actions);
        _availableActions = null; // Invalidate cache
        return this;
    }
}