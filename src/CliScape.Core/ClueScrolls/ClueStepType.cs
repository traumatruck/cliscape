namespace CliScape.Core.ClueScrolls;

/// <summary>
///     The type of action required to complete a clue step.
/// </summary>
public enum ClueStepType
{
    /// <summary>
    ///     Dig at a specific location to advance the clue.
    /// </summary>
    Dig,

    /// <summary>
    ///     Search an object or area at a specific location.
    /// </summary>
    Search,

    /// <summary>
    ///     Speak to an NPC at a specific location.
    /// </summary>
    Talk
}