using CliScape.Core.Players;

namespace CliScape.Core.Items.Actions;

/// <summary>
///     Action that allows an item to be eaten to restore hitpoints.
/// </summary>
public class EatAction : IItemAction
{
    /// <summary>
    ///     Creates a new eat action with the specified heal amount.
    /// </summary>
    /// <param name="healAmount">The amount of hitpoints restored.</param>
    public EatAction(int healAmount)
    {
        HealAmount = healAmount;
    }

    /// <summary>
    ///     The amount of hitpoints restored when eating.
    /// </summary>
    public int HealAmount { get; }

    /// <inheritdoc />
    public ItemAction ActionType => ItemAction.Eat;

    /// <inheritdoc />
    public string Description => $"Eat to restore {HealAmount} hitpoints";

    /// <inheritdoc />
    public bool ConsumesItem => true;

    /// <inheritdoc />
    public string Execute(IItem item, Player player)
    {
        var previousHealth = player.CurrentHealth;
        player.Heal(HealAmount);
        var actualHealed = player.CurrentHealth - previousHealth;

        if (actualHealed == 0)
        {
            return $"You eat the {item.Name}. You are already at full health.";
        }

        return $"You eat the {item.Name}. It heals {actualHealed} hitpoints.";
    }
}