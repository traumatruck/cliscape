using CliScape.Core.Players.Skills;

namespace CliScape.Core.Tests;

public sealed class TestPlayerSkillLevel
{
    [Fact]
    public void CreateNew_Level1_ZeroXp()
    {
        var skill = PlayerSkillLevel.CreateNew();
        Assert.Equal(1, skill.Value);
        Assert.Equal(0, skill.Experience);
    }

    [Fact]
    public void FromLevel_Level1_ZeroXp()
    {
        var skill = PlayerSkillLevel.FromLevel(1);
        Assert.Equal(1, skill.Value);
        Assert.Equal(0, skill.Experience);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(75)]
    [InlineData(99)]
    public void FromLevel_RoundTrip_ProducesCorrectLevel(int level)
    {
        var skill = PlayerSkillLevel.FromLevel(level);
        Assert.Equal(level, skill.Value);
    }

    [Fact]
    public void FromLevel_ClampsToMin()
    {
        var skill = PlayerSkillLevel.FromLevel(0);
        Assert.Equal(1, skill.Value);
    }

    [Fact]
    public void FromLevel_ClampsToMax()
    {
        var skill = PlayerSkillLevel.FromLevel(200);
        Assert.Equal(99, skill.Value);
    }

    [Fact]
    public void FromExperience_ZeroXp_Level1()
    {
        var skill = PlayerSkillLevel.FromExperience(0);
        Assert.Equal(1, skill.Value);
        Assert.Equal(0, skill.Experience);
    }

    [Fact]
    public void FromExperience_NegativeXp_ClampsToZero()
    {
        var skill = PlayerSkillLevel.FromExperience(-100);
        Assert.Equal(1, skill.Value);
        Assert.Equal(0, skill.Experience);
    }

    [Fact]
    public void FromExperience_MaxXp_Level99()
    {
        var skill = PlayerSkillLevel.FromExperience(200_000_000);
        Assert.Equal(99, skill.Value);
        Assert.Equal(200_000_000, skill.Experience);
    }

    [Fact]
    public void FromExperience_OverMaxXp_ClampsTo200M()
    {
        var skill = PlayerSkillLevel.FromExperience(300_000_000);
        Assert.Equal(99, skill.Value);
        Assert.Equal(200_000_000, skill.Experience);
    }

    [Fact]
    public void FromExperience_KnownLevel2Threshold()
    {
        // Level 2 requires 83 XP in OSRS
        var skill = PlayerSkillLevel.FromExperience(83);
        Assert.Equal(2, skill.Value);
    }

    [Fact]
    public void FromExperience_JustBelowLevel2()
    {
        var skill = PlayerSkillLevel.FromExperience(82);
        Assert.Equal(1, skill.Value);
    }

    [Fact]
    public void AddExperience_IncreasesXp()
    {
        var skill = PlayerSkillLevel.CreateNew();
        var updated = skill.AddExperience(100);

        Assert.Equal(100, updated.Experience);
        Assert.True(updated.Value >= skill.Value);
    }

    [Fact]
    public void AddExperience_CrossesLevelBoundary()
    {
        var skill = PlayerSkillLevel.CreateNew(); // level 1, 0 xp
        var updated = skill.AddExperience(83); // should be enough for level 2

        Assert.Equal(2, updated.Value);
    }

    [Fact]
    public void AddExperience_CapsAt200M()
    {
        var skill = PlayerSkillLevel.FromExperience(199_999_999);
        var updated = skill.AddExperience(100);

        Assert.Equal(200_000_000, updated.Experience);
    }

    [Fact]
    public void ExperienceToNextLevel_Level1_Returns83()
    {
        var skill = PlayerSkillLevel.CreateNew();
        // Level 2 XP - Level 1 XP (0) = 83
        Assert.Equal(83, skill.ExperienceToNextLevel);
    }

    [Fact]
    public void ExperienceToNextLevel_Level99_ReturnsZero()
    {
        var skill = PlayerSkillLevel.FromLevel(99);
        Assert.Equal(0, skill.ExperienceToNextLevel);
    }

    [Fact]
    public void ExperienceToNextLevel_DecreasesWithXpGained()
    {
        var skill = PlayerSkillLevel.CreateNew();
        var initial = skill.ExperienceToNextLevel;
        var updated = skill.AddExperience(50);

        Assert.True(updated.ExperienceToNextLevel < initial);
    }

    [Fact]
    public void ProgressToNextLevel_Level1_ZeroXp_IsZero()
    {
        var skill = PlayerSkillLevel.CreateNew();
        Assert.Equal(0.0, skill.ProgressToNextLevel, 0.01);
    }

    [Fact]
    public void ProgressToNextLevel_Level99_Is100()
    {
        var skill = PlayerSkillLevel.FromLevel(99);
        Assert.Equal(100.0, skill.ProgressToNextLevel, 0.01);
    }

    [Fact]
    public void ProgressToNextLevel_Midway_IsApproximately50()
    {
        // Level 1 needs 83 XP for level 2; midway ~41-42
        var skill = PlayerSkillLevel.FromExperience(41);
        var progress = skill.ProgressToNextLevel;
        Assert.InRange(progress, 40.0, 60.0);
    }

    [Fact]
    public void ExperienceInCurrentLevel_Level1_ZeroXp_IsZero()
    {
        var skill = PlayerSkillLevel.CreateNew();
        Assert.Equal(0, skill.ExperienceInCurrentLevel);
    }

    [Fact]
    public void ExperienceForCurrentLevel_Level1_IsZero()
    {
        var skill = PlayerSkillLevel.CreateNew();
        Assert.Equal(0, skill.ExperienceForCurrentLevel);
    }

    [Fact]
    public void ExperienceForNextLevel_Level1_Is83()
    {
        var skill = PlayerSkillLevel.CreateNew();
        Assert.Equal(83, skill.ExperienceForNextLevel);
    }

    [Fact]
    public void ToString_IncludesLevelAndXp()
    {
        var skill = PlayerSkillLevel.FromLevel(50);
        var str = skill.ToString();
        Assert.Contains("Level 50", str);
        Assert.Contains("XP", str);
    }

    [Fact]
    public void FromLevel_Monotonic_HigherLevelsNeedMoreXp()
    {
        var prevXp = 0;
        for (var level = 2; level <= 99; level++)
        {
            var skill = PlayerSkillLevel.FromLevel(level);
            Assert.True(skill.Experience > prevXp,
                $"Level {level} XP ({skill.Experience}) should be > level {level - 1} XP ({prevXp})");
            prevXp = skill.Experience;
        }
    }
}
