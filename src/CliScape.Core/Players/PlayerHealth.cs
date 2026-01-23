using CliScape.Core.Players.Skills;

namespace CliScape.Core.Players;

public sealed record PlayerHealth(
    int CurrentHealth = SkillConstants.StarterHealth,
    int MaxHealth = SkillConstants.StarterHealth);