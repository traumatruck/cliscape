using CliScape.Core.Combat;
using CliScape.Core.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Players.Skills;
using CliScape.Tests.Shared;
using NSubstitute;

namespace CliScape.Core.Tests;

public sealed class TestCombatCalculator
{
    private readonly StubRandomProvider _random = new();
    private readonly CombatCalculator _calculator;

    public TestCombatCalculator()
    {
        _calculator = new CombatCalculator(_random);
    }

    [Fact]
    public void CalculatePlayerMaxHit_Level1_NoBonus_ReturnsAtLeast1()
    {
        var player = TestFactory.CreatePlayer();
        var maxHit = _calculator.CalculatePlayerMaxHit(player, 0);
        Assert.True(maxHit >= 1);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(10, 0)]
    [InlineData(50, 0)]
    [InlineData(99, 0)]
    [InlineData(1, 10)]
    [InlineData(50, 50)]
    public void CalculatePlayerMaxHit_IncreasesWithStrengthLevel(int strengthLevel, int strengthBonus)
    {
        var player = TestFactory.CreatePlayerWithSkill(SkillConstants.StrengthSkillName, strengthLevel);
        var maxHit = _calculator.CalculatePlayerMaxHit(player, strengthBonus);
        Assert.True(maxHit >= 1);
    }

    [Fact]
    public void CalculatePlayerMaxHit_HigherStrength_HigherDamage()
    {
        var lowStr = TestFactory.CreatePlayerWithSkill(SkillConstants.StrengthSkillName, 10);
        var highStr = TestFactory.CreatePlayerWithSkill(SkillConstants.StrengthSkillName, 99);

        var lowMaxHit = _calculator.CalculatePlayerMaxHit(lowStr, 0);
        var highMaxHit = _calculator.CalculatePlayerMaxHit(highStr, 0);

        Assert.True(highMaxHit > lowMaxHit);
    }

    [Fact]
    public void CalculatePlayerMaxHit_StrengthBonusIncreasesMaxHit()
    {
        var player = TestFactory.CreatePlayerWithSkill(SkillConstants.StrengthSkillName, 50);

        var noBonus = _calculator.CalculatePlayerMaxHit(player, 0);
        var withBonus = _calculator.CalculatePlayerMaxHit(player, 50);

        Assert.True(withBonus > noBonus);
    }

    [Fact]
    public void CalculatePlayerAttackRoll_Level1_NoBonus()
    {
        var player = TestFactory.CreatePlayer();
        // effectiveAttack = 1 + 9 = 10; result = 10 * (0 + 64) = 640
        var roll = _calculator.CalculatePlayerAttackRoll(player, 0);
        Assert.Equal(640, roll);
    }

    [Fact]
    public void CalculatePlayerAttackRoll_HigherAttack_HigherRoll()
    {
        var lowAtk = TestFactory.CreatePlayerWithSkill(SkillConstants.AttackSkillName, 10);
        var highAtk = TestFactory.CreatePlayerWithSkill(SkillConstants.AttackSkillName, 99);

        var lowRoll = _calculator.CalculatePlayerAttackRoll(lowAtk, 0);
        var highRoll = _calculator.CalculatePlayerAttackRoll(highAtk, 0);

        Assert.True(highRoll > lowRoll);
    }

    [Fact]
    public void CalculatePlayerAttackRoll_BonusIncreasesRoll()
    {
        var player = TestFactory.CreatePlayerWithSkill(SkillConstants.AttackSkillName, 50);

        var noBonus = _calculator.CalculatePlayerAttackRoll(player, 0);
        var withBonus = _calculator.CalculatePlayerAttackRoll(player, 50);

        Assert.True(withBonus > noBonus);
    }

    [Fact]
    public void CalculateNpcDefenceRoll_UsesDefenceLevelAndCrushBonus()
    {
        var npc = Substitute.For<ICombatableNpc>();
        npc.DefenceLevel.Returns(10);
        npc.CrushDefenceBonus.Returns(5);

        // (10 + 9) * (5 + 64) = 19 * 69 = 1311
        var roll = _calculator.CalculateNpcDefenceRoll(npc);
        Assert.Equal(1311, roll);
    }

