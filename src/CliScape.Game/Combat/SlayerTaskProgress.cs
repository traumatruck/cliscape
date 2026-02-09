namespace CliScape.Game.Combat;

/// <summary>
///     Progress on a slayer task.
/// </summary>
public record SlayerTaskProgress(int RemainingKills, bool TaskComplete, int SlayerXpGained);