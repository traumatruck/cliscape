using CliScape.Core;
using CliScape.Core.ClueScrolls;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Npcs;
using CliScape.Core.World;
using CliScape.Game.ClueScrolls;
using CliScape.Tests.Shared;
using NSubstitute;

namespace CliScape.Game.Tests;

public sealed class TestClueScrollService
{
    private readonly StubRandomProvider _random = new(0);
    private readonly StubEventDispatcher _events = new();
    private readonly IClueStepPool _stepPool;
    private readonly IClueRewardTable _rewardTable;
    private readonly ClueScrollService _sut;

    private static readonly ClueStep StepLumbridge = new()
    {
        StepType = ClueStepType.Dig,
        HintText = "Dig near the castle.",
        RequiredLocation = new LocationName("Lumbridge"),
        CompletionText = "You found something!"
    };

    private static readonly ClueStep StepVarrock = new()
    {
        StepType = ClueStepType.Search,
        HintText = "Search the palace.",
        RequiredLocation = new LocationName("Varrock"),
        CompletionText = "A hidden compartment!"
    };

    private static readonly ClueStep StepFalador = new()
    {
        StepType = ClueStepType.Talk,
        HintText = "Talk to the white knight.",
        RequiredLocation = new LocationName("Falador"),
        CompletionText = "The knight reveals a secret."
    };

    public TestClueScrollService()
    {
        _stepPool = Substitute.For<IClueStepPool>();
        _rewardTable = Substitute.For<IClueRewardTable>();

        // Default step pool: 3 distinct steps
        _stepPool.GetSteps(Arg.Any<ClueScrollTier>())
            .Returns([StepLumbridge, StepVarrock, StepFalador]);

        // Default reward table
        _rewardTable.GetRewards(Arg.Any<ClueScrollTier>())
            .Returns([
                new ClueReward { ItemId = new ItemId(1), Weight = 10, MinQuantity = 1, MaxQuantity = 5 }
            ]);
        _rewardTable.GetRewardRollCount(Arg.Any<ClueScrollTier>()).Returns(2);

        _sut = new ClueScrollService(_random, _events, _stepPool, _rewardTable);
    }

    /// <summary>
    ///     Creates a location with a specific name for clue scroll testing.
    /// </summary>
    private static ILocation MakeLocation(string name)
    {
        var loc = Substitute.For<ILocation>();
        loc.Name.Returns(new LocationName(name));
        return loc;
    }

    [Fact]
    public void StartClue_CreatesClueWithCorrectTier()
    {
        // random.Next(2,4) for Easy step count — returns clamped default (0→2)
        _random.Returns(2); // step count
        // random.Next(1000) for shuffling (3 steps)
        _random.EnqueueRange(100, 200, 300);

        var player = TestFactory.CreatePlayer();

        var result = _sut.StartClue(player, ClueScrollTier.Easy);

        var started = Assert.IsType<ClueStartResult.Started>(result);
        Assert.Equal(ClueScrollTier.Easy, started.Clue.Tier);
        Assert.InRange(started.Clue.TotalSteps, 2, 3);
        Assert.NotNull(player.ActiveClue);
    }

    [Fact]
    public void StartClue_WithExistingClue_ReturnsAlreadyHaveClue()
    {
        var player = TestFactory.CreatePlayer();
        player.ActiveClue = new ClueScroll
        {
            Tier = ClueScrollTier.Medium,
            Steps = [StepLumbridge],
            CurrentStepIndex = 0
        };

        var result = _sut.StartClue(player, ClueScrollTier.Easy);

        Assert.IsType<ClueStartResult.AlreadyHaveClue>(result);
    }

    [Fact]
    public void AttemptStep_NoActiveClue_ReturnsNoActiveClue()
    {
        var player = TestFactory.CreatePlayer();

        var result = _sut.AttemptStep(player);

        Assert.IsType<ClueStepResult.NoActiveClue>(result);
    }

