namespace CliScape.Core.Events;

/// <summary>
///     Base record for domain events, providing common functionality.
/// </summary>
public abstract record DomainEventBase : IDomainEvent
{
    /// <inheritdoc />
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}