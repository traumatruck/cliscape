using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Game.Skills;
using CliScape.Tests.Shared;
using NSubstitute;

namespace CliScape.Game.Tests;

public sealed class TestWoodcuttingService
{
    private readonly StubEventDispatcher _events = new();
    private readonly IToolChecker _toolChecker;
    private readonly WoodcuttingService _sut;

    private static readonly ItemId LogItemId = new(1200);
    private static readonly ItemId HatchetId = new(105);
    private static readonly IItem Logs = TestFactory.CreateStubItem(LogItemId, "Logs");

    private static IItem? DefaultResolver(ItemId id) =>
        id == LogItemId ? Logs : null;

    public TestWoodcuttingService()
    {
        _toolChecker = Substitute.For<IToolChecker>();
        _sut = new WoodcuttingService(_toolChecker, _events);
    }

    [Fact]
    public void CanChop_MeetsRequirements_Succeeds()
    {
        _toolChecker.HasAnyTool(Arg.Any<Core.Players.Player>(), Arg.Any<IEnumerable<ItemId>>(), out Arg.Any<ItemId?>())
            .Returns(x =>
            {
                x[2] = HatchetId;
                return true;
            });

        var player = TestFactory.CreatePlayer();

        var result = _sut.CanChop(player, 1);

        Assert.True(result.Success);
    }

    [Fact]
    public void CanChop_InsufficientLevel_Fails()
    {
        _toolChecker.HasAnyTool(Arg.Any<Core.Players.Player>(), Arg.Any<IEnumerable<ItemId>>(), out Arg.Any<ItemId?>())
            .Returns(true);

        var player = TestFactory.CreatePlayer();

        var result = _sut.CanChop(player, 50);

        Assert.False(result.Success);
        Assert.Contains("level", result.Message);
    }

    [Fact]
    public void CanChop_NoHatchet_Fails()
    {
        _toolChecker.HasAnyTool(Arg.Any<Core.Players.Player>(), Arg.Any<IEnumerable<ItemId>>(), out Arg.Any<ItemId?>())
            .Returns(false);

        var player = TestFactory.CreatePlayer();

        var result = _sut.CanChop(player, 1);

        Assert.False(result.Success);
        Assert.Contains("hatchet", result.Message);
    }

    [Fact]
    public void Chop_Success_AddsLogsAndGrantsXp()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.Chop(player, LogItemId, 25, 3, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(3, result.LogsObtained);
        Assert.Equal(75, result.TotalExperience);
        Assert.Equal("Logs", result.LogName);
    }

    [Fact]
    public void Chop_StopsWhenInventoryFull()
    {
        var player = TestFactory.CreatePlayer();
        // Fill 27 of 28 slots — room for 1 log
        for (var i = 0; i < 27; i++)
            player.Inventory.TryAdd(TestFactory.CreateStubItem(new ItemId(500 + i), $"Junk {i}"));

        var result = _sut.Chop(player, LogItemId, 25, 5, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(1, result.LogsObtained);
    }

    [Fact]
    public void Chop_InventoryAlreadyFull_Fails()
    {
        var player = TestFactory.CreatePlayer();
        for (var i = 0; i < 28; i++)
            player.Inventory.TryAdd(TestFactory.CreateStubItem(new ItemId(500 + i), $"Junk {i}"));

        var result = _sut.Chop(player, LogItemId, 25, 1, DefaultResolver);

        Assert.False(result.Success);
        Assert.Contains("full", result.Message);
    }

    [Fact]
    public void Chop_RaisesExperienceGainedEvent()
    {
        var player = TestFactory.CreatePlayer();

        _sut.Chop(player, LogItemId, 25, 1, DefaultResolver);

        _events.AssertRaised<ExperienceGainedEvent>();
    }

    [Fact]
    public void Chop_NullItemResolver_Fails()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.Chop(player, LogItemId, 25, 1, _ => null);

        Assert.False(result.Success);
    }
}
