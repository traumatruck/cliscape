namespace CliScape.Core.ClueScrolls;

/// <summary>
///     Represents an active clue scroll with an ordered list of steps to complete.
/// </summary>
public record ClueScroll
{
    /// <summary>
    ///     The difficulty tier of this clue scroll.
    /// </summary>
    public required ClueScrollTier Tier { get; init; }

    /// <summary>
    ///     The ordered steps the player must complete.
    /// </summary>
    public required ClueStep[] Steps { get; init; }

    /// <summary>
    ///     The index of the current step (0-based).
    /// </summary>
    public required int CurrentStepIndex { get; init; }

    /// <summary>
    ///     The current step the player needs to complete.
    /// </summary>
    public ClueStep CurrentStep => Steps[CurrentStepIndex];

    /// <summary>
    ///     Whether all steps have been completed.
    /// </summary>
    public bool IsComplete => CurrentStepIndex >= Steps.Length;

    /// <summary>
    ///     The total number of steps in this clue scroll.
    /// </summary>
    public int TotalSteps => Steps.Length;

    /// <summary>
    ///     Returns a new ClueScroll advanced to the next step.
    /// </summary>
    public ClueScroll Advance()
    {
        return this with { CurrentStepIndex = CurrentStepIndex + 1 };
    }
}