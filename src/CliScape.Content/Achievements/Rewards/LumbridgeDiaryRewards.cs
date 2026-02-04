using CliScape.Content.Items;
using CliScape.Content.Locations.Towns;
using CliScape.Core;
using CliScape.Core.Achievements;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;

namespace CliScape.Content.Achievements.Rewards;

/// <summary>
///     Defines rewards for completing Lumbridge diary tiers.
/// </summary>
public static class LumbridgeDiaryRewards
{
    public static LocationName Location => Lumbridge.Name;

    public static Dictionary<DiaryTier, DiaryReward[]> GetRewards()
    {
        return new Dictionary<DiaryTier, DiaryReward[]>
        {
            [DiaryTier.Easy] = new DiaryReward[]
            {
                new ItemReward
                {
                    Description = "2500 coins",
                    ItemId = ItemIds.Coins,
                    Quantity = 2500
                },
                new ExperienceLampReward
                {
                    Description = "500 XP lamp (any skill)",
                    ExperienceAmount = new Experience(500),
                    AllowedSkills = null // Any skill
                }
            },

            [DiaryTier.Medium] = new DiaryReward[]
            {
                new ItemReward
                {
                    Description = "5000 coins",
                    ItemId = ItemIds.Coins,
                    Quantity = 5000
                },
                new ExperienceLampReward
                {
                    Description = "1000 XP lamp (combat skills only)",
                    ExperienceAmount = new Experience(1000),
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