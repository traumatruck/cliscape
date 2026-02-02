namespace CliScape.Core.Players.Skills;

/// <summary>
///     Contains all skills for a player, providing access by name and calculating derived stats.
/// </summary>
public class PlayerSkillCollection
{
    private readonly Dictionary<string, IPlayerSkill> _skillsByName;

    public PlayerSkillCollection()
    {
        // Create all skills using the consolidated PlayerSkill class
        All =
        [
            // Combat Skills
            new PlayerSkill(SkillConstants.AttackSkillName),
            new PlayerSkill(SkillConstants.DefenceSkillName),
            new PlayerSkill(SkillConstants.StrengthSkillName),
            new PlayerSkill(SkillConstants.HitpointsSkillName, SkillConstants.StartingHitpoints),
            new PlayerSkill(SkillConstants.RangedSkillName),
            new PlayerSkill(SkillConstants.PrayerSkillName),
            new PlayerSkill(SkillConstants.MagicSkillName),

            // Gathering Skills
            new PlayerSkill(SkillConstants.CookingSkillName),
            new PlayerSkill(SkillConstants.WoodcuttingSkillName),
            new PlayerSkill(SkillConstants.FletchingSkillName),
            new PlayerSkill(SkillConstants.FishingSkillName),
            new PlayerSkill(SkillConstants.FiremakingSkillName),
            new PlayerSkill(SkillConstants.CraftingSkillName),
            new PlayerSkill(SkillConstants.SmithingSkillName),
            new PlayerSkill(SkillConstants.MiningSkillName),
            new PlayerSkill(SkillConstants.HerbloreSkillName),
            new PlayerSkill(SkillConstants.AgilitySkillName),
            new PlayerSkill(SkillConstants.ThievingSkillName),
            new PlayerSkill(SkillConstants.SlayerSkillName),
            new PlayerSkill(SkillConstants.FarmingSkillName),
            new PlayerSkill(SkillConstants.RunecraftSkillName),
            new PlayerSkill(SkillConstants.HunterSkillName),
            new PlayerSkill(SkillConstants.ConstructionSkillName)
        ];

        // Build lookup dictionary for fast access
        _skillsByName = All.ToDictionary(s => s.Name.Name, s => s);
    }

    /// <summary>
    ///     Gets all player skills.
    /// </summary>
    public IPlayerSkill[] All { get; }

    /// <summary>
    ///     Calculates the player's combat level using OSRS formula.
    /// </summary>
    public int CombatLevel
    {
        get
        {
            var defence = GetLevel(SkillConstants.DefenceSkillName);
            var hitpoints = GetLevel(SkillConstants.HitpointsSkillName);
            var prayer = GetLevel(SkillConstants.PrayerSkillName);
            var attack = GetLevel(SkillConstants.AttackSkillName);
            var strength = GetLevel(SkillConstants.StrengthSkillName);
            var ranged = GetLevel(SkillConstants.RangedSkillName);
            var magic = GetLevel(SkillConstants.MagicSkillName);

            var baseLevel = 0.25 * (defence + hitpoints + Math.Floor(prayer / 2.0));
            var meleeLevel = 0.325 * (attack + strength);
            var rangedLevel = 0.325 * (Math.Floor(ranged / 2.0) + ranged);
            var magicLevel = 0.325 * (Math.Floor(magic / 2.0) + magic);
            var combatLevel = baseLevel + Math.Max(meleeLevel, Math.Max(rangedLevel, magicLevel));

            return (int)Math.Floor(combatLevel);
        }
    }

    /// <summary>
    ///     Calculates the player's total level (sum of all skill levels).
    /// </summary>
    public int TotalLevel => All.Sum(skill => skill.Level.Value);

    /// <summary>
    ///     Gets a skill by its name.
    /// </summary>
    /// <param name="name">The skill name.</param>
    /// <returns>The skill if found, null otherwise.</returns>
    public IPlayerSkill? GetByName(SkillName name)
    {
        return _skillsByName.GetValueOrDefault(name.Name);
    }

    /// <summary>
    ///     Gets a skill by its string name.
    /// </summary>
    /// <param name="name">The skill name as a string.</param>
    /// <returns>The skill if found, null otherwise.</returns>
    public IPlayerSkill? GetByName(string name)
    {
        return _skillsByName.GetValueOrDefault(name);
    }

    private int GetLevel(SkillName name)
    {
        return GetByName(name)?.Level.Value ?? 1;
    }
}