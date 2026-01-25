namespace CliScape.Core.Players.Skills;

/// <summary>
///     Represents a skill's level and experience points.
///     Uses Old School RuneScape skill formulas for experience calculations.
/// </summary>
public readonly record struct PlayerSkillLevel
{
    private const int MinLevel = 1;
    private const int MaxLevel = 99;
    private const int MaxExperience = 200_000_000;

    /// <summary>
    ///     Creates a new PlayerSkillLevel with the specified experience points.
    ///     The level is automatically calculated from the experience.
    /// </summary>
    /// <param name="experience">The total experience points (0 to 200,000,000)</param>
    private PlayerSkillLevel(int experience)
    {
        Experience = Math.Clamp(experience, 0, MaxExperience);
        Value = CalculateLevelFromExperience(Experience);
    }

    /// <summary>
    ///     Creates a new PlayerSkillLevel with the specified level and experience.
    /// </summary>
    /// <param name="value">The skill level (1 to 99)</param>
    /// <param name="experience">The total experience points</param>
    private PlayerSkillLevel(int value, int experience)
    {
        Value = value;
        Experience = experience;
    }

    public int Value { get; }

    public int Experience { get; }

    /// <summary>
    ///     Gets the experience required to reach the next level.
    ///     Returns 0 if already at max level.
    /// </summary>
    public int ExperienceToNextLevel
    {
        get
        {
            if (Value >= MaxLevel)
            {
                return 0;
            }

            var nextLevelExperience = GetExperienceForLevel(Value + 1);
            return nextLevelExperience - Experience;
        }
    }

    /// <summary>
    ///     Gets the experience gained in the current level.
    /// </summary>
    public int ExperienceInCurrentLevel
    {
        get
        {
            var currentLevelExperience = GetExperienceForLevel(Value);
            return Experience - currentLevelExperience;
        }
    }

    /// <summary>
    ///     Gets the total experience required for the current level.
    /// </summary>
    public int ExperienceForCurrentLevel => GetExperienceForLevel(Value);

    /// <summary>
    ///     Gets the total experience required for the next level.
    /// </summary>
    public int ExperienceForNextLevel => Value >= MaxLevel ? MaxExperience : GetExperienceForLevel(Value + 1);

    /// <summary>
    ///     Gets the percentage progress to the next level (0-100).
    /// </summary>
    public double ProgressToNextLevel
    {
        get
        {
            if (Value >= MaxLevel)
            {
                return 100.0;
            }

            var currentLevelXp = GetExperienceForLevel(Value);
            var nextLevelXp = GetExperienceForLevel(Value + 1);
            var xpInLevel = Experience - currentLevelXp;
            var xpNeededForLevel = nextLevelXp - currentLevelXp;

            return (double)xpInLevel / xpNeededForLevel * 100.0;
        }
    }

    /// <summary>
    ///     Creates a PlayerSkillLevel starting at level 1 with 0 experience.
    /// </summary>
    public static PlayerSkillLevel CreateNew()
    {
        return new PlayerSkillLevel(0);
    }

    /// <summary>
    ///     Creates a PlayerSkillLevel with a specific level (minimum experience for that level).
    /// </summary>
    /// <param name="level">The desired level (1 to 99)</param>
    public static PlayerSkillLevel FromLevel(int level)
    {
        level = Math.Clamp(level, MinLevel, MaxLevel);
        var experience = GetExperienceForLevel(level);
        return new PlayerSkillLevel(experience);
    }

    /// <summary>
    ///     Creates a PlayerSkillLevel from a specific experience amount.
    ///     The level is automatically calculated from the experience.
    /// </summary>
    /// <param name="experience">The total experience points (0 to 200,000,000)</param>
    public static PlayerSkillLevel FromExperience(int experience)
    {
        return new PlayerSkillLevel(experience);
    }

    /// <summary>
    ///     Adds experience points to this skill level and returns a new PlayerSkillLevel with the updated values.
    /// </summary>
    /// <param name="experienceGained">The amount of experience to add</param>
    /// <returns>A new PlayerSkillLevel with the added experience</returns>
    public PlayerSkillLevel AddExperience(int experienceGained)
    {
        var newExperience = Math.Min(Experience + experienceGained, MaxExperience);
        return new PlayerSkillLevel(newExperience);
    }

    /// <summary>
    ///     Calculates the level from a given experience amount using the OSRS formula.
    ///     Formula: Level = floor((1/4) * (1 + sqrt(1 + (8 * totalXP / 25))))
    ///     However, OSRS uses a table-based approach, so we iterate to find the correct level.
    /// </summary>
    private static int CalculateLevelFromExperience(int experience)
    {
        if (experience <= 0)
        {
            return 1;
        }

        if (experience >= GetExperienceForLevel(MaxLevel))
        {
            return MaxLevel;
        }

        // Binary search for the level
        var low = 1;
        var high = MaxLevel;

        while (low <= high)
        {
            var mid = (low + high) / 2;
            var xpForMid = GetExperienceForLevel(mid);
            var xpForNext = GetExperienceForLevel(mid + 1);

            if (experience >= xpForMid && experience < xpForNext)
            {
                return mid;
            }

            if (experience < xpForMid)
            {
                high = mid - 1;
            }
            else
            {
                low = mid + 1;
            }
        }

        return 1;
    }

    /// <summary>
    ///     Calculates the total experience required for a given level using the OSRS formula.
    ///     Formula: XP = sum from 1 to (level-1) of floor(L + 300 * 2^(L/7)) / 4
    /// </summary>
    private static int GetExperienceForLevel(int level)
    {
        if (level <= 1)
        {
            return 0;
        }

        if (level > MaxLevel)
        {
            level = MaxLevel;
        }

        var totalExperience = 0;
        
        for (var lvl = 1; lvl < level; lvl++)
        {
            totalExperience += (int)Math.Floor((lvl + 300.0 * Math.Pow(2.0, lvl / 7.0)) / 4.0);
        }

        return totalExperience;
    }

    public override string ToString()
    {
        return $"Level {Value} ({Experience:N0} XP)";
    }
}