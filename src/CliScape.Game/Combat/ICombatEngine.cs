using CliScape.Core.Combat;

namespace CliScape.Game.Combat;

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
