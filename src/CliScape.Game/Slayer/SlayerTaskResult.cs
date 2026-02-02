using CliScape.Core.Players;

namespace CliScape.Game.Slayer;

/// <summary>
///     Result of attempting to get a slayer task.
/// </summary>
public abstract record SlayerTaskResult
{
    /// <summary>
    ///     A task was successfully assigned.
    /// </summary>
    public sealed record TaskAssigned(SlayerTask Task) : SlayerTaskResult;

    /// <summary>
    ///     The player already has an active task.
    /// </summary>
    public sealed record AlreadyHaveTask(SlayerTask CurrentTask) : SlayerTaskResult;

    /// <summary>
    ///     The player's combat level is too low for this master.
    /// </summary>
    public sealed record InsufficientCombatLevel(int Required) : SlayerTaskResult;

    /// <summary>
    ///     The player's slayer level is too low for this master.
    /// </summary>
    public sealed record InsufficientSlayerLevel(int Required) : SlayerTaskResult;

    /// <summary>
    ///     No valid assignments are available for the player.
    /// </summary>
    public sealed record NoValidAssignments : SlayerTaskResult;
}