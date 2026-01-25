using CliScape.Core.Players.Skills;
using CliScape.Core.World;

namespace CliScape.Core.Players;

public sealed class Player
{
    public required int Id { get; init; }

    public required string Name { get; set; }

    public required ILocation CurrentLocation { get; set; }

    public int CurrentHealth => Health.CurrentHealth;

    public int MaximumHealth => Health.MaximumHealth;

    public int CombatLevel => Skills.CombatLevel;

    public int TotalLevel => Skills.TotalLevel;

    public PlayerHealth Health { private get; init; } = new();

    public PlayerSkillCollection Skills { get; init; } = new();

    public void Move(ILocation location)
    {
        CurrentLocation = location;
    }

    public PlayerSkillLevel GetSkillLevel(SkillName skillName)
    {
        return Skills.All.FirstOrDefault(skill => skill.Name == skillName)?.Level ??
               throw new InvalidOperationException($"Skill {skillName} not found");
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