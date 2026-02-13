using CliScape.Core;
using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Game.Skills;

/// <summary>
///     Handles firemaking logic.
/// </summary>
public interface IFiremakingService
{
    /// <summary>
    ///     Validates if the player can light the specified logs.
    /// </summary>
    ServiceResult CanLight(Player player, IItem logs);

    /// <summary>
    ///     Attempts to light logs using a tinderbox.
    /// </summary>
    FiremakingResult LightLogs(Player player, IItem logs);

    /// <summary>
    ///     Checks if an item is a type of log that can be burned.
    /// </summary>
    bool IsLog(IItem item);

    /// <summary>
    ///     Checks if an item is a tinderbox.
    /// </summary>
    bool IsTinderbox(IItem item);

    /// <summary>
    ///     Checks if two items can be used together for firemaking.
    /// </summary>
    bool CanUseForFiremaking(IItem item1, IItem item2, out IItem? tinderbox, out IItem? logs);
}