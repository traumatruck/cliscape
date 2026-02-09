using CliScape.Core.Players;

namespace CliScape.Core.Items.Actions;

/// <summary>
///     Generic use action for items with custom behavior.
/// </summary>
public class UseAction : IItemAction
{
    private readonly Func<IItem, Player, string> _executeFunc;

    /// <summary>
    ///     Creates a new use action with custom execution logic.
    /// </summary>
    /// <param name="description">Description of what the use action does.</param>
    /// <param name="executeFunc">Function that executes the action.</param>
    /// <param name="consumesItem">Whether using the item consumes it.</param>
    public UseAction(string description, Func<IItem, Player, string> executeFunc, bool consumesItem = false)
    {
        Description = description;
        _executeFunc = executeFunc;
        ConsumesItem = consumesItem;
    }

    /// <summary>
    ///     Creates a default "Nothing interesting happens" use action.
    /// </summary>
    public static UseAction Default { get; } = new(
        "Use the item",
        (item, _) => $"Nothing interesting happens when you use the {item.Name}.");

    /// <inheritdoc />
    public ItemAction ActionType => ItemAction.Use;

    /// <inheritdoc />
    public string Description { get; }

    /// <inheritdoc />
    public bool ConsumesItem { get; }

    /// <inheritdoc />
    public string Execute(IItem item, Player player)
    {
        return _executeFunc(item, player);
    }
}