using CliScape.Core.Combat;
using CliScape.Core.Npcs;
using CliScape.Core.Players;

namespace CliScape.Game;

/// <summary>
///     Manages the current combat session.
/// </summary>
public interface ICombatSessionManager
{
    /// <summary>
    ///     The current active combat session, if any.
    /// </summary>
    CombatSession? CurrentCombat { get; }

    /// <summary>
    ///     Whether the player is currently in combat.
    /// </summary>
    bool IsInCombat { get; }

    /// <summary>
    ///     Loot available from the most recent combat.
    /// </summary>
    PendingLoot PendingLoot { get; }

    /// <summary>
    ///     Start a combat session with the specified NPC.
    /// </summary>
    /// <param name="player">The player entering combat.</param>
    /// <param name="npc">The NPC to fight.</param>
    /// <returns>The new combat session.</returns>
    CombatSession StartCombat(Player player, ICombatableNpc npc);

    /// <summary>
    ///     End the current combat session.
    /// </summary>
    void EndCombat();
}