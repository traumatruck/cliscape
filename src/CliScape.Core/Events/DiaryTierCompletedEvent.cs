using CliScape.Core.Achievements;
using CliScape.Core.World;

namespace CliScape.Core.Events;

/// <summary>
///     Event raised when a player completes all achievements in a diary tier.
/// </summary>
public sealed class DiaryTierCompletedEvent : IDomainEvent
{
    public required LocationName Location { get; init; }
    public required DiaryTier Tier { get; init; }
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}