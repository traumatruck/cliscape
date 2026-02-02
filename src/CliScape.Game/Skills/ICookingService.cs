using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;

namespace CliScape.Game.Skills;

/// <summary>
///     Handles cooking logic.
/// </summary>
public interface ICookingService
{
    /// <summary>
    ///     Validates if the player can cook the specified recipe.
    /// </summary>
    (bool CanCook, string? ErrorMessage) CanCook(Player player, int requiredLevel);

    /// <summary>
    ///     Performs cooking of the specified recipe.
    /// </summary>
    CookingResult Cook(
        Player player,
        ICookingRange range,
        CookingRecipe recipe,
        int count,
        Func<ItemId, IItem?> itemResolver);
}
