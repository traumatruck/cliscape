namespace CliScape.Core.Events;

/// <summary>
///     Default implementation of <see cref="IDomainEventDispatcher" />.
///     Maintains a registry of handlers and dispatches events synchronously.
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    /// <summary>
    ///     Singleton instance for simple access. For DI scenarios, register as a service instead.
    /// </summary>
    public static readonly DomainEventDispatcher Instance = new();

    private readonly Dictionary<Type, List<object>> _handlers = new();

    /// <inheritdoc />
    public void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlers.TryGetValue(eventType, out var handlers))
        {
            return;
        }

        foreach (var handler in handlers)
        {
            switch (handler)
            {
                case IDomainEventHandler<TEvent> typedHandler:
                    typedHandler.Handle(@event);
                    break;
                case Action<TEvent> action:
                    action(@event);
                    break;
            }
        }
    }

    /// <inheritdoc />
    public void Register<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlers.TryGetValue(eventType, out var handlers))
        {
            handlers = [];
            _handlers[eventType] = handlers;
        }

        handlers.Add(handler);
    }

    /// <inheritdoc />
    public void Register<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlers.TryGetValue(eventType, out var handlers))
        {
            handlers = [];
            _handlers[eventType] = handlers;
        }

        handlers.Add(handler);
    }

    /// <summary>
    ///     Clears all registered handlers. Primarily useful for testing.
    /// </summary>
    public void ClearHandlers()
    {
        _handlers.Clear();
    }
}