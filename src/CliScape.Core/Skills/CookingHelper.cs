using CliScape.Core.Items;

namespace CliScape.Core.Skills;

/// <summary>
///     Handles Cooking skill operations.
/// </summary>
public static class CookingHelper
{
    /// <summary>
    ///     All cooking recipes.
    /// </summary>
    public static readonly CookingRecipe[] Recipes =
    [
        new(ItemIds.RawShrimps, ItemIds.Shrimp, ItemIds.BurntShrimps, 1, 30, 34),
        new(ItemIds.RawAnchovies, ItemIds.Anchovies, ItemIds.BurntAnchovies, 1, 30, 34),
        new(ItemIds.RawChicken, ItemIds.CookedChicken, ItemIds.BurntChicken, 1, 30, 34),
        new(ItemIds.RawBeef, ItemIds.CookedMeat, ItemIds.BurntMeat, 1, 30, 34),
        new(ItemIds.RawSardine, ItemIds.Sardine, ItemIds.BurntSardine, 1, 40, 38),
        new(ItemIds.RawHerring, ItemIds.Herring, ItemIds.BurntHerring, 5, 50, 41),
        new(ItemIds.RawTrout, ItemIds.Trout, ItemIds.BurntTrout, 15, 70, 50),
        new(ItemIds.RawSalmon, ItemIds.Salmon, ItemIds.BurntSalmon, 25, 90, 58),
        new(ItemIds.RawTuna, ItemIds.Tuna, ItemIds.BurntTuna, 30, 100, 65),
        new(ItemIds.RawLobster, ItemIds.Lobster, ItemIds.BurntLobster, 40, 120, 74),
        new(ItemIds.RawSwordfish, ItemIds.Swordfish, ItemIds.BurntSwordfish, 45, 140, 86)
    ];

    private static readonly Random Random = new();

    /// <summary>
    ///     Finds a cooking recipe for the given raw item.
    /// </summary>
    public static CookingRecipe? FindRecipe(ItemId rawItemId)
    {
        foreach (var recipe in Recipes)
        {
            if (recipe.RawItemId == rawItemId)
            {
                return recipe;
            }
        }

        return null;
    }

    /// <summary>
    ///     Finds a cooking recipe by raw food name.
    /// </summary>
    public static CookingRecipe? FindRecipeByName(string name)
    {
        var normalized = name.ToLowerInvariant().Trim();

        foreach (var recipe in Recipes)
        {
            var rawName = GetRawFoodName(recipe.RawItemId).ToLowerInvariant();
            if (rawName.Contains(normalized) || normalized.Contains(rawName.Replace("raw ", "")))
            {
                return recipe;
            }
        }

        return null;
    }

    /// <summary>
    ///     Calculates the burn chance for a given recipe and player level.
    /// </summary>
    /// <param name="recipe">The cooking recipe.</param>
    /// <param name="cookingLevel">The player's cooking level.</param>
    /// <param name="burnChanceReduction">The reduction from using a range (0-100%).</param>
    /// <returns>The chance to burn as a value between 0.0 and 1.0.</returns>
    public static double CalculateBurnChance(CookingRecipe recipe, int cookingLevel, int burnChanceReduction)
    {
        // At required level, ~60% burn chance
        // At stop burn level, 0% burn chance
        // Linear interpolation between

        if (cookingLevel >= recipe.StopBurnLevel)
        {
            return 0.0;
        }

        var levelRange = recipe.StopBurnLevel - recipe.RequiredLevel;
        var playerProgress = cookingLevel - recipe.RequiredLevel;

        if (levelRange <= 0 || playerProgress < 0)
        {
            return 0.6; // Base 60% burn chance at minimum level
        }

        var baseBurnChance = 0.6 * (1.0 - (double)playerProgress / levelRange);

        // Apply range reduction
        var reduction = burnChanceReduction / 100.0;
        var finalChance = baseBurnChance * (1.0 - reduction);

        return Math.Max(0.0, Math.Min(1.0, finalChance));
    }

    /// <summary>
    ///     Determines if food burns based on the burn chance.
    /// </summary>
    public static bool DoesBurn(double burnChance)
    {
        return Random.NextDouble() < burnChance;
    }

