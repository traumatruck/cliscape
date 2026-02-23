using CliScape.Tests.Shared;
using CliScape.Core;
using CliScape.Core.Combat;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Slayer;
using CliScape.Game.Combat;
using NSubstitute;

namespace CliScape.Content.Tests;

public class TestCombatEngine
{
    private readonly ICombatCalculator _calculator = Substitute.For<ICombatCalculator>();
    private readonly StubEventDispatcher _events = new();
    private readonly StubRandomProvider _random = new();
    private readonly CombatEngine _engine;

    public TestCombatEngine()
    {
        _engine = new CombatEngine(_calculator, _random, _events);
    }

    private static ICombatableNpc CreateNpc(int hitpoints = 10, int slayerXp = 0,
        SlayerCategory? slayerCategory = null, DropTable? dropTable = null)
    {
        var npc = Substitute.For<ICombatableNpc>();
        npc.Name.Returns(new NpcName("Test NPC"));
        npc.Hitpoints.Returns(hitpoints);
        npc.Attacks.Returns(new[] { new NpcAttack(NpcAttackStyle.Slash, 3) });
        npc.SlayerXp.Returns(slayerXp);
        npc.SlayerCategory.Returns(slayerCategory);
        npc.DropTable.Returns(dropTable ?? DropTable.Empty);
        npc.Immunities.Returns(new NpcImmunities());
        return npc;
    }

