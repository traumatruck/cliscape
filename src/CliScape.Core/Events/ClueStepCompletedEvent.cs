using CliScape.Core.ClueScrolls;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when a clue scroll step is completed successfully.
/// </summary>
public sealed record ClueStepCompletedEvent(
    ClueScrollTier Tier,
    int StepNumber,
    int TotalSteps) : DomainEventBase;