namespace CliScape.Core.ClueScrolls;

/// <summary>
///     The difficulty tier of a clue scroll, affecting step count and reward quality.
/// </summary>
public enum ClueScrollTier
{
    /// <summary>
    ///     Easy clue scroll — 2-3 steps, basic rewards.
    /// </summary>
    Easy,

    /// <summary>
    ///     Medium clue scroll — 3-4 steps, moderate rewards.
    /// </summary>
    Medium,

    /// <summary>
    ///     Hard clue scroll — 4-5 steps, valuable rewards.
    /// </summary>
    Hard,

    /// <summary>
    ///     Elite clue scroll — 5-6 steps, the best rewards.
    /// </summary>
    Elite
}