using CliScape.Core;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game.Skills;
using CliScape.Tests.Shared;
using NSubstitute;

namespace CliScape.Game.Tests;

public sealed class TestCookingService
{
    private readonly StubRandomProvider _random = new(0);
    private readonly StubEventDispatcher _events = new();
    private readonly CookingService _sut;

    // Shrimp recipe: Level 1, XP 30, stop burn at 34
    private static readonly CookingRecipe ShrimpRecipe = new(
        new ItemId(1300), new ItemId(1002), new ItemId(1800), 1, 30, 34);

    private static readonly IItem RawShrimps = TestFactory.CreateStubItem(new ItemId(1300), "Raw shrimps");
    private static readonly IItem CookedShrimps = TestFactory.CreateStubItem(new ItemId(1002), "Shrimps");
    private static readonly IItem BurntShrimps = TestFactory.CreateStubItem(new ItemId(1800), "Burnt shrimps");

    private static IItem? DefaultResolver(ItemId id) =>
        id.Value switch
        {
            1300 => RawShrimps,
            1002 => CookedShrimps,
            1800 => BurntShrimps,
            _ => null
        };

    private static ICookingRange MakeRange(int burnChanceReduction = 0)
    {
        var range = Substitute.For<ICookingRange>();
        range.Name.Returns("Range");
        range.SourceType.Returns(CookingSourceType.Range);
        range.BurnChanceReduction.Returns(burnChanceReduction);
        return range;
    }

    public TestCookingService()
    {
        _sut = new CookingService(_random, _events);
    }

    [Fact]
    public void CanCook_SufficientLevel_Succeeds()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.CanCook(player, 1);

        Assert.True(result.Success);
    }

    [Fact]
    public void CanCook_InsufficientLevel_Fails()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.CanCook(player, 50);

        Assert.False(result.Success);
        Assert.Contains("level", result.Message);
    }

    [Fact]
    public void Cook_Success_CooksItemAndGrantsXp()
    {
        // random.NextDouble() returns 99/100 = 0.99 → 0.99 >= burnChance (0.6) → no burn
        _random.Returns(99);
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(RawShrimps);

        var result = _sut.Cook(player, MakeRange(), ShrimpRecipe, 1, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(1, result.ItemsCooked);
        Assert.Equal(0, result.ItemsBurnt);
        Assert.Equal(30, result.TotalExperience);
    }

    [Fact]
    public void Cook_BurnsItem_WhenRandomRollLow()
    {
        // random.NextDouble() returns 0/100 = 0.0 → 0.0 < burnChance (0.6) → burns
        _random.Returns(0);
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(RawShrimps);

        var result = _sut.Cook(player, MakeRange(), ShrimpRecipe, 1, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(0, result.ItemsCooked);
        Assert.Equal(1, result.ItemsBurnt);
        Assert.Equal(0, result.TotalExperience);
    }

    [Fact]
    public void Cook_NoRawFood_Fails()
    {
        var player = TestFactory.CreatePlayer();
        // Inventory is empty — no raw shrimps

        var result = _sut.Cook(player, MakeRange(), ShrimpRecipe, 1, DefaultResolver);

        Assert.False(result.Success);
        Assert.Contains("don't have", result.Message);
    }

    [Fact]
    public void Cook_MultipleCooks_CountsCorrectly()
    {
        // First cook: no burn (NextDouble 99/100=0.99 >= 0.6)
        // Second cook: burns (NextDouble 0/100=0.0 < 0.6)
        _random.EnqueueRange(99, 0);
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(RawShrimps);
        player.Inventory.TryAdd(RawShrimps);

        var result = _sut.Cook(player, MakeRange(), ShrimpRecipe, 2, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(1, result.ItemsCooked);
        Assert.Equal(1, result.ItemsBurnt);
        Assert.Equal(30, result.TotalExperience);
    }

    [Fact]
    public void Cook_RaisesExperienceGainedEvent()
    {
        // Ensure no burn so XP is granted
        _random.Returns(99);
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(RawShrimps);

        _sut.Cook(player, MakeRange(), ShrimpRecipe, 1, DefaultResolver);

        _events.AssertRaised<ExperienceGainedEvent>();
    }

    [Fact]
    public void Cook_NullItemResolver_ReturnsFailure()
    {
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(RawShrimps);

        var result = _sut.Cook(player, MakeRange(), ShrimpRecipe, 1, _ => null);

        Assert.False(result.Success);
    }
}
