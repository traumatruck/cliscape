using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Handles smelting logic.
/// </summary>
public interface ISmeltingService
{
    /// <summary>
    ///     Validates if the player can smelt the specified recipe.
    /// </summary>
    (bool CanSmelt, string? ErrorMessage) CanSmelt(Player player, int requiredLevel);

    /// <summary>
    ///     Performs smelting of the specified recipe.
    /// </summary>
    SmeltingResult Smelt(
        Player player,
        SmeltingRecipe recipe,
        int count,
        Func<ItemId, IItem?> itemResolver);
}