    [Fact]
    public void ExecuteTurn_PlayerHits_DamagesNpc()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 20);
        var session = new CombatSession(player, npc, _random);

        _calculator.CalculatePlayerAttackRoll(player).Returns(100);
        _calculator.CalculateNpcDefenceRoll(npc).Returns(50);
        _calculator.DoesAttackHit(100, 50).Returns(true);
        _calculator.CalculatePlayerMaxHit(player).Returns(10);
        _calculator.RollDamage(10).Returns(5);
        _calculator.CalculateExperience(5, CombatStyle.Accurate)
            .Returns(new CombatExperience(20, 0, 0, 7));

        // NPC counter-attack: miss
        _calculator.CalculateNpcAttackRoll(npc).Returns(10);
        _calculator.CalculatePlayerDefenceRoll(player, npc).Returns(100);
        _calculator.DoesAttackHit(10, 100).Returns(false);

        var result = _engine.ExecuteTurn(session, CombatStyle.Accurate);

        Assert.True(result.PlayerAttack.Hit);
        Assert.Equal(5, result.PlayerAttack.Damage);
        Assert.Equal(15, session.NpcCurrentHitpoints);
        Assert.False(result.CombatEnded);
    }

    [Fact]
    public void ExecuteTurn_PlayerMisses_NoDamage()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 20);
        var session = new CombatSession(player, npc, _random);

        _calculator.CalculatePlayerAttackRoll(player).Returns(10);
        _calculator.CalculateNpcDefenceRoll(npc).Returns(100);
        _calculator.DoesAttackHit(10, 100).Returns(false);

        // NPC counter-attack: miss too
        _calculator.CalculateNpcAttackRoll(npc).Returns(10);
        _calculator.CalculatePlayerDefenceRoll(player, npc).Returns(100);
        _calculator.DoesAttackHit(10, 100).Returns(false);

        var result = _engine.ExecuteTurn(session, CombatStyle.Accurate);

        Assert.False(result.PlayerAttack.Hit);
        Assert.Equal(0, result.PlayerAttack.Damage);
        Assert.Equal(20, session.NpcCurrentHitpoints);
    }

    [Fact]
    public void ExecuteTurn_KillsNpc_CombatEnds()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 5);
        var session = new CombatSession(player, npc, _random);

        _calculator.CalculatePlayerAttackRoll(player).Returns(100);
        _calculator.CalculateNpcDefenceRoll(npc).Returns(50);
        _calculator.DoesAttackHit(100, 50).Returns(true);
        _calculator.CalculatePlayerMaxHit(player).Returns(10);
        _calculator.RollDamage(10).Returns(5);
        _calculator.CalculateExperience(5, CombatStyle.Aggressive)
            .Returns(new CombatExperience(0, 20, 0, 7));

        var result = _engine.ExecuteTurn(session, CombatStyle.Aggressive);

        Assert.True(result.CombatEnded);
        Assert.Equal(0, session.NpcCurrentHitpoints);
        Assert.Null(result.NpcAttack); // NPC shouldn't attack when dead
    }

    [Fact]
    public void ExecuteTurn_NpcHitsPlayer_DamagesPlayer()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 20);
        var session = new CombatSession(player, npc, _random);

        // Player misses
        _calculator.CalculatePlayerAttackRoll(player).Returns(10);
        _calculator.CalculateNpcDefenceRoll(npc).Returns(100);
        _calculator.DoesAttackHit(10, 100).Returns(false);

        // NPC hits
        _calculator.CalculateNpcAttackRoll(npc).Returns(100);
        _calculator.CalculatePlayerDefenceRoll(player, npc).Returns(10);
        _calculator.DoesAttackHit(100, 10).Returns(true);
        _calculator.RollDamage(3).Returns(2);

        var result = _engine.ExecuteTurn(session, CombatStyle.Accurate);

        Assert.NotNull(result.NpcAttack);
        Assert.True(result.NpcAttack.Hit);
        Assert.Equal(2, result.NpcAttack.Damage);
        Assert.Equal(48, player.CurrentHealth);
    }

    [Fact]
    public void ProcessOutcome_PlayerWon_ReturnsVictory()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 1);
        var session = new CombatSession(player, npc, _random);

        // Kill the NPC
        session.DamageNpc(1);

        var result = _engine.ProcessOutcome(session);

        Assert.Equal(CombatOutcome.PlayerVictory, result.Outcome);
    }

    [Fact]
    public void ProcessOutcome_PlayerWon_RaisesCombatEndedEvent()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 1);
        var session = new CombatSession(player, npc, _random);
        session.DamageNpc(1);

        _engine.ProcessOutcome(session);

        var evt = _events.AssertRaised<CombatEndedEvent>();
        Assert.Equal(CombatOutcome.PlayerVictory, evt.Outcome);
    }

    [Fact]
    public void ProcessOutcome_PlayerDied_RaisesPlayerDiedEvent()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 1, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 20);
        var session = new CombatSession(player, npc, _random);

        // Kill the player
        session.DamagePlayer(1);

        var result = _engine.ProcessOutcome(session);

        Assert.Equal(CombatOutcome.PlayerDeath, result.Outcome);
        _events.AssertRaised<PlayerDiedEvent>();
        _events.AssertRaised<CombatEndedEvent>();
    }

    [Fact]
    public void ProcessOutcome_PlayerWon_WithSlayerTask_ProgressesTask()
    {
        var category = new SlayerCategory("Goblins");
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        player.SlayerTask = new SlayerTask
        {
            Category = category,
            RemainingKills = 2,
            TotalKills = 5,
            SlayerMaster = new SlayerMasterName("Turael")
        };

        var npc = CreateNpc(hitpoints: 1, slayerXp: 10, slayerCategory: category);
        var session = new CombatSession(player, npc, _random);
        session.DamageNpc(1);

        var result = _engine.ProcessOutcome(session);

        Assert.NotNull(result.SlayerProgress);
        Assert.Equal(1, result.SlayerProgress.RemainingKills);
        Assert.False(result.SlayerProgress.TaskComplete);
        Assert.Equal(10, result.SlayerProgress.SlayerXpGained);
    }

    [Fact]
    public void ProcessOutcome_PlayerWon_SlayerTaskCompletes_RaisesEvent()
    {
        var category = new SlayerCategory("Goblins");
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        player.SlayerTask = new SlayerTask
        {
            Category = category,
            RemainingKills = 1,
            TotalKills = 5,
            SlayerMaster = new SlayerMasterName("Turael")
        };

        var npc = CreateNpc(hitpoints: 1, slayerXp: 10, slayerCategory: category);
        var session = new CombatSession(player, npc, _random);
        session.DamageNpc(1);

        var result = _engine.ProcessOutcome(session);

        Assert.NotNull(result.SlayerProgress);
        Assert.True(result.SlayerProgress.TaskComplete);
        _events.AssertRaised<SlayerTaskCompletedEvent>();
    }

    [Fact]
    public void ProcessOutcome_PlayerWon_WithDrops_ReturnsDrops()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var dropTable = new DropTable(
            new NpcDrop { ItemId = new ItemId(100), Rarity = DropRarity.Always }
        );
        var npc = CreateNpc(hitpoints: 1, dropTable: dropTable);
        var session = new CombatSession(player, npc, _random);
        session.DamageNpc(1);

        // Always drops: Next(1, 2) == 1 is always true
        _random.EnqueueRange(1);

        var result = _engine.ProcessOutcome(session);

        Assert.Single(result.Drops);
        Assert.Equal(new ItemId(100), result.Drops[0].ItemId);
    }

    [Fact]
    public void ExecuteTurn_AwardsXpOnHit()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 20);
        var session = new CombatSession(player, npc, _random);

        _calculator.CalculatePlayerAttackRoll(player).Returns(100);
        _calculator.CalculateNpcDefenceRoll(npc).Returns(50);
        _calculator.DoesAttackHit(100, 50).Returns(true);
        _calculator.CalculatePlayerMaxHit(player).Returns(10);
        _calculator.RollDamage(10).Returns(5);
        _calculator.CalculateExperience(5, CombatStyle.Accurate)
            .Returns(new CombatExperience(20, 0, 0, 7));

        // NPC counter-attack: miss
        _calculator.CalculateNpcAttackRoll(npc).Returns(10);
        _calculator.CalculatePlayerDefenceRoll(player, npc).Returns(100);
        _calculator.DoesAttackHit(10, 100).Returns(false);

        _engine.ExecuteTurn(session, CombatStyle.Accurate);

        // Should have raised ExperienceGainedEvents for Attack and Hitpoints
        var xpEvents = _events.GetEvents<ExperienceGainedEvent>().ToList();
        Assert.True(xpEvents.Count >= 2); // Attack XP + Hitpoints XP
    }

    [Fact]
    public void ExecuteTurn_IncrementsTurnCounter()
    {
        var player = TestFactory.CreatePlayer(currentHealth: 50, maxHealth: 50);
        var npc = CreateNpc(hitpoints: 20);
        var session = new CombatSession(player, npc, _random);

        // Player misses
        _calculator.CalculatePlayerAttackRoll(player).Returns(10);
        _calculator.CalculateNpcDefenceRoll(npc).Returns(100);
        _calculator.DoesAttackHit(10, 100).Returns(false);

        // NPC misses
        _calculator.CalculateNpcAttackRoll(npc).Returns(10);
        _calculator.CalculatePlayerDefenceRoll(player, npc).Returns(100);
        _calculator.DoesAttackHit(10, 100).Returns(false);

        Assert.Equal(0, session.TurnCount);
        _engine.ExecuteTurn(session, CombatStyle.Accurate);
        Assert.Equal(1, session.TurnCount);
    }
}
