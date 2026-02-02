using CliScape.Core.Npcs;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when combat starts with an NPC.
/// </summary>
public sealed record CombatStartedEvent(NpcName NpcName, int NpcCombatLevel) : DomainEventBase;