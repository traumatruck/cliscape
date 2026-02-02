using CliScape.Core.Items;

namespace CliScape.Core.Events;

/// <summary>
///     Raised when a player picks up an item.
/// </summary>
public sealed record ItemPickedUpEvent(ItemId ItemId, int Quantity) : DomainEventBase;