namespace CliScape.Game.Persistence;

/// <summary>
///     Represents a snapshot of an active clue scroll for persistence.
/// </summary>
/// <param name="Tier">The difficulty tier (Easy, Medium, Hard, Elite).</param>
/// <param name="Steps">The ordered clue steps.</param>
/// <param name="CurrentStepIndex">The index of the current step (0-based).</param>
public readonly record struct ClueScrollSnapshot(
    string Tier,
    ClueStepSnapshot[] Steps,
    int CurrentStepIndex);