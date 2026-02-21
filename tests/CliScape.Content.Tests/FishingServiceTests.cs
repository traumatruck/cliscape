using CliScape.Content.Tests.Helpers;
using CliScape.Core.Items;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game.Skills;
using NSubstitute;

namespace CliScape.Content.Tests;

public class FishingServiceTests
{
    private readonly StubEventDispatcher _events = new();
    private readonly StubRandomProvider _random = new();
    private readonly IToolChecker _toolChecker = Substitute.For<IToolChecker>();
    private readonly FishingService _service;

    public FishingServiceTests()
    {
        _service = new FishingService(_random, _toolChecker, _events);
    }

    private static IFishingSpot CreateSpot(int requiredLevel = 1, ItemId? requiredTool = null)
    {
        var spot = Substitute.For<IFishingSpot>();
        spot.Name.Returns("Test Spot");
        spot.RequiredLevel.Returns(requiredLevel);
        spot.RequiredTool.Returns(requiredTool ?? new ItemId(700));
        spot.MaxActions.Returns(10);
        spot.PossibleCatches.Returns(new[]
        {
            new FishingYield(new ItemId(1300), requiredLevel, 10)
        });
        return spot;
    }

    private static IItem CreateFishItem(ItemId id)
    {
        var item = Substitute.For<IItem>();
        item.Id.Returns(id);
        item.Name.Returns(new ItemName("Raw shrimps"));
        item.IsStackable.Returns(false);
        return item;
    }

    [Fact]
    public void CanFish_LevelTooLow_Fails()
    {
        var player = TestFactory.CreatePlayer();
        var spot = CreateSpot(requiredLevel: 20);

        var result = _service.CanFish(player, spot);

        Assert.False(result.Success);
        Assert.Contains("level 20", result.Message);
    }

    [Fact]
    public void CanFish_NoTool_Fails()
    {
        var player = TestFactory.CreatePlayerWithSkill(SkillConstants.FishingSkillName, 20);
        var spot = CreateSpot(requiredLevel: 1);
        _toolChecker.HasTool(player, Arg.Any<ItemId>()).Returns(false);

        var result = _service.CanFish(player, spot);

        Assert.False(result.Success);
        Assert.Contains("tool", result.Message);
    }

    [Fact]
    public void CanFish_MeetsRequirements_Succeeds()
    {
        var player = TestFactory.CreatePlayerWithSkill(SkillConstants.FishingSkillName, 20);
        var spot = CreateSpot(requiredLevel: 1);
        _toolChecker.HasTool(player, Arg.Any<ItemId>()).Returns(true);

        var result = _service.CanFish(player, spot);

        Assert.True(result.Success);
    }

    [Fact]
    public void Fish_CatchesFish_GrantsXp()
    {
        var player = TestFactory.CreatePlayerWithSkill(SkillConstants.FishingSkillName, 5);
        var spot = CreateSpot(requiredLevel: 1);
        var fishItem = CreateFishItem(new ItemId(1300));

        // Random returns 0 for picking first (only) fish each time
        _random.WithDefault(0);

        IItem? ItemResolver(ItemId id) => id == new ItemId(1300) ? fishItem : null;

        var result = _service.Fish(player, spot, 1, ItemResolver);

        Assert.True(result.Success);
        Assert.Equal(10, result.TotalExperience);
        Assert.True(result.FishCaught.Count > 0);
    }

    [Fact]
    public void Fish_InventoryFull_ReportsError()
    {
        var player = TestFactory.CreatePlayerWithSkill(SkillConstants.FishingSkillName, 5);
        var spot = CreateSpot(requiredLevel: 1);

        // Fill inventory with dummy items
        for (var i = 0; i < 28; i++)
        {
            var dummy = Substitute.For<IItem>();
            dummy.Id.Returns(new ItemId(9000 + i));
            dummy.Name.Returns(new ItemName($"Dummy {i}"));
            dummy.IsStackable.Returns(false);
            player.Inventory.TryAdd(dummy);
        }

        _random.WithDefault(0);
        IItem? ItemResolver(ItemId id) => null;

        var result = _service.Fish(player, spot, 1, ItemResolver);

        Assert.False(result.Success);
        Assert.Contains("inventory is full", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Fish_MultipleCatches_AccumulatesXp()
    {
        var player = TestFactory.CreatePlayerWithSkill(SkillConstants.FishingSkillName, 5);
        var spot = CreateSpot(requiredLevel: 1);
        var fishItem = CreateFishItem(new ItemId(1300));

        _random.WithDefault(0);
        IItem? ItemResolver(ItemId id) => id == new ItemId(1300) ? fishItem : null;

        var result = _service.Fish(player, spot, 3, ItemResolver);

        Assert.True(result.Success);
        Assert.Equal(30, result.TotalExperience); // 10 XP * 3 catches
    }
}
