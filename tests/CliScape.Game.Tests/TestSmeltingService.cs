using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Game.Skills;
using CliScape.Tests.Shared;

namespace CliScape.Game.Tests;

public sealed class TestSmeltingService
{
    private readonly StubEventDispatcher _events = new();
    private readonly SmeltingService _sut;

    // Bronze bar: tin + copper, level 1, 6.2 xp (rounded to 6)
    private static readonly ItemId TinOreId = new(1100);
    private static readonly ItemId CopperOreId = new(1101);
    private static readonly ItemId BronzeBarId = new(1400);

    private static readonly SmeltingRecipe BronzeRecipe = new(
        BronzeBarId, TinOreId, CopperOreId, 1, 1, 6);

    // Simple recipe with no secondary ore
    private static readonly SmeltingRecipe SimpleRecipe = new(
        BronzeBarId, TinOreId, null, 0, 1, 6);

    private static readonly IItem TinOre = TestFactory.CreateStubItem(TinOreId, "Tin ore");
    private static readonly IItem CopperOre = TestFactory.CreateStubItem(CopperOreId, "Copper ore");
    private static readonly IItem BronzeBar = TestFactory.CreateStubItem(BronzeBarId, "Bronze bar");

    private static IItem? DefaultResolver(ItemId id) =>
        id.Value switch
        {
            1100 => TinOre,
            1101 => CopperOre,
            1400 => BronzeBar,
            _ => null
        };

    public TestSmeltingService()
    {
        _sut = new SmeltingService(_events);
    }

    [Fact]
    public void CanSmelt_SufficientLevel_Succeeds()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.CanSmelt(player, 1);

        Assert.True(result.Success);
    }

    [Fact]
    public void CanSmelt_InsufficientLevel_Fails()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.CanSmelt(player, 50);

        Assert.False(result.Success);
        Assert.Contains("level", result.Message);
    }

    [Fact]
    public void Smelt_WithSecondaryOre_CreatesBars()
    {
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(TinOre);
        player.Inventory.TryAdd(CopperOre);

        var result = _sut.Smelt(player, BronzeRecipe, 1, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(1, result.BarsSmelted);
        Assert.Equal(6, result.TotalExperience);
        Assert.Equal("Bronze bar", result.BarName);
    }

    [Fact]
    public void Smelt_WithoutSecondaryOre_CreatesBars()
    {
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(TinOre);

        var result = _sut.Smelt(player, SimpleRecipe, 1, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(1, result.BarsSmelted);
    }

    [Fact]
    public void Smelt_MissingPrimaryOre_Fails()
    {
        var player = TestFactory.CreatePlayer();
        // No ores in inventory

        var result = _sut.Smelt(player, SimpleRecipe, 1, DefaultResolver);

        Assert.False(result.Success);
        Assert.Contains("don't have", result.Message);
    }

    [Fact]
    public void Smelt_MissingSecondaryOre_Fails()
    {
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(TinOre);
        // Missing copper ore for bronze recipe

        var result = _sut.Smelt(player, BronzeRecipe, 1, DefaultResolver);

        Assert.False(result.Success);
        Assert.Contains("need", result.Message);
    }

    [Fact]
    public void Smelt_MultipleBars_ConsumesCorrectAmounts()
    {
        var player = TestFactory.CreatePlayer();
        for (var i = 0; i < 3; i++)
        {
            player.Inventory.TryAdd(TinOre);
            player.Inventory.TryAdd(CopperOre);
        }

        var result = _sut.Smelt(player, BronzeRecipe, 3, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(3, result.BarsSmelted);
        Assert.Equal(18, result.TotalExperience);
    }

    [Fact]
    public void Smelt_StopsWhenInventoryFull()
    {
        var player = TestFactory.CreatePlayer();
        // Fill 26 slots with junk, add 1 tin ore = 27 slots total (room for 1 bar)
        for (var i = 0; i < 26; i++)
            player.Inventory.TryAdd(TestFactory.CreateStubItem(new ItemId(500 + i), $"Junk {i}"));
        player.Inventory.TryAdd(TinOre);

        // Removes tin ore (26 slots), adds bar (27 slots), next iter has no ore → stops
        var result = _sut.Smelt(player, SimpleRecipe, 5, DefaultResolver);

        Assert.True(result.Success);
        Assert.Equal(1, result.BarsSmelted);
    }

    [Fact]
    public void Smelt_RaisesExperienceGainedEvent()
    {
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(TinOre);

        _sut.Smelt(player, SimpleRecipe, 1, DefaultResolver);

        _events.AssertRaised<ExperienceGainedEvent>();
    }

    [Fact]
    public void Smelt_NullItemResolver_Fails()
    {
        var player = TestFactory.CreatePlayer();
        player.Inventory.TryAdd(TinOre);

        var result = _sut.Smelt(player, SimpleRecipe, 1, _ => null);

        Assert.False(result.Success);
    }
}
