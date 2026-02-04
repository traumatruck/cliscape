using CliScape.Content.Achievements;
using CliScape.Content.Items;
using CliScape.Core.Achievements;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;

namespace CliScape.Game.Achievements;

/// <summary>
///     Service for managing diary rewards.
/// </summary>
public sealed class DiaryRewardService
{
    private static readonly Lazy<DiaryRewardService> _instance = new(() => new DiaryRewardService());

    public static DiaryRewardService Instance => _instance.Value;

    /// <summary>
    ///     Checks if a player can claim rewards for a specific tier.
    /// </summary>
    public bool CanClaimRewards(Player player, LocationName location, DiaryTier tier, out string? failureReason)
    {
        var diary = DiaryRegistry.Instance.GetDiary(location);
        if (diary == null)
        {
            failureReason = "Diary not found for this location.";
            return false;
        }

        var progress = player.DiaryProgress.GetProgress(location);

        // Check if tier is complete
        if (!progress.IsTierComplete(diary, tier))
        {
            failureReason = $"You have not completed all {tier} tier achievements yet.";
            return false;
        }

        // Check if already claimed
        var rewardKey = GetRewardKey(location, tier);
        if (player.ClaimedDiaryRewards.Contains(rewardKey))
        {
            failureReason = "You have already claimed rewards for this tier.";
            return false;
        }

        failureReason = null;
        return true;
    }

    /// <summary>
    ///     Claims rewards for a completed diary tier.
    /// </summary>
    /// <param name="lampSkill">The skill to apply XP lamp rewards to (if applicable).</param>
    /// <returns>True if rewards were successfully claimed.</returns>
    public bool ClaimRewards(Player player, LocationName location, DiaryTier tier, SkillName? lampSkill,
        out string? errorMessage)
    {
        if (!CanClaimRewards(player, location, tier, out var failureReason))
        {
            errorMessage = failureReason;
            return false;
        }

        var rewards = DiaryRewardRegistry.Instance.GetRewards(location, tier);
        if (rewards.Length == 0)
        {
            errorMessage = "No rewards defined for this tier.";
            return false;
        }

        // Process each reward
        foreach (var reward in rewards)
        {
            switch (reward)
            {
                case ItemReward itemReward:
                    var item = ItemRegistry.GetById(itemReward.ItemId);
                    if (item == null)
                    {
                        errorMessage = $"Item {itemReward.ItemId} not found.";
                        return false;
                    }

                    if (!player.Inventory.TryAdd(item, itemReward.Quantity))
                    {
                        errorMessage = "Your inventory is full. Please make space and try again.";
                        return false;
                    }

                    break;

                case ExperienceLampReward lampReward:
                    if (lampSkill == null)
                    {
                        errorMessage = "You must specify a skill for the experience lamp.";
                        return false;
                    }

                    if (!lampReward.IsSkillAllowed(lampSkill))
                    {
                        errorMessage = $"The experience lamp cannot be used on {lampSkill.Name}.";
                        return false;
                    }

                    var skill = player.GetSkill(lampSkill);
                    Player.AddExperience(skill, lampReward.ExperienceAmount.Value);
                    break;
            }
        }

        // Mark as claimed
        var rewardKey = GetRewardKey(location, tier);
        player.ClaimedDiaryRewards.Add(rewardKey);

        errorMessage = null;
        return true;
    }

    /// <summary>
    ///     Gets the rewards for a specific tier without claiming them.
    /// </summary>
    public DiaryReward[] GetRewards(LocationName location, DiaryTier tier)
    {
        return DiaryRewardRegistry.Instance.GetRewards(location, tier);
    }

    /// <summary>
    ///     Checks if any rewards require a skill selection (XP lamps).
    /// </summary>
    public bool RequiresSkillSelection(LocationName location, DiaryTier tier)
    {
        var rewards = GetRewards(location, tier);
        return rewards.OfType<ExperienceLampReward>().Any();
    }

    /// <summary>
    ///     Gets all valid skills for XP lamp rewards at this tier.
    /// </summary>
    public IEnumerable<SkillName> GetValidLampSkills(LocationName location, DiaryTier tier)
    {
        var rewards = GetRewards(location, tier);
        var lampRewards = rewards.OfType<ExperienceLampReward>().ToList();

        if (lampRewards.Count == 0)
        {
            return Enumerable.Empty<SkillName>();
        }

        // If any lamp allows all skills, return all skills
        if (lampRewards.Any(l => l.AllowedSkills == null))
        {
            // Return all combat + non-combat skills
            return new[]
            {
                SkillConstants.AttackSkillName,
                SkillConstants.StrengthSkillName,
                SkillConstants.DefenceSkillName,
                SkillConstants.HitpointsSkillName,
                SkillConstants.RangedSkillName,
                SkillConstants.PrayerSkillName,
                SkillConstants.MagicSkillName,
                SkillConstants.MiningSkillName,
                SkillConstants.FishingSkillName,
                SkillConstants.WoodcuttingSkillName,
                SkillConstants.SmithingSkillName,
                SkillConstants.CraftingSkillName,
                SkillConstants.FletchingSkillName,
                SkillConstants.CookingSkillName,
                SkillConstants.FiremakingSkillName,
                SkillConstants.HerbloreSkillName,
                SkillConstants.AgilitySkillName,
                SkillConstants.ThievingSkillName,
                SkillConstants.SlayerSkillName,
                SkillConstants.RunecraftSkillName,
                SkillConstants.FarmingSkillName,
                SkillConstants.HunterSkillName,
                SkillConstants.ConstructionSkillName
            };
        }

        // Otherwise, return the union of all allowed skills
        var allowedSkills = new HashSet<SkillName>();
        foreach (var lamp in lampRewards)
        {
            if (lamp.AllowedSkills != null)
            {
                foreach (var skill in lamp.AllowedSkills)
                {
                    allowedSkills.Add(skill);
                }
            }
        }

        return allowedSkills;
    }

    private static string GetRewardKey(LocationName location, DiaryTier tier)
    {
        return $"{location.Value}_{tier}";
    }
}