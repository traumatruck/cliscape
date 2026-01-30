using CliScape.Core.Players;

namespace CliScape.Core.Items;

/// <summary>
///     A food item that can be eaten to restore hitpoints.
/// </summary>
public class EdibleItem : Item, IEdible
{
    private static readonly IReadOnlyList<ItemAction> EdibleActions = [ItemAction.Eat];

    /// <inheritdoc />
    public required int HealAmount { get; init; }

    /// <inheritdoc />
    public IReadOnlyList<ItemAction> AvailableActions => EdibleActions;

    /// <inheritdoc />
    public bool SupportsAction(ItemAction action) => action == ItemAction.Eat;

    /// <inheritdoc />
    public string Eat(Player player)
    {
        var previousHealth = player.CurrentHealth;
        player.Heal(HealAmount);
        var actualHealed = player.CurrentHealth - previousHealth;

        if (actualHealed == 0)
        {
            return $"You eat the {Name}. You are already at full health.";
        }

        return $"You eat the {Name}. It heals {actualHealed} hitpoints.";
    }
}
