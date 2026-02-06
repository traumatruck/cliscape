namespace CliScape.Core.ClueScrolls;

/// <summary>
///     Provides clue step definitions for assembling clue scrolls.
/// </summary>
public interface IClueStepPool
{
    /// <summary>
    ///     Gets all available clue steps for the specified tier.
    /// </summary>
    IReadOnlyList<ClueStep> GetSteps(ClueScrollTier tier);
}