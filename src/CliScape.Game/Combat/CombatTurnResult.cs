namespace CliScape.Game.Combat;

/// <summary>
///     Result of a combat turn (player and NPC actions).
/// </summary>
public record CombatTurnResult(AttackResult PlayerAttack, AttackResult? NpcAttack, bool CombatEnded);
