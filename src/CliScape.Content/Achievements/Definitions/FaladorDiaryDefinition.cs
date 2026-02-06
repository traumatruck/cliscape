using CliScape.Content.Locations.Towns;
using CliScape.Core.Achievements;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Content.Achievements.Definitions;

/// <summary>
///     Defines all achievements for the Falador achievement diary.
/// </summary>
public static class FaladorDiaryDefinition
{
    public static Diary Diary { get; } = new()
    {
        Location = Falador.Name,
        Achievements = new Achievement[]
        {
            // Easy Tier
            new()
            {
                Id = new AchievementId("falador_easy_mine_copper"),
                Name = "Mining Apprentice",
                Description = "Mine some copper or tin ore near Falador",
                Tier = DiaryTier.Easy,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.MiningSkillName).Experience >= 18
            },
            new()
            {
                Id = new AchievementId("falador_easy_smith_dagger"),
                Name = "Dagger Smith",
                Description = "Smith a bronze dagger on the Falador anvil",
                Tier = DiaryTier.Easy,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.SmithingSkillName).Experience >= 13
            },
            new()
            {
                Id = new AchievementId("falador_easy_kill_guard"),
                Name = "Troublemaker",
                Description = "Kill a guard in Falador",
                Tier = DiaryTier.Easy,
                CompletionCheck = p =>
                {
                    return p.GetSkillLevel(SkillConstants.AttackSkillName).Experience >= 100 ||
                           p.GetSkillLevel(SkillConstants.StrengthSkillName).Experience >= 100;
                }
            },

            // Medium Tier
            new()
            {
                Id = new AchievementId("falador_medium_mine_coal"),
                Name = "Coal Miner",
                Description = "Mine coal in Falador",
                Tier = DiaryTier.Medium,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.MiningSkillName).Experience >= 1000
            },
            new()
            {
                Id = new AchievementId("falador_medium_smith_iron"),
                Name = "Iron Smith",
                Description = "Smith an iron item at the Falador anvil",
                Tier = DiaryTier.Medium,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.SmithingSkillName).Experience >= 200
            },
            new()
            {
                Id = new AchievementId("falador_medium_thieve_guard"),
                Name = "Guard Robber",
                Description = "Pickpocket a guard in Falador",
                Tier = DiaryTier.Medium,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.ThievingSkillName).Experience >= 500
            },

            // Hard Tier
            new()
            {
                Id = new AchievementId("falador_hard_mine_mithril"),
                Name = "Mithril Miner",
                Description = "Mine mithril ore in Falador",
                Tier = DiaryTier.Hard,
                CompletionCheck = p => p.GetSkillLevel(SkillConstants.MiningSkillName).Experience >= 5000
            },
            new()
            {
                Id = new AchievementId("falador_hard_combat_level_40"),
                Name = "Falador Knight",
                Description = "Reach combat level 40",
                Tier = DiaryTier.Hard,
                CompletionCheck = p => p.CombatLevel >= 40
            }
        }
    };
}
