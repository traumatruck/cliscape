namespace CliScape.Core.Combat;

/// <summary>
///     Tracks experience gained and level-ups during combat.
/// </summary>
public class CombatRewards
{
    private readonly Dictionary<string, int> _experienceGained = new();
    private readonly List<LevelUp> _levelUps = [];

    /// <summary>
    ///     Gets the total experience gained per skill.
    /// </summary>
    public IReadOnlyDictionary<string, int> ExperienceGained => _experienceGained;

    /// <summary>
    ///     Gets the list of level-ups that occurred during combat.
    /// </summary>
    public IReadOnlyList<LevelUp> LevelUps => _levelUps;

    /// <summary>
    ///     Records experience gained for a skill.
    /// </summary>
    public void AddExperience(string skillName, int amount)
    {
        if (!_experienceGained.TryAdd(skillName, amount))
        {
            _experienceGained[skillName] += amount;
        }
    }

    /// <summary>
    ///     Records a level-up.
    /// </summary>
    public void AddLevelUp(string skillName, int newLevel)
    {
        _levelUps.Add(new LevelUp(skillName, newLevel));
    }

    /// <summary>
    ///     Gets the total experience gained across all skills.
    /// </summary>
    public int TotalExperience => _experienceGained.Values.Sum();
}

/// <summary>
///     Represents a level-up event.
/// </summary>
public record LevelUp(string SkillName, int NewLevel);
