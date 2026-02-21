using CliScape.Core.Achievements;
using CliScape.Core.Events;
using CliScape.Core.Players;
using CliScape.Core.World;

namespace CliScape.Game.Achievements;

/// <summary>
///     Service for managing achievement diary progress.
/// </summary>
public sealed class DiaryService
{
    private static readonly Lazy<DiaryService> _instance = new(() =>
        new DiaryService(DomainEventDispatcher.Instance, Content.Achievements.DiaryRegistry.Instance));

    private readonly DomainEventDispatcher _eventDispatcher;
    private readonly IDiaryRegistry _diaryRegistry;

    public DiaryService(DomainEventDispatcher eventDispatcher, IDiaryRegistry diaryRegistry)
    {
        _eventDispatcher = eventDispatcher;
        _diaryRegistry = diaryRegistry;
    }

    public static DiaryService Instance => _instance.Value;

    /// <summary>
    ///     Checks and completes a specific achievement if the completion check passes.
    /// </summary>
    /// <returns>True if the achievement was newly completed, false if already completed or check failed.</returns>
    public bool CheckAndCompleteAchievement(Player player, LocationName location, AchievementId achievementId)
    {
        var diary = _diaryRegistry.GetDiary(location);
        if (diary == null)
        {
            return false;
        }

        var achievement = diary.Achievements.FirstOrDefault(a => a.Id.Value == achievementId.Value);
        if (achievement == null)
        {
            return false;
        }

        var progress = player.DiaryProgress.GetProgress(location);

        // Already completed
        if (progress.IsCompleted(achievementId))
        {
            return false;
        }

        // Check if completion condition is met
        if (!achievement.CompletionCheck(player))
        {
            return false;
        }

        // Mark as completed
        progress.CompleteAchievement(achievementId);

        // Fire achievement completed event
        _eventDispatcher.Raise(new AchievementCompletedEvent(
            location,
            achievementId,
            achievement.Name,
            achievement.Tier));

        // Check if tier is now complete
        if (progress.IsTierComplete(diary, achievement.Tier))
        {
            _eventDispatcher.Raise(new DiaryTierCompletedEvent(
                location,
                achievement.Tier));
        }

        return true;
    }

    /// <summary>
    ///     Checks all achievements for a player and completes any that meet their criteria.
    /// </summary>
    /// <returns>The number of achievements newly completed.</returns>
    public int CheckAllAchievements(Player player)
    {
        var completedCount = 0;

        foreach (var diary in _diaryRegistry.GetAllDiaries())
        {
            foreach (var achievement in diary.Achievements)
            {
                if (CheckAndCompleteAchievement(player, diary.Location, achievement.Id))
                {
                    completedCount++;
                }
            }
        }

        return completedCount;
    }

    /// <summary>
    ///     Gets the completion percentage for a specific diary tier.
    /// </summary>
    public double GetTierCompletionPercentage(Player player, LocationName location, DiaryTier tier)
    {
        var diary = _diaryRegistry.GetDiary(location);
        if (diary == null)
        {
            return 0;
        }

        var progress = player.DiaryProgress.GetProgress(location);
        var totalCount = diary.GetAchievementCountForTier(tier);

        if (totalCount == 0)
        {
            return 0;
        }

        var completedCount = progress.GetCompletedCountForTier(diary, tier);
        return (double)completedCount / totalCount * 100;
    }

    /// <summary>
    ///     Gets the overall completion percentage for a diary.
    /// </summary>
    public double GetOverallCompletionPercentage(Player player, LocationName location)
    {
        var diary = _diaryRegistry.GetDiary(location);
        if (diary == null)
        {
            return 0;
        }

        var progress = player.DiaryProgress.GetProgress(location);
        var totalCount = diary.Achievements.Count;

        if (totalCount == 0)
        {
            return 0;
        }

        var completedCount = diary.Achievements.Count(a => progress.IsCompleted(a.Id));
        return (double)completedCount / totalCount * 100;
    }
}