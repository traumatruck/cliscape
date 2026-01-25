namespace CliScape.Core.Players.Skills;

public static class SkillConstants
{
    /// <summary>
    ///     The initial HP level a new player starts the game with.
    /// </summary>
    public const int StartingHitpoints = 10;

    // Combat Skills
    public static readonly SkillName AttackSkillName = new("Attack");
    public static readonly SkillName StrengthSkillName = new("Strength");
    public static readonly SkillName DefenceSkillName = new("Defence");
    public static readonly SkillName HitpointsSkillName = new("Hitpoints");
    public static readonly SkillName RangedSkillName = new("Ranged");
    public static readonly SkillName PrayerSkillName = new("Prayer");
    public static readonly SkillName MagicSkillName = new("Magic");

    // Gathering Skills
    public static readonly SkillName MiningSkillName = new("Mining");
    public static readonly SkillName FishingSkillName = new("Fishing");
    public static readonly SkillName WoodcuttingSkillName = new("Woodcutting");
    public static readonly SkillName FarmingSkillName = new("Farming");
    public static readonly SkillName HunterSkillName = new("Hunter");

    // Artisan Skills
    public static readonly SkillName SmithingSkillName = new("Smithing");
    public static readonly SkillName CraftingSkillName = new("Crafting");
    public static readonly SkillName FletchingSkillName = new("Fletching");
    public static readonly SkillName CookingSkillName = new("Cooking");
    public static readonly SkillName FiremakingSkillName = new("Firemaking");
    public static readonly SkillName HerbloreSkillName = new("Herblore");
    public static readonly SkillName ConstructionSkillName = new("Construction");

    // Support Skills
    public static readonly SkillName AgilitySkillName = new("Agility");
    public static readonly SkillName ThievingSkillName = new("Thieving");
    public static readonly SkillName SlayerSkillName = new("Slayer");
    public static readonly SkillName RunecraftSkillName = new("Runecraft");
}