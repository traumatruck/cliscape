namespace CliScape.Core.Events;

/// <summary>
///     Handles a specific type of domain event.
/// </summary>
/// <typeparam name="TEvent">The type of event to handle.</typeparam>
public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    /// <summary>
    ///     Handles the specified event.
    /// </summary>
    void Handle(TEvent @event);
}