using CliScape.Core.ClueScrolls;

namespace CliScape.Game.ClueScrolls;

/// <summary>
///     Result of attempting to advance a clue scroll step.
/// </summary>
public abstract record ClueStepResult
{
    /// <summary>
    ///     The step was completed successfully and the clue advances.
    /// </summary>
    public sealed record StepCompleted(
        ClueStep CompletedStep,
        int StepNumber,
        int TotalSteps,
        ClueStep? NextStep) : ClueStepResult;

    /// <summary>
    ///     All steps completed — the clue scroll is finished and rewards are granted.
    /// </summary>
    public sealed record ClueCompleted(
        ClueScrollTier Tier,
        int TotalSteps) : ClueStepResult;

    /// <summary>
    ///     The player is not at the required location for this step.
    /// </summary>
    public sealed record WrongLocation(string RequiredLocation, string CurrentLocation) : ClueStepResult;

    /// <summary>
    ///     The player has no active clue scroll.
    /// </summary>
    public sealed record NoActiveClue : ClueStepResult;
}