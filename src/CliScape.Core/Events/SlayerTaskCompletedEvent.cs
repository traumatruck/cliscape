namespace CliScape.Core.Events;

/// <summary>
///     Raised when a slayer task is completed.
/// </summary>
public sealed record SlayerTaskCompletedEvent(string Category, int TotalKills, string SlayerMaster) : DomainEventBase;