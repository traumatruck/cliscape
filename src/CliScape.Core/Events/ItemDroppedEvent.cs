using CliScape.Core.Items;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when a player drops an item.
/// </summary>
public sealed record ItemDroppedEvent(ItemId ItemId, int Quantity) : DomainEventBase;