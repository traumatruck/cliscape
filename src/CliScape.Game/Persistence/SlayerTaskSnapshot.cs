namespace CliScape.Game.Persistence;

/// <summary>
///     Represents a snapshot of a slayer task for persistence.
/// </summary>
/// <param name="Category">The slayer category of NPCs to kill.</param>
/// <param name="RemainingKills">The number of NPCs remaining to kill.</param>
/// <param name="TotalKills">The original number of NPCs assigned.</param>
/// <param name="SlayerMaster">The name of the slayer master who assigned this task.</param>
public readonly record struct SlayerTaskSnapshot(
    string Category,
    int RemainingKills,
    int TotalKills,
    string SlayerMaster);
