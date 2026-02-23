using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game.Skills;
using CliScape.Tests.Shared;
using NSubstitute;

namespace CliScape.Game.Tests;

/// <summary>
///     Concrete test implementation of IMiningRock to avoid NSubstitute value-type issues.
/// </summary>
file sealed class TestMiningRock(
    string name,
    RockType rockType,
    ItemId oreItemId,
    int requiredLevel,
    int experience,
    int maxActions,
    PickaxeTier requiredPickaxe) : IMiningRock
{
    public string Name => name;
    public RockType RockType => rockType;
    public ItemId OreItemId => oreItemId;
    public int RequiredLevel => requiredLevel;
    public int Experience => experience;
    public int MaxActions => maxActions;
    public PickaxeTier RequiredPickaxe => requiredPickaxe;
}

public sealed class TestMiningService
{
    private readonly StubEventDispatcher _events = new();
    private readonly IToolChecker _toolChecker;
    private readonly MiningService _sut;

    private static readonly ItemId CopperOreId = new(1100);
    private static readonly ItemId BronzePickaxeId = new(701);
    private static readonly IItem CopperOre = TestFactory.CreateStubItem(CopperOreId, "Copper ore");

    private static IItem? DefaultResolver(ItemId id) =>
        id == CopperOreId ? CopperOre : null;

    private static IMiningRock MakeRock(
        int requiredLevel = 1,
        int experience = 18,
        PickaxeTier requiredPickaxe = PickaxeTier.Bronze)
    {
        return new TestMiningRock(
            "Copper rock", RockType.Copper, CopperOreId,
            requiredLevel, experience, 5, requiredPickaxe);
    }

    public TestMiningService()
    {
        _toolChecker = Substitute.For<IToolChecker>();
        _sut = new MiningService(_toolChecker, _events);
    }

    [Fact]
    public void CanMine_MeetsRequirements_Succeeds()
    {
        _toolChecker.HasTool(Arg.Any<Core.Players.Player>(), BronzePickaxeId).Returns(true);

        var player = TestFactory.CreatePlayer();
        var rock = MakeRock();

        var result = _sut.CanMine(player, rock);

        Assert.True(result.Success);
        Assert.Contains("pickaxe", result.Value!);
    }

    [Fact]
    public void CanMine_InsufficientLevel_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var rock = MakeRock(requiredLevel: 50);

        var result = _sut.CanMine(player, rock);

        Assert.False(result.Success);
        Assert.Contains("level", result.Message);
    }

    [Fact]
    public void CanMine_NoPickaxe_Fails()
    {
        // ToolChecker returns false for all tool queries
        _toolChecker.HasTool(Arg.Any<Core.Players.Player>(), Arg.Any<ItemId>()).Returns(false);

        var player = TestFactory.CreatePlayer();
        var rock = MakeRock();

        var result = _sut.CanMine(player, rock);

        Assert.False(result.Success);
        Assert.Contains("pickaxe", result.Message);
    }

    [Fact]
    public void Mine_Success_AddsOresAndGrantsXp()
    {
        var player = TestFactory.CreatePlayer();
        var rock = MakeRock(experience: 18);

        var result = _sut.Mine(player, rock, 3, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(3, result.OresObtained);
        Assert.Equal(54, result.TotalExperience);
        Assert.Equal("Copper ore", result.OreName);
    }

    [Fact]
    public void Mine_StopsWhenInventoryFull()
    {
        var player = TestFactory.CreatePlayer();
        for (var i = 0; i < 27; i++)
            player.Inventory.TryAdd(TestFactory.CreateStubItem(new ItemId(500 + i), $"Junk {i}"));

        var rock = MakeRock();

        var result = _sut.Mine(player, rock, 5, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(1, result.OresObtained);
    }

    [Fact]
    public void Mine_InventoryAlreadyFull_Fails()
    {
        var player = TestFactory.CreatePlayer();
        for (var i = 0; i < 28; i++)
            player.Inventory.TryAdd(TestFactory.CreateStubItem(new ItemId(500 + i), $"Junk {i}"));

        var rock = MakeRock();

        var result = _sut.Mine(player, rock, 1, DefaultResolver);

        Assert.False(result.Success);
        Assert.Contains("full", result.Message);
    }

    [Fact]
    public void Mine_RaisesExperienceGainedEvent()
    {
        var player = TestFactory.CreatePlayer();

        _sut.Mine(player, MakeRock(), 1, DefaultResolver);

        _events.AssertRaised<ExperienceGainedEvent>();
    }

    [Fact]
    public void Mine_NullItemResolver_Fails()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.Mine(player, MakeRock(), 1, _ => null);

        Assert.False(result.Success);
    }
}
