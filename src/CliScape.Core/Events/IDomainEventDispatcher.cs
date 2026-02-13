namespace CliScape.Core.Events;

/// <summary>
///     Dispatches domain events to registered handlers.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    ///     Raises a domain event and dispatches it to all registered handlers.
    /// </summary>
    void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent;

    /// <summary>
    ///     Registers a handler for a specific event type.
    /// </summary>
    void Register<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : IDomainEvent;

    /// <summary>
    ///     Registers an action as an event handler for a specific event type.
    ///     Returns a disposable that removes the handler when disposed.
    /// </summary>
    IDisposable Register<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent;
}