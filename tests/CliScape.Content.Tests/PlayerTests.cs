using CliScape.Content.Tests.Helpers;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Content.Tests;

public class PlayerTests
{
    [Fact]
    public void TakeDamage_LessThanHealth_ReducesCorrectly()
    {
        var player = TestFactory.CreatePlayer(10);

        player.TakeDamage(3);

        Assert.Equal(7, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_EqualToHealth_SetsToZero()
    {
        var player = TestFactory.CreatePlayer(10);

        player.TakeDamage(10);

        Assert.Equal(0, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_GreaterThanHealth_ClampsToZero()
    {
        var player = TestFactory.CreatePlayer(5);

        player.TakeDamage(20);

        Assert.Equal(0, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_Zero_NoChange()
    {
        var player = TestFactory.CreatePlayer(10);

        player.TakeDamage(0);

        Assert.Equal(10, player.CurrentHealth);
    }

    [Fact]
    public void Heal_RestoresHealth()
    {
        var player = TestFactory.CreatePlayer(5, 10);

        player.Heal(3);

        Assert.Equal(8, player.CurrentHealth);
    }

    [Fact]
    public void Heal_ClampsToMaximumHealth()
    {
        var player = TestFactory.CreatePlayer(8, 10);

        player.Heal(50);

        Assert.Equal(10, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_ThenHeal_RestoresCorrectly()
    {
        var player = TestFactory.CreatePlayer(10, 10);

        player.TakeDamage(7);
        Assert.Equal(3, player.CurrentHealth);

        player.Heal(4);
        Assert.Equal(7, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_MultipleTimes_AccumulatesCorrectly()
    {
        var player = TestFactory.CreatePlayer(10);

        player.TakeDamage(3);
        player.TakeDamage(3);
        player.TakeDamage(3);

        Assert.Equal(1, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_MultipleTimes_NeverGoesNegative()
    {
        var player = TestFactory.CreatePlayer(5);

        player.TakeDamage(3);
        player.TakeDamage(3);
        player.TakeDamage(3);

        Assert.Equal(0, player.CurrentHealth);
    }

    [Fact]
    public void GetSkill_BySkillName_ReturnsCorrectSkill()
    {
        var player = TestFactory.CreatePlayer();

        var skill = player.GetSkill(SkillConstants.AttackSkillName);

        Assert.Equal("Attack", skill.Name.Name);
    }

    [Fact]
    public void GetSkill_ByString_ReturnsCorrectSkill()
    {
        var player = TestFactory.CreatePlayer();

        var skill = player.GetSkill("Attack");

        Assert.Equal("Attack", skill.Name.Name);
    }

    [Fact]
    public void GetSkill_InvalidName_ThrowsInvalidOperationException()
    {
        var player = TestFactory.CreatePlayer();

        Assert.Throws<InvalidOperationException>(() => player.GetSkill("NonExistentSkill"));
    }

    [Fact]
    public void GetSkillLevel_ReturnsLevel()
    {
        var player = TestFactory.CreatePlayer();

        var level = player.GetSkillLevel(SkillConstants.AttackSkillName);

        Assert.Equal(1, level.Value);
    }

    [Fact]
    public void GetSkillLevel_Hitpoints_StartsAt10()
    {
        var player = TestFactory.CreatePlayer();

        var level = player.GetSkillLevel(SkillConstants.HitpointsSkillName);

        Assert.Equal(10, level.Value);
    }

    [Fact]
    public void AddExperience_IncreasesSkillExperience()
    {
        var player = TestFactory.CreatePlayer();
        var skill = player.GetSkill(SkillConstants.AttackSkillName);

        player.AddExperience(skill, 100);

        Assert.True(skill.Level.Experience >= 100);
    }

    [Fact]
    public void AddExperience_CanLevelUp()
    {
        var player = TestFactory.CreatePlayer();
        var skill = player.GetSkill(SkillConstants.AttackSkillName);

        // 83 XP is needed for level 2 in OSRS
        player.AddExperience(skill, 83);

        Assert.True(skill.Level.Value >= 2);
    }

    [Fact]
    public void Move_ChangesCurrentLocation()
    {
        var player = TestFactory.CreatePlayer();
        var newLocation = new TestFactory.StubLocation();

        player.Move(newLocation);

        Assert.Same(newLocation, player.CurrentLocation);
    }

    [Fact]
    public void SetPrayerPoints_ClampsToMaximum()
    {
        var player = TestFactory.CreatePlayer();
        // Prayer starts at level 1, so max prayer points = 1

        player.SetPrayerPoints(999);

        Assert.Equal(1, player.CurrentPrayerPoints);
    }

    [Fact]
    public void SetPrayerPoints_ClampsToZero()
    {
        var player = TestFactory.CreatePlayer();

        player.SetPrayerPoints(-10);

        Assert.Equal(0, player.CurrentPrayerPoints);
    }

    [Fact]
    public void Skills_ContainsAll23Skills()
    {
        var player = TestFactory.CreatePlayer();

        Assert.Equal(23, player.Skills.Length);
    }

    [Fact]
    public void TotalLevel_DefaultPlayer_Is32()
    {
        // 22 skills at level 1 + Hitpoints at level 10 = 32
        var player = TestFactory.CreatePlayer();

        Assert.Equal(32, player.TotalLevel);
    }
}