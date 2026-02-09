using CliScape.Content.Locations.Towns;
using CliScape.Core.Achievements;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Content.Achievements.Definitions;

/// <summary>
///     Defines all achievements for the Varrock achievement diary.
/// </summary>
public static class VarrockDiaryDefinition
{
    public static Diary Diary { get; } = new()
    {
        Location = Varrock.Name,
        Achievements = new Achievement[]
        {
            // Easy Tier
            new()
            {
                Id = new AchievementId("varrock_easy_mine_copper"),
                Name = "Copper Miner",
                Description = "Mine some copper ore in Varrock",
                Tier = DiaryTier.Easy,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.MiningSkillName).Experience >= 18
            },
            new()
            {
                Id = new AchievementId("varrock_easy_thieve_man"),
                Name = "Sticky Fingers",
                Description = "Pickpocket a man in Varrock",
                Tier = DiaryTier.Easy,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.ThievingSkillName).Experience >= 8
            },
            new()
            {
                Id = new AchievementId("varrock_easy_smith_bronze"),
                Name = "Bronze Worker",
                Description = "Smelt a bronze bar at the Varrock furnace",
                Tier = DiaryTier.Easy,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.SmithingSkillName).Experience >= 6
            },
            new()
            {
                Id = new AchievementId("varrock_easy_buy_sword"),
                Name = "Armed and Dangerous",
                Description = "Buy an iron sword from the Varrock Sword Shop",
                Tier = DiaryTier.Easy,
                CompletionCheck = p =>
                {
                    return p.GetSkillLevel(SkillConstants.AttackSkillName).Experience >= 50 ||
                           p.GetSkillLevel(SkillConstants.StrengthSkillName).Experience >= 50;
                }
            },

            // Medium Tier
            new()
            {
                Id = new AchievementId("varrock_medium_combat_level_30"),
                Name = "Varrock Champion",
                Description = "Reach combat level 30",
                Tier = DiaryTier.Medium,
                CompletionCheck = p => p.CombatLevel >= 30
            },
            new()
            {
                Id = new AchievementId("varrock_medium_mine_iron"),
                Name = "Iron Miner",
                Description = "Mine iron ore in Varrock",
                Tier = DiaryTier.Medium,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.MiningSkillName).Experience >= 300
            },
            new()
            {
                Id = new AchievementId("varrock_medium_thieve_silk"),
                Name = "Silk Snatcher",
                Description = "Steal from the silk stall in Varrock",
                Tier = DiaryTier.Medium,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.ThievingSkillName).Experience >= 200
            },

            // Hard Tier
            new()
            {
                Id = new AchievementId("varrock_hard_equip_steel"),
                Name = "Steel Warrior",
                Description = "Equip a full set of steel armor",
                Tier = DiaryTier.Hard,
                CompletionCheck = p =>
                {
                    var equipped = p.Equipment.GetAllEquipped().ToList();
                    var hasHelm = equipped.Any(i =>
                        i.Name.Value.Contains("steel", StringComparison.OrdinalIgnoreCase) &&
                        i.Name.Value.Contains("helm", StringComparison.OrdinalIgnoreCase));
                    var hasBody = equipped.Any(i =>
                        i.Name.Value.Contains("steel", StringComparison.OrdinalIgnoreCase) &&
                        (i.Name.Value.Contains("platebody", StringComparison.OrdinalIgnoreCase) ||
                         i.Name.Value.Contains("chainbody", StringComparison.OrdinalIgnoreCase)));
                    var hasLegs = equipped.Any(i =>
                        i.Name.Value.Contains("steel", StringComparison.OrdinalIgnoreCase) &&
                        i.Name.Value.Contains("platelegs", StringComparison.OrdinalIgnoreCase));
                    return hasHelm && hasBody && hasLegs;
                }
            },
            new()
            {
                Id = new AchievementId("varrock_hard_combat_level_50"),
                Name = "Seasoned Fighter",
                Description = "Reach combat level 50",
                Tier = DiaryTier.Hard,
                CompletionCheck = p => p.CombatLevel >= 50
            }
        }
    };
}