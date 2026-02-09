using CliScape.Core.Npcs;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when combat ends.
/// </summary>
public sealed record CombatEndedEvent(NpcName NpcName, CombatOutcome Outcome) : DomainEventBase;