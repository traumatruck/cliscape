using CliScape.Core.Players;

namespace CliScape.Core.Items;

/// <summary>
///     Represents an executable action that can be performed on an item.
/// </summary>
public interface IItemAction
{
    /// <summary>
    ///     The type of action this represents.
    /// </summary>
    ItemAction ActionType { get; }

    /// <summary>
    ///     A human-readable description of what this action does.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Whether this action consumes the item when executed.
    /// </summary>
    bool ConsumesItem { get; }

    /// <summary>
    ///     Executes the action on the given item for the specified player.
    /// </summary>
    /// <param name="item">The item the action is being performed on.</param>
    /// <param name="player">The player performing the action.</param>
    /// <returns>A result message describing what happened.</returns>
    string Execute(IItem item, Player player);
}