    [Fact]
    public void AttemptStep_WrongLocation_ReturnsWrongLocation()
    {
        var player = TestFactory.CreatePlayer();
        player.ActiveClue = new ClueScroll
        {
            Tier = ClueScrollTier.Easy,
            Steps = [StepLumbridge],
            CurrentStepIndex = 0
        };
        // Player is at "Test Location" (default), step requires "Lumbridge"

        var result = _sut.AttemptStep(player);

        var wrong = Assert.IsType<ClueStepResult.WrongLocation>(result);
        Assert.Equal("Lumbridge", wrong.RequiredLocation);
    }

    [Fact]
    public void AttemptStep_CorrectLocation_AdvancesStep()
    {
        var player = TestFactory.CreatePlayer();
        player.CurrentLocation = MakeLocation("Lumbridge");
        player.ActiveClue = new ClueScroll
        {
            Tier = ClueScrollTier.Easy,
            Steps = [StepLumbridge, StepVarrock],
            CurrentStepIndex = 0
        };

        var result = _sut.AttemptStep(player);

        var completed = Assert.IsType<ClueStepResult.StepCompleted>(result);
        Assert.Equal(1, completed.StepNumber);
        Assert.Equal(2, completed.TotalSteps);
        Assert.NotNull(completed.NextStep);
        Assert.NotNull(player.ActiveClue);
        Assert.Equal(1, player.ActiveClue.CurrentStepIndex);
    }

    [Fact]
    public void AttemptStep_FinalStep_CompletesClue()
    {
        var player = TestFactory.CreatePlayer();
        player.CurrentLocation = MakeLocation("Lumbridge");
        player.ActiveClue = new ClueScroll
        {
            Tier = ClueScrollTier.Easy,
            Steps = [StepLumbridge],
            CurrentStepIndex = 0
        };

        var result = _sut.AttemptStep(player);

        var clueCompleted = Assert.IsType<ClueStepResult.ClueCompleted>(result);
        Assert.Equal(ClueScrollTier.Easy, clueCompleted.Tier);
        Assert.Null(player.ActiveClue);
    }

    [Fact]
    public void AttemptStep_RaisesClueStepCompletedEvent()
    {
        var player = TestFactory.CreatePlayer();
        player.CurrentLocation = MakeLocation("Lumbridge");
        player.ActiveClue = new ClueScroll
        {
            Tier = ClueScrollTier.Easy,
            Steps = [StepLumbridge, StepVarrock],
            CurrentStepIndex = 0
        };

        _sut.AttemptStep(player);

        _events.AssertRaised<ClueStepCompletedEvent>();
    }

    [Fact]
    public void AttemptStep_FinalStep_RaisesClueScrollCompletedEvent()
    {
        var player = TestFactory.CreatePlayer();
        player.CurrentLocation = MakeLocation("Lumbridge");
        player.ActiveClue = new ClueScroll
        {
            Tier = ClueScrollTier.Hard,
            Steps = [StepLumbridge],
            CurrentStepIndex = 0
        };

        _sut.AttemptStep(player);

        _events.AssertRaised<ClueScrollCompletedEvent>();
    }

    [Fact]
    public void RollRewards_ReturnsItemsBasedOnRewardTable()
    {
        // 2 rolls; each picks the only reward
        _random.EnqueueRange(0, 0, 1, 3); // two weight rolls, two quantity rolls

        var rewards = _sut.RollRewards(ClueScrollTier.Easy);

        Assert.Equal(2, rewards.Count);
        Assert.All(rewards, r => Assert.Equal(1, r.ItemId.Value));
    }

    [Fact]
    public void RollRewards_EmptyRewardTable_ReturnsEmpty()
    {
        _rewardTable.GetRewards(ClueScrollTier.Elite).Returns(new List<ClueReward>());

        var rewards = _sut.RollRewards(ClueScrollTier.Elite);

        Assert.Empty(rewards);
    }
}
