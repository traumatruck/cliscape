using CliScape.Core.Combat;
using CliScape.Core.Npcs;
using CliScape.Core.Players.Skills;
using CliScape.Tests.Shared;
using NSubstitute;

namespace CliScape.Core.Tests;

public sealed class TestCombatSession
{
    private static ICombatableNpc MakeNpc(int hp = 10)
    {
        var npc = Substitute.For<ICombatableNpc>();
        npc.Hitpoints.Returns(hp);
        npc.Name.Returns(new NpcName("Test NPC"));
        npc.Attacks.Returns(Array.Empty<NpcAttack>());
        return npc;
    }

    [Fact]
    public void NewSession_InitialState()
    {
        var player = TestFactory.CreatePlayer();
        var npc = MakeNpc(20);
        var session = new CombatSession(player, npc, new StubRandomProvider());

        Assert.Equal(20, session.NpcCurrentHitpoints);
        Assert.Equal(0, session.TurnCount);
        Assert.False(session.IsComplete);
        Assert.False(session.PlayerWon);
        Assert.False(session.PlayerDied);
        Assert.False(session.PlayerFled);
    }

    [Fact]
    public void DamageNpc_ReducesHitpoints()
    {
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(20), new StubRandomProvider());

        session.DamageNpc(5);
        Assert.Equal(15, session.NpcCurrentHitpoints);
    }

    [Fact]
    public void DamageNpc_ToZero_CompletesSession_PlayerWon()
    {
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(10), new StubRandomProvider());

        session.DamageNpc(10);

        Assert.True(session.IsComplete);
        Assert.True(session.PlayerWon);
        Assert.False(session.PlayerDied);
        Assert.Equal(0, session.NpcCurrentHitpoints);
    }

    [Fact]
    public void DamageNpc_OverKill_ClampsToZero()
    {
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(5), new StubRandomProvider());

        session.DamageNpc(100);
        Assert.Equal(0, session.NpcCurrentHitpoints);
    }

    [Fact]
    public void DamagePlayer_ReducesHealthOnPlayer()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 20, maxHealth: 20);
        var session = new CombatSession(player, MakeNpc(), new StubRandomProvider());

        session.DamagePlayer(8);
        Assert.Equal(12, player.CurrentHealth);
    }

    [Fact]
    public void DamagePlayer_ToZero_CompletesSession_PlayerDied()
    {
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(), new StubRandomProvider());

        session.DamagePlayer(SkillConstants.StartingHitpoints);

        Assert.True(session.IsComplete);
        Assert.True(session.PlayerDied);
        Assert.False(session.PlayerWon);
    }

    [Fact]
    public void NextTurn_IncrementsTurnCount()
    {
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(), new StubRandomProvider());

        session.NextTurn();
        session.NextTurn();

        Assert.Equal(2, session.TurnCount);
    }

    [Fact]
    public void AttemptFlee_Turn0_25PercentChance_SucceedsOnLowRoll()
    {
        // Flee chance = 25 + 0*5 = 25; roll must be < 25
        var random = new StubRandomProvider().Returns(10); // 10 < 25 → success
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(), random);

        var fled = session.AttemptFlee();

        Assert.True(fled);
        Assert.True(session.PlayerFled);
        Assert.True(session.IsComplete);
    }

    [Fact]
    public void AttemptFlee_Turn0_FailsOnHighRoll()
    {
        // Flee chance = 25; roll must be < 25
        var random = new StubRandomProvider().Returns(50); // 50 >= 25 → fail
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(), random);

        var fled = session.AttemptFlee();

        Assert.False(fled);
        Assert.False(session.PlayerFled);
        Assert.False(session.IsComplete);
    }

    [Fact]
    public void AttemptFlee_ChanceIncreasesPerTurn()
    {
        // Turn 5: flee chance = min(75, 25 + 5*5) = 50; roll 40 < 50 → success
        var random = new StubRandomProvider().Returns(40);
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(), random);

        for (var i = 0; i < 5; i++) session.NextTurn();

        var fled = session.AttemptFlee();
        Assert.True(fled);
    }

    [Fact]
    public void AttemptFlee_CapsAt75Percent()
    {
        // Turn 20: flee chance = min(75, 25 + 20*5) = 75; roll 74 < 75 → success
        var random = new StubRandomProvider().Returns(74);
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(), random);

        for (var i = 0; i < 20; i++) session.NextTurn();

        var fled = session.AttemptFlee();
        Assert.True(fled);
    }

    [Fact]
    public void AttemptFlee_75Percent_Roll75_Fails()
    {
        // Turn 20: flee chance = 75; roll 75 >= 75 → fail
        var random = new StubRandomProvider().Returns(75);
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(), random);

        for (var i = 0; i < 20; i++) session.NextTurn();

        var fled = session.AttemptFlee();
        Assert.False(fled);
    }

    [Fact]
    public void Rewards_InitializedEmpty()
    {
        var player = TestFactory.CreatePlayer();
        var session = new CombatSession(player, MakeNpc(), new StubRandomProvider());

        Assert.Equal(0, session.Rewards.TotalExperience);
        Assert.Empty(session.Rewards.LevelUps);
    }
}
