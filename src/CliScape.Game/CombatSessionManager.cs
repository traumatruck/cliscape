using CliScape.Core.Combat;
using CliScape.Core.Npcs;
using CliScape.Core.Players;

namespace CliScape.Game;

/// <summary>
///     Default implementation of <see cref="ICombatSessionManager" />.
/// </summary>
public sealed class CombatSessionManager : ICombatSessionManager
{
    /// <summary>
    ///     Singleton instance for simple access patterns.
    /// </summary>
    public static readonly CombatSessionManager Instance = new();

    /// <inheritdoc />
    public CombatSession? CurrentCombat { get; private set; }

    /// <inheritdoc />
    public bool IsInCombat => CurrentCombat is { IsComplete: false };

    /// <inheritdoc />
    public PendingLoot PendingLoot { get; } = new();

    /// <inheritdoc />
    public CombatSession StartCombat(Player player, ICombatableNpc npc)
    {
        CurrentCombat = new CombatSession(player, npc);
        return CurrentCombat;
    }

    /// <inheritdoc />
    public void EndCombat()
    {
        CurrentCombat = null;
    }
}