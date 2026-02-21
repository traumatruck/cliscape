using CliScape.Core.Events;
using CliScape.Core.Players.Skills;

namespace CliScape.Content.Tests;

public class DomainEventDispatcherTests
{
    [Fact]
    public void Raise_WithRegisteredActionHandler_InvokesHandler()
    {
        var dispatcher = new DomainEventDispatcher();
        LevelUpEvent? received = null;
        dispatcher.Register<LevelUpEvent>(e => received = e);

        var evt = new LevelUpEvent(new SkillName("Attack"), 5);
        dispatcher.Raise(evt);

        Assert.NotNull(received);
        Assert.Equal(5, received.NewLevel);
        Assert.Equal("Attack", received.SkillName.Name);
    }

    [Fact]
    public void Raise_NoHandlers_DoesNotThrow()
    {
        var dispatcher = new DomainEventDispatcher();

        var evt = new LevelUpEvent(new SkillName("Attack"), 5);

        // Should not throw
        dispatcher.Raise(evt);
    }

    [Fact]
    public void Raise_MultipleHandlers_InvokesAll()
    {
        var dispatcher = new DomainEventDispatcher();
        var count = 0;
        dispatcher.Register<LevelUpEvent>(_ => count++);
        dispatcher.Register<LevelUpEvent>(_ => count++);

        dispatcher.Raise(new LevelUpEvent(new SkillName("Attack"), 5));

        Assert.Equal(2, count);
    }

    [Fact]
    public void Register_ReturnsDisposable_ThatUnregistersHandler()
    {
        var dispatcher = new DomainEventDispatcher();
        var count = 0;
        var registration = dispatcher.Register<LevelUpEvent>(_ => count++);

        dispatcher.Raise(new LevelUpEvent(new SkillName("Attack"), 5));
        Assert.Equal(1, count);

        registration.Dispose();

        dispatcher.Raise(new LevelUpEvent(new SkillName("Attack"), 6));
        Assert.Equal(1, count); // Should not have been called again
    }

    [Fact]
    public void ClearHandlers_RemovesAllHandlers()
    {
        var dispatcher = new DomainEventDispatcher();
        var count = 0;
        dispatcher.Register<LevelUpEvent>(_ => count++);

        dispatcher.Raise(new LevelUpEvent(new SkillName("Attack"), 5));
        Assert.Equal(1, count);

        dispatcher.ClearHandlers();

        dispatcher.Raise(new LevelUpEvent(new SkillName("Attack"), 6));
        Assert.Equal(1, count); // Should not have been called
    }

    [Fact]
    public void Raise_HandlerThrows_DoesNotStopOtherHandlers()
    {
        var dispatcher = new DomainEventDispatcher();
        var secondHandlerCalled = false;

        dispatcher.Register<LevelUpEvent>(_ => throw new InvalidOperationException("Boom"));
        dispatcher.Register<LevelUpEvent>(_ => secondHandlerCalled = true);

        dispatcher.Raise(new LevelUpEvent(new SkillName("Attack"), 5));

        Assert.True(secondHandlerCalled);
    }

    [Fact]
    public void Raise_DifferentEventTypes_OnlyDispatchesToMatchingHandlers()
    {
        var dispatcher = new DomainEventDispatcher();
        var levelUpCount = 0;
        var diedCount = 0;

        dispatcher.Register<LevelUpEvent>(_ => levelUpCount++);
        dispatcher.Register<PlayerDiedEvent>(_ => diedCount++);

        dispatcher.Raise(new LevelUpEvent(new SkillName("Attack"), 5));

        Assert.Equal(1, levelUpCount);
        Assert.Equal(0, diedCount);
    }

    [Fact]
    public void Raise_WithTypedHandler_InvokesHandler()
    {
        var dispatcher = new DomainEventDispatcher();
        var handler = new TestLevelUpHandler();

        dispatcher.Register<LevelUpEvent>(handler);
        dispatcher.Raise(new LevelUpEvent(new SkillName("Attack"), 10));

        Assert.Equal(1, handler.HandleCount);
        Assert.Equal(10, handler.LastNewLevel);
    }

    private sealed class TestLevelUpHandler : IDomainEventHandler<LevelUpEvent>
    {
        public int HandleCount { get; private set; }
        public int LastNewLevel { get; private set; }

        public void Handle(LevelUpEvent @event)
        {
            HandleCount++;
            LastNewLevel = @event.NewLevel;
        }
    }
}
