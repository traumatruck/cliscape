using CliScape.Core.Achievements;
using CliScape.Core.World;

namespace CliScape.Core.Events;

/// <summary>
///     Event raised when a player completes an achievement.
/// </summary>
public sealed record AchievementCompletedEvent(
    LocationName Location,
    AchievementId AchievementId,
    string AchievementName,
    DiaryTier Tier) : DomainEventBase;