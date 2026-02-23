using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Slayer;
using CliScape.Game.Slayer;
using CliScape.Tests.Shared;

namespace CliScape.Game.Tests;

public sealed class TestSlayerService
{
    private static SlayerAssignment MakeAssignment(
        string category = "Goblins",
        int minKills = 10,
        int maxKills = 20,
        int weight = 1,
        int requiredSlayerLevel = 1)
    {
        return new SlayerAssignment
        {
            Category = new SlayerCategory(category),
            MinKills = minKills,
            MaxKills = maxKills,
            Weight = weight,
            RequiredSlayerLevel = requiredSlayerLevel
        };
    }

    private static SlayerMaster MakeMaster(
        string name = "Turael",
        int requiredCombatLevel = 1,
        int requiredSlayerLevel = 1,
        params SlayerAssignment[] assignments)
    {
        return new SlayerMaster
        {
            Name = new SlayerMasterName(name),
            RequiredCombatLevel = requiredCombatLevel,
            RequiredSlayerLevel = requiredSlayerLevel,
            Assignments = assignments.Length > 0 ? assignments : [MakeAssignment()]
        };
    }

    [Fact]
    public void AssignTask_GivesTaskFromMaster()
    {
        var random = new StubRandomProvider(0);
        // Roll for assignment selection and kill count
        random.EnqueueRange(0, 15);
        var sut = new SlayerService(random);

        var player = TestFactory.CreatePlayer();
        var master = MakeMaster();

        var result = sut.AssignTask(player, master);

        var assigned = Assert.IsType<SlayerTaskResult.TaskAssigned>(result);
        Assert.Equal("Goblins", assigned.Task.Category.Value);
        Assert.InRange(assigned.Task.TotalKills, 10, 20);
        Assert.Equal(assigned.Task.TotalKills, assigned.Task.RemainingKills);
        Assert.NotNull(player.SlayerTask);
    }

    [Fact]
    public void AssignTask_WithExistingTask_ReturnsAlreadyHaveTask()
    {
        var sut = new SlayerService(new StubRandomProvider());
        var player = TestFactory.CreatePlayer();
        player.SlayerTask = new SlayerTask
        {
            Category = new SlayerCategory("Cows"),
            RemainingKills = 5,
            TotalKills = 10,
            SlayerMaster = new SlayerMasterName("Turael")
        };

        var result = sut.AssignTask(player, MakeMaster());

        var already = Assert.IsType<SlayerTaskResult.AlreadyHaveTask>(result);
        Assert.Equal("Cows", already.CurrentTask.Category.Value);
    }

    [Fact]
    public void AssignTask_InsufficientCombatLevel_Fails()
    {
        var sut = new SlayerService(new StubRandomProvider());
        var player = TestFactory.CreatePlayer();
        var master = MakeMaster(requiredCombatLevel: 100);

        var result = sut.AssignTask(player, master);

        Assert.IsType<SlayerTaskResult.InsufficientCombatLevel>(result);
    }

    [Fact]
    public void AssignTask_InsufficientSlayerLevel_Fails()
    {
        var sut = new SlayerService(new StubRandomProvider());
        var player = TestFactory.CreatePlayer();
        var master = MakeMaster(requiredSlayerLevel: 50);

        var result = sut.AssignTask(player, master);

        Assert.IsType<SlayerTaskResult.InsufficientSlayerLevel>(result);
    }

    [Fact]
    public void AssignTask_NoMatchingAssignments_Fails()
    {
        var sut = new SlayerService(new StubRandomProvider());
        var player = TestFactory.CreatePlayer();
        // All assignments require high slayer level
        var master = MakeMaster(assignments: MakeAssignment(requiredSlayerLevel: 99));

        var result = sut.AssignTask(player, master);

        Assert.IsType<SlayerTaskResult.NoValidAssignments>(result);
    }

    [Fact]
    public void AssignTask_WeightedSelection_PicksCorrectAssignment()
    {
        // Two assignments: "Cows" (weight 1) and "Goblins" (weight 99)
        // Total weight = 100; roll of 0 picks "Cows", roll of 1+ picks "Goblins"
        var random = new StubRandomProvider();
        random.EnqueueRange(1, 15); // roll=1 → should pick "Goblins", killCount=15
        var sut = new SlayerService(random);

        var player = TestFactory.CreatePlayer();
        var master = MakeMaster(assignments: new[]
        {
            MakeAssignment(category: "Cows", weight: 1),
            MakeAssignment(category: "Goblins", weight: 99)
        });

        var result = sut.AssignTask(player, master);

        var assigned = Assert.IsType<SlayerTaskResult.TaskAssigned>(result);
        Assert.Equal("Goblins", assigned.Task.Category.Value);
    }

    [Fact]
    public void CancelTask_WithActiveTask_ReturnsTrue()
    {
        var sut = new SlayerService(new StubRandomProvider());
        var player = TestFactory.CreatePlayer();
        player.SlayerTask = new SlayerTask
        {
            Category = new SlayerCategory("Cows"),
            RemainingKills = 5,
            TotalKills = 10,
            SlayerMaster = new SlayerMasterName("Turael")
        };

        var cancelled = sut.CancelTask(player);

        Assert.True(cancelled);
        Assert.Null(player.SlayerTask);
    }

    [Fact]
    public void CancelTask_NoTask_ReturnsFalse()
    {
        var sut = new SlayerService(new StubRandomProvider());
        var player = TestFactory.CreatePlayer();

        Assert.False(sut.CancelTask(player));
    }
}
