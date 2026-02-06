namespace CliScape.Game.Persistence;

/// <summary>
///     Represents a snapshot of a single clue step for persistence.
/// </summary>
/// <param name="StepType">The type of action required (Dig, Search, Talk).</param>
/// <param name="HintText">The hint text displayed to the player.</param>
/// <param name="RequiredLocation">The location name the player must be at.</param>
/// <param name="CompletionText">The text displayed when the step is completed.</param>
public readonly record struct ClueStepSnapshot(
    string StepType,
    string HintText,
    string RequiredLocation,
    string CompletionText);
