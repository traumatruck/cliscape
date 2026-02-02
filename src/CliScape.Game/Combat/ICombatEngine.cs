using CliScape.Core.Combat;
using CliScape.Core.Events;
using CliScape.Core.Npcs;

namespace CliScape.Game.Combat;

/// <summary>
///     Result of a single attack action.
/// </summary>
public record AttackResult(bool Hit, int Damage, CombatExperience Experience);

/// <summary>
///     Result of a combat turn (player and NPC actions).
/// </summary>
public record CombatTurnResult(AttackResult PlayerAttack, AttackResult? NpcAttack, bool CombatEnded);

/// <summary>
///     Result of a flee attempt.
/// </summary>
public record FleeResult(bool Success);

/// <summary>
///     Result of completing a combat encounter.
/// </summary>
public record CombatOutcomeResult(
    CombatOutcome Outcome,
    IReadOnlyDictionary<string, int> ExperienceGained,
    IReadOnlyList<LevelUp> LevelUps,
    IReadOnlyList<DroppedItem> Drops,
    SlayerTaskProgress? SlayerProgress);

/// <summary>
///     Progress on a slayer task.
/// </summary>
public record SlayerTaskProgress(int RemainingKills, bool TaskComplete, int SlayerXpGained);

/// <summary>
///     Orchestrates combat between a player and an NPC.
/// </summary>
public interface ICombatEngine
{
    /// <summary>
    ///     Executes a player attack and NPC counter-attack.
    /// </summary>
    CombatTurnResult ExecuteTurn(CombatSession session, CombatStyle style);

    /// <summary>
    ///     Attempts to flee from combat.
    /// </summary>
    FleeResult AttemptFlee(CombatSession session);

    /// <summary>
    ///     Processes the outcome of combat when it ends.
    /// </summary>
    CombatOutcomeResult ProcessOutcome(CombatSession session);
}