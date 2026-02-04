using CliScape.Content.Locations.Towns;
using CliScape.Core.Achievements;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Content.Achievements.Definitions;

/// <summary>
///     Defines all achievements for the Lumbridge achievement diary.
/// </summary>
public static class LumbridgeDiaryDefinition
{
    public static Diary Diary { get; } = new()
    {
        Location = Lumbridge.Name,
        Achievements = new Achievement[]
        {
            // Easy Tier
            new()
            {
                Id = new AchievementId("lumbridge_easy_kill_chicken"),
                Name = "Chicken Dinner",
                Description = "Kill a chicken in Lumbridge",
                Tier = DiaryTier.Easy,
                CompletionCheck = p =>
                {
                    // Simple check - if player has any combat XP, they've likely killed something
                    // More sophisticated tracking would need kill count tracking
                    return p.GetSkillLevel(SkillConstants.AttackSkillName).Experience >= 10;
                }
            },
            new()
            {
                Id = new AchievementId("lumbridge_easy_kill_cow"),
                Name = "Cow Tipper",
                Description = "Kill a cow in Lumbridge",
                Tier = DiaryTier.Easy,
                CompletionCheck = p => { return p.GetSkillLevel(SkillConstants.AttackSkillName).Experience >= 25; }
            },
            new()
            {
                Id = new AchievementId("lumbridge_easy_fish_shrimps"),
                Name = "Gone Fishing",
                Description = "Catch 10 shrimps",
                Tier = DiaryTier.Easy,
                CompletionCheck = p => { return p.GetSkillLevel(SkillConstants.FishingSkillName).Experience >= 40; }
            },
            new()
            {
                Id = new AchievementId("lumbridge_easy_chop_logs"),
                Name = "Tree Hugger",
                Description = "Chop 10 normal logs",
                Tier = DiaryTier.Easy,
                CompletionCheck = p =>
                {
                    return p.GetSkillLevel(SkillConstants.WoodcuttingSkillName).Experience >= 250;
                }
            },
            new()
            {
                Id = new AchievementId("lumbridge_easy_cook_shrimps"),
                Name = "Hot Stuff",
                Description = "Cook 10 shrimps successfully",
                Tier = DiaryTier.Easy,
                CompletionCheck = p => { return p.GetSkillLevel(SkillConstants.CookingSkillName).Experience >= 300; }
            },

            // Medium Tier
            new()
            {
                Id = new AchievementId("lumbridge_medium_combat_level_20"),
                Name = "Lumbridge Champion",
                Description = "Reach combat level 20",
                Tier = DiaryTier.Medium,
                CompletionCheck = p => p.CombatLevel >= 20
            },
            new()
            {
                Id = new AchievementId("lumbridge_medium_equip_iron_armor"),
                Name = "Well Equipped",
                Description = "Equip a full set of iron armor",
                Tier = DiaryTier.Medium,
                CompletionCheck = p =>
                {
                    // Check for iron helm, platebody, platelegs equipped
                    var equipped = p.Equipment.GetAllEquipped().ToList();
                    var hasIronHelm = equipped.Any(i =>
                        i.Name.Value.Contains("iron", StringComparison.OrdinalIgnoreCase) &&
                        i.Name.Value.Contains("helm", StringComparison.OrdinalIgnoreCase));
                    var hasIronBody = equipped.Any(i =>
                        i.Name.Value.Contains("iron", StringComparison.OrdinalIgnoreCase) &&
                        (i.Name.Value.Contains("platebody", StringComparison.OrdinalIgnoreCase) ||
                         i.Name.Value.Contains("chainbody", StringComparison.OrdinalIgnoreCase)));
                    var hasIronLegs = equipped.Any(i =>
                        i.Name.Value.Contains("iron", StringComparison.OrdinalIgnoreCase) &&
                        i.Name.Value.Contains("platelegs", StringComparison.OrdinalIgnoreCase));
                    return hasIronHelm && hasIronBody && hasIronLegs;
                }
            },
            new()
            {
                Id = new AchievementId("lumbridge_medium_smith_bronze_sword"),
                Name = "Bronze Smith",
                Description = "Smith a bronze sword",
                Tier = DiaryTier.Medium,
                CompletionCheck = p => { return p.GetSkillLevel(SkillConstants.SmithingSkillName).Experience >= 50; }
            }
        }
    };
}