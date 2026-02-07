using CliScape.Content.Items;
using CliScape.Content.Locations.Towns;
using CliScape.Core;
using CliScape.Core.Achievements;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;

namespace CliScape.Content.Achievements.Rewards;

/// <summary>
///     Defines rewards for completing Varrock diary tiers.
/// </summary>
public static class VarrockDiaryRewards
{
    public static LocationName Location => Varrock.Name;

    public static Dictionary<DiaryTier, DiaryReward[]> GetRewards()
    {
        return new Dictionary<DiaryTier, DiaryReward[]>
        {
            [DiaryTier.Easy] = new DiaryReward[]
            {
                new ItemReward
                {
                    Description = "3000 coins",
                    ItemId = ItemIds.Coins,
                    Quantity = 3000
                },
                new ExperienceLampReward
                {
                    Description = "500 XP lamp (any skill)",
                    ExperienceAmount = new Experience(500),
                    AllowedSkills = null
                }
            },

            [DiaryTier.Medium] = new DiaryReward[]
            {
                new ItemReward
                {
                    Description = "7500 coins",
                    ItemId = ItemIds.Coins,
                    Quantity = 7500
                },
                new ExperienceLampReward
                {
                    Description = "1500 XP lamp (any skill)",
                    ExperienceAmount = new Experience(1500),
                    AllowedSkills = null
                }
            },

            [DiaryTier.Hard] = new DiaryReward[]
            {
                new ItemReward
                {
                    Description = "15000 coins",
                    ItemId = ItemIds.Coins,
                    Quantity = 15000
                },
                new ExperienceLampReward
                {
                    Description = "5000 XP lamp (combat skills only)",
                    ExperienceAmount = new Experience(5000),
                    AllowedSkills = new HashSet<SkillName>
                    {
                        SkillConstants.AttackSkillName,
                        SkillConstants.StrengthSkillName,
                        SkillConstants.DefenceSkillName,
                        SkillConstants.HitpointsSkillName,
                        SkillConstants.RangedSkillName,
                        SkillConstants.MagicSkillName
                    }
                }
            }
        };
    }
}
