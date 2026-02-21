using CliScape.Core.Events;

namespace CliScape.Content.Tests.Helpers;

/// <summary>
///     Stub event dispatcher that records all raised events for assertion.
/// </summary>
public sealed class StubEventDispatcher : IDomainEventDispatcher
{
    private readonly List<object> _raisedEvents = new();

    public IReadOnlyList<object> RaisedEvents => _raisedEvents;

    public void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        _raisedEvents.Add(@event);
    }

    public void Register<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : IDomainEvent
    {
        // No-op for testing — we just record events
    }

    public IDisposable Register<TEvent>(Action<TEvent> handler) where TEvent : IDomainEvent
    {
        // No-op for testing
        return new NoOpDisposable();
    }

    /// <summary>
    ///     Gets all raised events of a specific type.
    /// </summary>
    public IEnumerable<TEvent> GetEvents<TEvent>() where TEvent : IDomainEvent
    {
        return _raisedEvents.OfType<TEvent>();
    }

    /// <summary>
    ///     Asserts that at least one event of the specified type was raised.
    /// </summary>
    public TEvent AssertRaised<TEvent>() where TEvent : IDomainEvent
    {
        var events = GetEvents<TEvent>().ToList();
        Assert.NotEmpty(events);
        return events[0];
    }

    /// <summary>
    ///     Asserts no events of the given type were raised.
    /// </summary>
    public void AssertNotRaised<TEvent>() where TEvent : IDomainEvent
    {
        Assert.Empty(GetEvents<TEvent>());
    }

    private sealed class NoOpDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
