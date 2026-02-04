using CliScape.Core.World;

namespace CliScape.Core.Achievements;

/// <summary>
///     Represents an achievement diary for a specific location.
/// </summary>
public sealed class Diary
{
    public required LocationName Location { get; init; }
    public required IReadOnlyList<Achievement> Achievements { get; init; }

    /// <summary>
    ///     Gets all achievements for a specific tier.
    /// </summary>
    public IEnumerable<Achievement> GetAchievementsForTier(DiaryTier tier)
    {
        return Achievements.Where(a => a.Tier == tier);
    }

    /// <summary>
    ///     Gets the count of achievements in a specific tier.
    /// </summary>
    public int GetAchievementCountForTier(DiaryTier tier)
    {
        return Achievements.Count(a => a.Tier == tier);
    }
}