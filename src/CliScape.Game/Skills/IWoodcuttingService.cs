using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Game.Skills;

/// <summary>
///     Handles woodcutting logic.
/// </summary>
public interface IWoodcuttingService
{
    /// <summary>
    ///     Validates if the player can chop the specified tree.
    /// </summary>
    (bool CanChop, string? ErrorMessage) CanChop(Player player, int requiredLevel);

    /// <summary>
    ///     Performs woodcutting at the specified tree.
    /// </summary>
    WoodcuttingResult Chop(Player player, ItemId logItemId, int experience, int count,
        Func<ItemId, IItem?> itemResolver);
}
