using CliScape.Core.Players;

namespace CliScape.Core.Items.Actions;

/// <summary>
///     Action that allows a potion to be drunk for its effects.
/// </summary>
public class DrinkAction : IItemAction
{
    private readonly Func<IItem, Player, string> _effectFunc;

    /// <summary>
    ///     Creates a new drink action with custom effects.
    /// </summary>
    /// <param name="description">Description of the potion's effects.</param>
    /// <param name="effectFunc">Function that applies the potion effects.</param>
    public DrinkAction(string description, Func<IItem, Player, string> effectFunc)
    {
        Description = description;
        _effectFunc = effectFunc;
    }

    /// <inheritdoc />
    public ItemAction ActionType => ItemAction.Drink;

    /// <inheritdoc />
    public string Description { get; }

    /// <inheritdoc />
    public bool ConsumesItem => true;

    /// <inheritdoc />
    public string Execute(IItem item, Player player)
    {
        return _effectFunc(item, player);
    }
}