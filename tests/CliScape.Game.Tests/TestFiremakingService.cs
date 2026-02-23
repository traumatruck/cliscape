using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players.Skills;
using CliScape.Game.Skills;
using CliScape.Tests.Shared;
using NSubstitute;

namespace CliScape.Game.Tests;

public sealed class TestFiremakingService
{
    private readonly StubEventDispatcher _events = new();
    private readonly FiremakingService _sut;

    private static readonly ItemId TinderboxId = new(700);
    private static readonly ItemId NormalLogId = new(1200);
    private static readonly ItemId OakLogId = new(1201);

    private static IItem MakeTinderbox()
    {
        return TestFactory.CreateStubItem(TinderboxId, "Tinderbox");
    }

    private static IItem MakeLogs(ItemId? id = null, string name = "Logs")
    {
        return TestFactory.CreateStubItem(id ?? NormalLogId, name);
    }

    private static IItem MakeNonLog()
    {
        return TestFactory.CreateStubItem(new ItemId(9999), "Bucket");
    }

    public TestFiremakingService()
    {
        _sut = new FiremakingService(_events);
    }

    [Fact]
    public void IsTinderbox_CorrectItem_ReturnsTrue()
    {
        var tinderbox = MakeTinderbox();

        Assert.True(_sut.IsTinderbox(tinderbox));
    }

    [Fact]
    public void IsTinderbox_WrongItem_ReturnsFalse()
    {
        var bucket = MakeNonLog();

        Assert.False(_sut.IsTinderbox(bucket));
    }

    [Fact]
    public void IsLog_NormalLogs_ReturnsTrue()
    {
        var logs = MakeLogs();

        Assert.True(_sut.IsLog(logs));
    }

    [Fact]
    public void IsLog_OakLogs_ReturnsTrue()
    {
        var logs = MakeLogs(OakLogId, "Oak logs");

        Assert.True(_sut.IsLog(logs));
    }

    [Fact]
    public void IsLog_NonLog_ReturnsFalse()
    {
        var bucket = MakeNonLog();

        Assert.False(_sut.IsLog(bucket));
    }

    [Fact]
    public void CanLight_WithLogs_Succeeds()
    {
        var player = TestFactory.CreatePlayer();
        var logs = MakeLogs();

        var result = _sut.CanLight(player, logs);

        Assert.True(result.Success);
    }

    [Fact]
    public void CanLight_NonLog_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var bucket = MakeNonLog();

        var result = _sut.CanLight(player, bucket);

        Assert.False(result.Success);
        Assert.Contains("can't light", result.Message);
    }

    [Fact]
    public void CanLight_InsufficientLevel_Fails()
    {
        var player = TestFactory.CreatePlayer();
        // Oak logs require level 15
        var oakLogs = MakeLogs(OakLogId, "Oak logs");

        var result = _sut.CanLight(player, oakLogs);

        Assert.False(result.Success);
        Assert.Contains("level", result.Message);
    }

    [Fact]
    public void LightLogs_Success_RemovesLogsAndGrantsXp()
    {
        var player = TestFactory.CreatePlayer();
        var logs = MakeLogs();
        player.Inventory.TryAdd(logs);

        var result = _sut.LightLogs(player, logs);

        Assert.True(result.Success);
        Assert.Equal(40, result.ExperienceGained);
        Assert.False(player.Inventory.Contains(logs));
    }

    [Fact]
    public void LightLogs_RaisesExperienceGainedEvent()
    {
        var player = TestFactory.CreatePlayer();
        var logs = MakeLogs();
        player.Inventory.TryAdd(logs);

        _sut.LightLogs(player, logs);

        _events.AssertRaised<ExperienceGainedEvent>();
    }

    [Fact]
    public void LightLogs_InsufficientLevel_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var oakLogs = MakeLogs(OakLogId, "Oak logs");
        player.Inventory.TryAdd(oakLogs);

        var result = _sut.LightLogs(player, oakLogs);

        Assert.False(result.Success);
    }

    [Fact]
    public void CanUseForFiremaking_TinderboxAndLogs_ReturnsTrue()
    {
        var tinderbox = MakeTinderbox();
        var logs = MakeLogs();

        var canUse = _sut.CanUseForFiremaking(tinderbox, logs, out var outTinderbox, out var outLogs);

        Assert.True(canUse);
        Assert.Same(tinderbox, outTinderbox);
        Assert.Same(logs, outLogs);
    }

    [Fact]
    public void CanUseForFiremaking_LogsAndTinderbox_Reversed_ReturnsTrue()
    {
        var tinderbox = MakeTinderbox();
        var logs = MakeLogs();

        var canUse = _sut.CanUseForFiremaking(logs, tinderbox, out var outTinderbox, out var outLogs);

        Assert.True(canUse);
        Assert.Same(tinderbox, outTinderbox);
        Assert.Same(logs, outLogs);
    }

    [Fact]
    public void CanUseForFiremaking_TwoLogs_ReturnsFalse()
    {
        var logs1 = MakeLogs();
        var logs2 = MakeLogs(OakLogId, "Oak logs");

        var canUse = _sut.CanUseForFiremaking(logs1, logs2, out _, out _);

        Assert.False(canUse);
    }

    [Fact]
    public void CanUseForFiremaking_TwoNonCombatible_ReturnsFalse()
    {
        var bucket = MakeNonLog();
        var logs = MakeLogs();

        var canUse = _sut.CanUseForFiremaking(bucket, logs, out _, out _);

        Assert.False(canUse);
    }
}
