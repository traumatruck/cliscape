using CliScape.Core.Players;

namespace CliScape.Core.Achievements;

/// <summary>
///     Represents a single achievement within a diary.
/// </summary>
public sealed class Achievement
{
    public required AchievementId Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required DiaryTier Tier { get; init; }

    /// <summary>
    ///     Predicate function that determines if the achievement is complete.
    /// </summary>
    public required Func<Player, bool> CompletionCheck { get; init; }
}