    /// <summary>
    ///     Gets the name of a raw food item.
    /// </summary>
    public static string GetRawFoodName(ItemId id)
    {
        if (id == ItemIds.RawShrimps)
        {
            return "Raw shrimps";
        }

        if (id == ItemIds.RawAnchovies)
        {
            return "Raw anchovies";
        }

        if (id == ItemIds.RawSardine)
        {
            return "Raw sardine";
        }

        if (id == ItemIds.RawHerring)
        {
            return "Raw herring";
        }

        if (id == ItemIds.RawTrout)
        {
            return "Raw trout";
        }

        if (id == ItemIds.RawSalmon)
        {
            return "Raw salmon";
        }

        if (id == ItemIds.RawLobster)
        {
            return "Raw lobster";
        }

        if (id == ItemIds.RawTuna)
        {
            return "Raw tuna";
        }

        if (id == ItemIds.RawSwordfish)
        {
            return "Raw swordfish";
        }

        if (id == ItemIds.RawChicken)
        {
            return "Raw chicken";
        }

        if (id == ItemIds.RawBeef)
        {
            return "Raw beef";
        }

        return "Unknown food";
    }

    /// <summary>
    ///     Gets the name of a cooked food item.
    /// </summary>
    public static string GetCookedFoodName(ItemId id)
    {
        if (id == ItemIds.Shrimp)
        {
            return "Shrimps";
        }

        if (id == ItemIds.Anchovies)
        {
            return "Anchovies";
        }

        if (id == ItemIds.Sardine)
        {
            return "Sardine";
        }

        if (id == ItemIds.Herring)
        {
            return "Herring";
        }

        if (id == ItemIds.Trout)
        {
            return "Trout";
        }

        if (id == ItemIds.Salmon)
        {
            return "Salmon";
        }

        if (id == ItemIds.Lobster)
        {
            return "Lobster";
        }

        if (id == ItemIds.Tuna)
        {
            return "Tuna";
        }

        if (id == ItemIds.Swordfish)
        {
            return "Swordfish";
        }

        if (id == ItemIds.CookedChicken)
        {
            return "Cooked chicken";
        }

        if (id == ItemIds.CookedMeat)
        {
            return "Cooked meat";
        }

        return "Unknown food";
    }

    /// <summary>
    ///     Checks if an item is raw cookable food.
    /// </summary>
    public static bool IsRawFood(ItemId id)
    {
        return FindRecipe(id) is not null;
    }

    /// <summary>
    ///     Well-known item IDs for cooking.
    /// </summary>
    private static class ItemIds
    {
        // Raw food
        public static readonly ItemId RawShrimps = new(1300);
        public static readonly ItemId RawAnchovies = new(1301);
        public static readonly ItemId RawSardine = new(1302);
        public static readonly ItemId RawHerring = new(1303);
        public static readonly ItemId RawTrout = new(1304);
        public static readonly ItemId RawSalmon = new(1305);
        public static readonly ItemId RawLobster = new(1306);
        public static readonly ItemId RawTuna = new(1307);
        public static readonly ItemId RawSwordfish = new(1308);
        public static readonly ItemId RawChicken = new(1104);
        public static readonly ItemId RawBeef = new(1101);

        // Cooked food
        public static readonly ItemId Shrimp = new(1002);
        public static readonly ItemId Anchovies = new(1852);
        public static readonly ItemId Sardine = new(1853);
        public static readonly ItemId Herring = new(1854);
        public static readonly ItemId Trout = new(1003);
        public static readonly ItemId Salmon = new(1004);
        public static readonly ItemId Lobster = new(1005);
        public static readonly ItemId Tuna = new(1855);
        public static readonly ItemId Swordfish = new(1856);
        public static readonly ItemId CookedChicken = new(1850);
        public static readonly ItemId CookedMeat = new(1851);

        // Burnt food
        public static readonly ItemId BurntShrimps = new(1800);
        public static readonly ItemId BurntAnchovies = new(1801);
        public static readonly ItemId BurntSardine = new(1802);
        public static readonly ItemId BurntHerring = new(1803);
        public static readonly ItemId BurntTrout = new(1804);
        public static readonly ItemId BurntSalmon = new(1805);
        public static readonly ItemId BurntLobster = new(1806);
        public static readonly ItemId BurntTuna = new(1807);
        public static readonly ItemId BurntSwordfish = new(1808);
        public static readonly ItemId BurntChicken = new(1809);
        public static readonly ItemId BurntMeat = new(1810);
    }
}