using CliScape.Core.ClueScrolls;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when all steps of a clue scroll are completed and rewards are granted.
/// </summary>
public sealed record ClueScrollCompletedEvent(
    ClueScrollTier Tier,
    int TotalSteps) : DomainEventBase;