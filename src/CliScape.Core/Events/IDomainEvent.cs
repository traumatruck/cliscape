namespace CliScape.Core.Events;

/// <summary>
///     Marker interface for domain events.
///     Domain events represent something that happened in the domain that domain experts care about.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    ///     Gets the timestamp when this event occurred.
    /// </summary>
    DateTime OccurredAt { get; }
}