    [Fact]
    public void CalculateNpcAttackRoll_UsesAttackStyleBonus()
    {
        var npc = Substitute.For<ICombatableNpc>();
        npc.AttackLevel.Returns(5);
        npc.SlashAttackBonus.Returns(10);
        npc.Attacks.Returns([new NpcAttack(NpcAttackStyle.Slash, 4)]);

        // (5 + 9) * (10 + 64) = 14 * 74 = 1036
        var roll = _calculator.CalculateNpcAttackRoll(npc);
        Assert.Equal(1036, roll);
    }

    [Fact]
    public void CalculatePlayerDefenceRoll_Level1_NoBonus()
    {
        var player = TestFactory.CreatePlayer();
        // effectiveDefence = 1 + 9 = 10; result = 10 * (0 + 64) = 640
        var roll = _calculator.CalculatePlayerDefenceRoll(player, 0);
        Assert.Equal(640, roll);
    }

    [Fact]
    public void DoesAttackHit_HighAttackRoll_Hits()
    {
        // attackRoll > defenceRoll → high hitChance
        // random.NextDouble() returns 0.0 (from default 0 / 100.0)
        _random.WithDefault(0);
        var result = _calculator.DoesAttackHit(1000, 100);
        Assert.True(result);
    }

    [Fact]
    public void DoesAttackHit_LowAttackRoll_HighRandom_Misses()
    {
        // attackRoll < defenceRoll → low hitChance
        // random.NextDouble() returns 0.99 (from 99 / 100.0)
        _random.Returns(99);
        var result = _calculator.DoesAttackHit(100, 1000);
        Assert.False(result);
    }

    [Fact]
    public void RollDamage_ReturnsValueInRange()
    {
        _random.Returns(5);
        var damage = _calculator.RollDamage(10);
        Assert.InRange(damage, 0, 10);
    }

    [Fact]
    public void RollDamage_MaxHitZero_ReturnsZero()
    {
        var damage = _calculator.RollDamage(0);
        Assert.Equal(0, damage);
    }

    [Fact]
    public void CalculateExperience_Accurate_GrantsAttackXp()
    {
        var xp = _calculator.CalculateExperience(10, CombatStyle.Accurate);

        Assert.Equal(40, xp.AttackXp); // 10 * 4
        Assert.Equal(0, xp.StrengthXp);
        Assert.Equal(0, xp.DefenceXp);
        Assert.Equal(13, xp.HitpointsXp); // (int)(10 * 1.33)
    }

    [Fact]
    public void CalculateExperience_Aggressive_GrantsStrengthXp()
    {
        var xp = _calculator.CalculateExperience(10, CombatStyle.Aggressive);

        Assert.Equal(0, xp.AttackXp);
        Assert.Equal(40, xp.StrengthXp);
        Assert.Equal(0, xp.DefenceXp);
        Assert.Equal(13, xp.HitpointsXp);
    }

    [Fact]
    public void CalculateExperience_Defensive_GrantsDefenceXp()
    {
        var xp = _calculator.CalculateExperience(10, CombatStyle.Defensive);

        Assert.Equal(0, xp.AttackXp);
        Assert.Equal(0, xp.StrengthXp);
        Assert.Equal(40, xp.DefenceXp);
        Assert.Equal(13, xp.HitpointsXp);
    }

    [Fact]
    public void CalculateExperience_Controlled_SplitsXp()
    {
        var xp = _calculator.CalculateExperience(10, CombatStyle.Controlled);

        // 40 / 3 = 13 per skill
        Assert.Equal(13, xp.AttackXp);
        Assert.Equal(13, xp.StrengthXp);
        Assert.Equal(13, xp.DefenceXp);
        Assert.Equal(13, xp.HitpointsXp);
    }

    [Fact]
    public void CalculateExperience_ZeroDamage_ReturnsZeroXp()
    {
        var xp = _calculator.CalculateExperience(0, CombatStyle.Accurate);

        Assert.Equal(0, xp.AttackXp);
        Assert.Equal(0, xp.HitpointsXp);
        Assert.Equal(0, xp.Total);
    }

    [Fact]
    public void CalculateExperience_Total_SumsAllSkills()
    {
        var xp = _calculator.CalculateExperience(10, CombatStyle.Accurate);
        Assert.Equal(xp.AttackXp + xp.StrengthXp + xp.DefenceXp + xp.HitpointsXp, xp.Total);
    }
}
