using CliScape.Core.Combat;

namespace CliScape.Game.Combat;

/// <summary>
///     Result of a single attack action.
/// </summary>
public record AttackResult(bool Hit, int Damage, CombatExperience Experience);