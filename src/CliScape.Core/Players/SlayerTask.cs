using CliScape.Core.Slayer;

namespace CliScape.Core.Players;

/// <summary>
///     Represents an active slayer task assigned to the player.
/// </summary>
public record SlayerTask
{
    /// <summary>
    ///     The slayer category of NPCs to kill (e.g., "Goblins", "Cows").
    /// </summary>
    public required SlayerCategory Category { get; init; }

    /// <summary>
    ///     The number of NPCs remaining to kill.
    /// </summary>
    public required int RemainingKills { get; init; }

    /// <summary>
    ///     The original number of NPCs assigned.
    /// </summary>
    public required int TotalKills { get; init; }

    /// <summary>
    ///     The name of the slayer master who assigned this task.
    /// </summary>
    public required SlayerMasterName SlayerMaster { get; init; }

    /// <summary>
    ///     Whether the task is complete.
    /// </summary>
    public bool IsComplete => RemainingKills <= 0;

    /// <summary>
    ///     Returns a new SlayerTask with one kill completed.
    /// </summary>
    public SlayerTask CompleteKill()
    {
        return this with { RemainingKills = Math.Max(0, RemainingKills - 1) };
    }
}