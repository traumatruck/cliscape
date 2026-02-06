using CliScape.Core.Achievements;
using CliScape.Core.ClueScrolls;
using CliScape.Core.Items;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;

namespace CliScape.Core.Players;

public sealed class Player
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required ILocation CurrentLocation { get; set; }

    public PlayerHealth Health { private get; init; } = new();

    public PlayerSkillCollection SkillCollection { private get; init; } = new();

    public Inventory Inventory { get; init; } = new();

    public Equipment Equipment { get; init; } = new();

    public Bank Bank { get; init; } = new();

    /// <summary>
    ///     The player's current slayer task, if any.
    /// </summary>
    public SlayerTask? SlayerTask { get; set; }

    /// <summary>
    ///     Tracks the player's achievement diary progress across all locations.
    /// </summary>
    public DiaryProgressCollection DiaryProgress { get; init; } = new();

    /// <summary>
    ///     Tracks which diary tier rewards have been claimed (format: "location_tier").
    /// </summary>
    public HashSet<string> ClaimedDiaryRewards { get; init; } = new();

    /// <summary>
    ///     The player's active clue scroll, if any. Only one clue can be active at a time.
    /// </summary>
    public ClueScroll? ActiveClue { get; set; }

    public int CurrentHealth => Health.CurrentHealth;

    public int MaximumHealth => Health.MaximumHealth;

    public int CurrentPrayerPoints { get; private set; }

    public int MaximumPrayerPoints => this.GetSkillLevel(SkillConstants.PrayerSkillName).Value;

    public int CombatLevel => SkillCollection.CombatLevel;

    public int TotalLevel => SkillCollection.TotalLevel;

    public IPlayerSkill[] Skills => SkillCollection.All;

    public void Move(ILocation location)
    {
        CurrentLocation = location;
    }

    public void SetPrayerPoints(int points)
    {
        CurrentPrayerPoints = Math.Clamp(points, 0, MaximumPrayerPoints);
    }

    /// <summary>
    ///     Adds experience to this skill.
    /// </summary>
    /// <param name="playerSkill">The player skill to be modified.</param>
    /// <param name="experienceGained">The amount of experience to add.</param>
    public static void AddExperience(IPlayerSkill playerSkill, int experienceGained)
    {
        playerSkill.Level = playerSkill.Level.AddExperience(experienceGained);
    }

    public void TakeDamage(int damage)
    {
        if (damage > Health.CurrentHealth)
        {
            Health.CurrentHealth = 0;
        }

        Health.CurrentHealth -= damage;
    }

    public void Heal(int healAmount)
    {
        Health.CurrentHealth += healAmount;

        if (Health.CurrentHealth > Health.MaximumHealth)
        {
            Health.CurrentHealth = Health.MaximumHealth;
        }
    }
}