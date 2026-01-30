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

    public int CurrentHealth => Health.CurrentHealth;

    public int MaximumHealth => Health.MaximumHealth;

    public int CombatLevel => SkillCollection.CombatLevel;

    public int TotalLevel => SkillCollection.TotalLevel;

    public IPlayerSkill[] Skills => SkillCollection.All;

    public void Move(ILocation location)
    {
        CurrentLocation = location;
    }

    /// <summary>
    ///     Adds experience to this skill.
    /// </summary>
    /// <param name="playerSkill">The player skill to be modified.</param>
    /// <param name="experienceGained">The amount of experience to add.</param>
    public static void AddExperience(IPlayerSkill playerSkill, int experienceGained)
    {
        playerSkill.Level.AddExperience(experienceGained);
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