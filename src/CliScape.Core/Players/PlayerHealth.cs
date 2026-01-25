using CliScape.Core.Players.Skills;

namespace CliScape.Core.Players;

public sealed record PlayerHealth
{
    public int CurrentHealth { get; set; } = SkillConstants.StartingHitpoints;

    public int MaximumHealth { get; set; } = SkillConstants.StartingHitpoints;
}