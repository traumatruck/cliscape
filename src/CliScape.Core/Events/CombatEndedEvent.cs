using CliScape.Core.Npcs;

namespace CliScape.Core.Events;

/// <summary>
///     The outcome of a combat encounter.
/// </summary>
public enum CombatOutcome
{
    PlayerVictory,
    PlayerDeath,
    PlayerFled
}

/// <summary>
///     Raised when combat ends.
/// </summary>
public sealed record CombatEndedEvent(NpcName NpcName, CombatOutcome Outcome) : DomainEventBase;