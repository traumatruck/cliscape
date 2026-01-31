using CliScape.Core.Players;

namespace CliScape.Core.Items.Actions;

/// <summary>
///     Action that allows examining an item to see its description.
/// </summary>
public class ExamineAction : IItemAction
{
    /// <summary>
    ///     Singleton instance of the examine action.
    /// </summary>
    public static ExamineAction Instance { get; } = new();

    private ExamineAction()
    {
    }

    /// <inheritdoc />
    public ItemAction ActionType => ItemAction.Examine;

    /// <inheritdoc />
    public string Description => "Examine the item";

    /// <inheritdoc />
    public bool ConsumesItem => false;

    /// <inheritdoc />
    public string Execute(IItem item, Player player)
    {
        return item.ExamineText;
    }
}
