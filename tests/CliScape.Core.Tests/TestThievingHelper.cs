using CliScape.Core.Skills;
using CliScape.Core.World.Resources;
using NSubstitute;

namespace CliScape.Core.Tests;

public sealed class TestThievingHelper
{
    private static IThievingTarget MakeTarget(int requiredLevel = 1, double baseChance = 0.5)
    {
        var target = Substitute.For<IThievingTarget>();
        target.RequiredLevel.Returns(requiredLevel);
        target.BaseSuccessChance.Returns(baseChance);
        target.Name.Returns("Test Target");
        target.TargetType.Returns(ThievingTargetType.Npc);
        return target;
    }

    [Fact]
    public void CalculateSuccessChance_AtRequiredLevel_ReturnsBaseChance()
    {
        var target = MakeTarget(requiredLevel: 10, baseChance: 0.5);
        var chance = ThievingHelper.CalculateSuccessChance(target, 10);

        Assert.Equal(0.5, chance, 0.001);
    }

    [Fact]
    public void CalculateSuccessChance_EachLevelAbove_Adds1Percent()
    {
        var target = MakeTarget(requiredLevel: 10, baseChance: 0.5);

        var chanceAt10 = ThievingHelper.CalculateSuccessChance(target, 10);
        var chanceAt20 = ThievingHelper.CalculateSuccessChance(target, 20);

        // 10 levels above → +10% (+0.10)
        Assert.Equal(chanceAt10 + 0.10, chanceAt20, 0.001);
    }

    [Fact]
    public void CalculateSuccessChance_CapsAt95Percent()
    {
        var target = MakeTarget(requiredLevel: 1, baseChance: 0.8);
        var chance = ThievingHelper.CalculateSuccessChance(target, 99);

        Assert.Equal(0.95, chance, 0.001);
    }

    [Fact]
    public void CalculateSuccessChance_BelowRequiredLevel_StillUsesFormula()
    {
        var target = MakeTarget(requiredLevel: 50, baseChance: 0.5);
        // 10 levels BELOW: bonus = (10 - 50) * 0.01 = -0.40
        var chance = ThievingHelper.CalculateSuccessChance(target, 10);

        Assert.Equal(0.10, chance, 0.001);
    }

    [Fact]
    public void CalculateSuccessChance_VeryLowLevel_ClampsToZero()
    {
        var target = MakeTarget(requiredLevel: 99, baseChance: 0.3);
        // Way below required: bonus = (1 - 99) * 0.01 = -0.98
        var chance = ThievingHelper.CalculateSuccessChance(target, 1);

        Assert.Equal(0.0, chance, 0.001);
    }

    [Fact]
    public void CalculateSuccessChance_IncreasesMonotonically()
    {
        var target = MakeTarget(requiredLevel: 10, baseChance: 0.5);

        var prev = 0.0;
        for (var level = 10; level <= 60; level++)
        {
            var chance = ThievingHelper.CalculateSuccessChance(target, level);
            Assert.True(chance >= prev,
                $"Chance at level {level} ({chance}) should be >= chance at level {level - 1} ({prev})");
            prev = chance;
        }
    }
}
