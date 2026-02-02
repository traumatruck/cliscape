namespace CliScape.Core.Combat;

/// <summary>
///     Combat styles that determine XP distribution.
/// </summary>
public enum CombatStyle
{
    /// <summary>
    ///     Attack XP is gained.
    /// </summary>
    Accurate,

    /// <summary>
    ///     Strength XP is gained.
    /// </summary>
    Aggressive,

    /// <summary>
    ///     Defence XP is gained.
    /// </summary>
    Defensive,

    /// <summary>
    ///     XP is shared between Attack, Strength, and Defence.
    /// </summary>
    Controlled
}