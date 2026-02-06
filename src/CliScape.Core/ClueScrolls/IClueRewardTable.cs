namespace CliScape.Core.ClueScrolls;

/// <summary>
///     Provides reward definitions for completed clue scrolls.
/// </summary>
public interface IClueRewardTable
{
    /// <summary>
    ///     Gets the possible rewards for the specified tier.
    /// </summary>
    IReadOnlyList<ClueReward> GetRewards(ClueScrollTier tier);

    /// <summary>
    ///     Gets how many reward rolls to make for the specified tier.
    /// </summary>
    int GetRewardRollCount(ClueScrollTier tier);
}