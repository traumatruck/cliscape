using CliScape.Core.World;

namespace CliScape.Core.Achievements;

/// <summary>
///     Tracks completed achievements for a specific location.
/// </summary>
public sealed class DiaryProgress
{
    private readonly HashSet<AchievementId> _completedAchievements;

    public DiaryProgress(LocationName location)
    {
        Location = location;
        _completedAchievements = new HashSet<AchievementId>();
    }

    public DiaryProgress(LocationName location, IEnumerable<AchievementId> completedAchievements)
    {
        Location = location;
        _completedAchievements = new HashSet<AchievementId>(completedAchievements);
    }

    public LocationName Location { get; }

    public IReadOnlySet<AchievementId> CompletedAchievements => _completedAchievements;

    /// <summary>
    ///     Marks an achievement as completed.
    /// </summary>
    /// <returns>True if the achievement was newly completed, false if already completed.</returns>
    public bool CompleteAchievement(AchievementId achievementId)
    {
        return _completedAchievements.Add(achievementId);
    }

    /// <summary>
    ///     Checks if an achievement is completed.
    /// </summary>
    public bool IsCompleted(AchievementId achievementId)
    {
        return _completedAchievements.Contains(achievementId);
    }

    /// <summary>
    ///     Gets the count of completed achievements for a specific tier.
    /// </summary>
    public int GetCompletedCountForTier(Diary diary, DiaryTier tier)
    {
        var tierAchievements = diary.GetAchievementsForTier(tier);
        return tierAchievements.Count(a => IsCompleted(a.Id));
    }

    /// <summary>
    ///     Checks if a tier is fully completed.
    /// </summary>
    public bool IsTierComplete(Diary diary, DiaryTier tier)
    {
        var tierAchievements = diary.GetAchievementsForTier(tier).ToList();
        if (tierAchievements.Count == 0)
        {
            return false; // Cannot complete a tier with no achievements
        }

        return tierAchievements.All(a => IsCompleted(a.Id));
    }
}