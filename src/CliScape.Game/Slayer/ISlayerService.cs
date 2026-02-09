using CliScape.Core.Players;
using CliScape.Core.Slayer;

namespace CliScape.Game.Slayer;

/// <summary>
///     Manages slayer task assignment and completion.
/// </summary>
public interface ISlayerService
{
    /// <summary>
    ///     Attempts to assign a new slayer task from the specified master.
    /// </summary>
    SlayerTaskResult AssignTask(Player player, SlayerMaster master);

    /// <summary>
    ///     Cancels the player's current slayer task.
    /// </summary>
    bool CancelTask(Player player);
}