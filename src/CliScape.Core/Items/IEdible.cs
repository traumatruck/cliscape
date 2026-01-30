using CliScape.Core.Players;

namespace CliScape.Core.Items;

/// <summary>
///     Interface for items that can be eaten to restore hitpoints.
/// </summary>
public interface IEdible : IActionableItem
{
    /// <summary>
    ///     The amount of hitpoints restored when eating this item.
    /// </summary>
    int HealAmount { get; }

    /// <summary>
    ///     Eats the item, applying its effects to the player.
    /// </summary>
    /// <param name="player">The player eating the item.</param>
    /// <returns>A message describing the result of eating.</returns>
    string Eat(Player player);
}
