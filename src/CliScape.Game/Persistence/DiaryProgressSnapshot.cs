namespace CliScape.Game.Persistence;

/// <summary>
///     Snapshot of diary progress for a specific location.
/// </summary>
public readonly record struct DiaryProgressSnapshot(
    string LocationName,
    string[] CompletedAchievementIds
);