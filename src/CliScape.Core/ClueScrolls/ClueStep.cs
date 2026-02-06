using CliScape.Core.World;

namespace CliScape.Core.ClueScrolls;

/// <summary>
///     Represents a single step in a clue scroll trail.
/// </summary>
public record ClueStep
{
    /// <summary>
    ///     The type of action the player must perform to complete this step.
    /// </summary>
    public required ClueStepType StepType { get; init; }

    /// <summary>
    ///     The hint text displayed to the player describing what to do.
    /// </summary>
    public required string HintText { get; init; }

    /// <summary>
    ///     The location the player must be at to complete this step.
    /// </summary>
    public required LocationName RequiredLocation { get; init; }

    /// <summary>
    ///     Flavour text displayed when the step is completed successfully.
    /// </summary>
    public required string CompletionText { get; init; }
}