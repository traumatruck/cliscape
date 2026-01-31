using CliScape.Core.Players;

namespace CliScape.Core.Items.Actions;

/// <summary>
///     Action that allows a book or scroll to be read.
/// </summary>
public class ReadAction : IItemAction
{
    private readonly string _readText;

    /// <summary>
    ///     Creates a new read action with the specified text content.
    /// </summary>
    /// <param name="readText">The text content to display when read.</param>
    /// <param name="consumesItem">Whether reading consumes the item (e.g., one-time scrolls).</param>
    public ReadAction(string readText, bool consumesItem = false)
    {
        _readText = readText;
        ConsumesItem = consumesItem;
    }

    /// <inheritdoc />
    public ItemAction ActionType => ItemAction.Read;

    /// <inheritdoc />
    public string Description => "Read the text";

    /// <inheritdoc />
    public bool ConsumesItem { get; }

    /// <inheritdoc />
    public string Execute(IItem item, Player player)
    {
        return _readText;
    }
}