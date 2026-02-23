using CliScape.Core.Items;
using CliScape.Core.Skills;

namespace CliScape.Core.Tests;

public sealed class TestCookingHelper
{
    // Shrimp recipe: RequiredLevel=1, Experience=30, StopBurnLevel=34
    private static readonly CookingRecipe ShrimpRecipe = new(
        new ItemId(1300), new ItemId(1002), new ItemId(1800), 1, 30, 34);

    // Lobster recipe: RequiredLevel=40, Experience=120, StopBurnLevel=74
    private static readonly CookingRecipe LobsterRecipe = new(
        new ItemId(1306), new ItemId(1005), new ItemId(1806), 40, 120, 74);

    [Fact]
    public void CalculateBurnChance_AtStopBurnLevel_ReturnsZero()
    {
        var chance = CookingHelper.CalculateBurnChance(ShrimpRecipe, 34, 0);
        Assert.Equal(0.0, chance);
    }

    [Fact]
    public void CalculateBurnChance_AboveStopBurnLevel_ReturnsZero()
    {
        var chance = CookingHelper.CalculateBurnChance(ShrimpRecipe, 99, 0);
        Assert.Equal(0.0, chance);
    }

    [Fact]
    public void CalculateBurnChance_AtRequiredLevel_Returns60Percent()
    {
        var chance = CookingHelper.CalculateBurnChance(ShrimpRecipe, 1, 0);
        Assert.Equal(0.6, chance, 0.01);
    }

    [Fact]
    public void CalculateBurnChance_BetweenLevels_LinearInterpolation()
    {
        // Shrimp: range = 34 - 1 = 33 levels; at level 17 (midway ~16/33)
        var chance = CookingHelper.CalculateBurnChance(ShrimpRecipe, 17, 0);
        Assert.True(chance > 0.0);
        Assert.True(chance < 0.6);
    }

    [Fact]
    public void CalculateBurnChance_DecreasesWithHigherLevel()
    {
        var chanceLow = CookingHelper.CalculateBurnChance(LobsterRecipe, 40, 0);
        var chanceMid = CookingHelper.CalculateBurnChance(LobsterRecipe, 57, 0);
        var chanceHigh = CookingHelper.CalculateBurnChance(LobsterRecipe, 73, 0);

        Assert.True(chanceLow > chanceMid);
        Assert.True(chanceMid > chanceHigh);
        Assert.True(chanceHigh > 0.0);
    }

    [Fact]
    public void CalculateBurnChance_RangeReduction_LowersChance()
    {
        var noReduction = CookingHelper.CalculateBurnChance(ShrimpRecipe, 10, 0);
        var withReduction = CookingHelper.CalculateBurnChance(ShrimpRecipe, 10, 50);

        Assert.True(withReduction < noReduction);
    }

    [Fact]
    public void CalculateBurnChance_FullReduction_ReturnsZero()
    {
        var chance = CookingHelper.CalculateBurnChance(ShrimpRecipe, 10, 100);
        Assert.Equal(0.0, chance, 0.001);
    }

    [Fact]
    public void CalculateBurnChance_ClampedBetweenZeroAndOne()
    {
        var chance = CookingHelper.CalculateBurnChance(ShrimpRecipe, 1, 0);
        Assert.InRange(chance, 0.0, 1.0);
    }

    [Fact]
    public void FindRecipe_KnownRawItemId_ReturnsRecipe()
    {
        var recipe = CookingHelper.FindRecipe(new ItemId(1300)); // Raw shrimps
        Assert.NotNull(recipe);
        Assert.Equal(1, recipe.Value.RequiredLevel);
    }

    [Fact]
    public void FindRecipe_UnknownItemId_ReturnsNull()
    {
        var recipe = CookingHelper.FindRecipe(new ItemId(9999));
        Assert.Null(recipe);
    }

    [Fact]
    public void FindRecipeByName_MatchesPartialName()
    {
        var recipe = CookingHelper.FindRecipeByName("shrimp");
        Assert.NotNull(recipe);
    }

    [Fact]
    public void FindRecipeByName_CaseInsensitive()
    {
        var recipe = CookingHelper.FindRecipeByName("LOBSTER");
        Assert.NotNull(recipe);
    }

    [Fact]
    public void FindRecipeByName_NoMatch_ReturnsNull()
    {
        var recipe = CookingHelper.FindRecipeByName("pizza");
        Assert.Null(recipe);
    }

    [Fact]
    public void Recipes_AllHaveValidStopBurnLevel()
    {
        foreach (var recipe in CookingHelper.Recipes)
        {
            Assert.True(recipe.StopBurnLevel > recipe.RequiredLevel,
                $"Stop burn level ({recipe.StopBurnLevel}) must be > required level ({recipe.RequiredLevel})");
        }
    }
}
