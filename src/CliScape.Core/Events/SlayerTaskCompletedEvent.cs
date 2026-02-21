using CliScape.Core.Slayer;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when a slayer task is completed.
/// </summary>
public sealed record SlayerTaskCompletedEvent(SlayerCategory Category, int TotalKills, SlayerMasterName SlayerMaster) : DomainEventBase;