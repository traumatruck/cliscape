using CliScape.Core.Achievements;
using CliScape.Core.World;

namespace CliScape.Core.Events;

/// <summary>
///     Event raised when a player completes an achievement.
/// </summary>
public sealed class AchievementCompletedEvent : IDomainEvent
{
    public required LocationName Location { get; init; }
    public required AchievementId AchievementId { get; init; }
    public required string AchievementName { get; init; }
    public required DiaryTier Tier { get; init; }
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}