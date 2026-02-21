using CliScape.Core.World;

namespace CliScape.Core.Achievements;

/// <summary>
///     Provides diary reward lookups without coupling callers to the content layer.
/// </summary>
public interface IDiaryRewardRegistry
{
    /// <summary>
    ///     Gets the rewards for a specific location and diary tier.
    /// </summary>
    DiaryReward[] GetRewards(LocationName location, DiaryTier tier);
}
