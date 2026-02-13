using CliScape.Core.Achievements;
using CliScape.Core.World;

namespace CliScape.Core.Events;

/// <summary>
///     Event raised when a player completes all achievements in a diary tier.
/// </summary>
public sealed record DiaryTierCompletedEvent(
    LocationName Location,
    DiaryTier Tier) : DomainEventBase;