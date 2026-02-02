using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Core.Skills;

/// <summary>
///     Handles Smithing skill operations for smelting ores into bars.
/// </summary>
public static class SmithingHelper
{
    /// <summary>
    ///     All smelting recipes.
    /// </summary>
    public static readonly SmeltingRecipe[] SmeltingRecipes =
    [
        new(ItemIds.BronzeBar, ItemIds.CopperOre, ItemIds.TinOre, 1, 1, 6),
        new(ItemIds.IronBar, ItemIds.IronOre, null, 0, 15, 13), // 50% success rate not implemented
        new(ItemIds.SteelBar, ItemIds.IronOre, ItemIds.Coal, 2, 30, 18),
        new(ItemIds.MithrilBar, ItemIds.MithrilOre, ItemIds.Coal, 4, 50, 30),
        new(ItemIds.AdamantiteBar, ItemIds.AdamantiteOre, ItemIds.Coal, 6, 70, 38),
        new(ItemIds.RuniteBar, ItemIds.RuniteOre, ItemIds.Coal, 8, 85, 50)
    ];

    /// <summary>
    ///     Bronze smithing recipes.
    /// </summary>
    public static readonly SmithingRecipe[] BronzeRecipes =
    [
        new(new ItemId(100), ItemIds.BronzeBar, 1, 1, 13), // Bronze dagger
        new(new ItemId(104), ItemIds.BronzeBar, 1, 2, 13), // Bronze mace
        new(new ItemId(111), ItemIds.BronzeBar, 1, 3, 13), // Bronze med helm
        new(new ItemId(101), ItemIds.BronzeBar, 1, 4, 13), // Bronze sword
        new(new ItemId(102), ItemIds.BronzeBar, 2, 5, 25), // Bronze scimitar
        new(new ItemId(110), ItemIds.BronzeBar, 2, 7, 25), // Bronze full helm
        new(new ItemId(141), ItemIds.BronzeBar, 2, 8, 25), // Bronze sq shield
        new(new ItemId(121), ItemIds.BronzeBar, 3, 11, 38), // Bronze chainbody
        new(new ItemId(140), ItemIds.BronzeBar, 3, 12, 38), // Bronze kiteshield
        new(new ItemId(130), ItemIds.BronzeBar, 3, 16, 38), // Bronze platelegs
        new(new ItemId(120), ItemIds.BronzeBar, 5, 18, 63) // Bronze platebody
    ];

    /// <summary>
    ///     Iron smithing recipes.
    /// </summary>
    public static readonly SmithingRecipe[] IronRecipes =
    [
        new(new ItemId(200), ItemIds.IronBar, 1, 15, 25), // Iron dagger
        new(new ItemId(204), ItemIds.IronBar, 1, 17, 25), // Iron mace
        new(new ItemId(211), ItemIds.IronBar, 1, 18, 25), // Iron med helm
        new(new ItemId(201), ItemIds.IronBar, 1, 19, 25), // Iron sword
        new(new ItemId(202), ItemIds.IronBar, 2, 20, 50), // Iron scimitar
        new(new ItemId(210), ItemIds.IronBar, 2, 22, 50), // Iron full helm
        new(new ItemId(241), ItemIds.IronBar, 2, 23, 50), // Iron sq shield
        new(new ItemId(221), ItemIds.IronBar, 3, 26, 75), // Iron chainbody
        new(new ItemId(240), ItemIds.IronBar, 3, 27, 75), // Iron kiteshield
        new(new ItemId(230), ItemIds.IronBar, 3, 31, 75), // Iron platelegs
        new(new ItemId(220), ItemIds.IronBar, 5, 33, 125) // Iron platebody
    ];

    /// <summary>
    ///     Steel smithing recipes.
    /// </summary>
    public static readonly SmithingRecipe[] SteelRecipes =
    [
        new(new ItemId(300), ItemIds.SteelBar, 1, 30, 38), // Steel dagger
        new(new ItemId(304), ItemIds.SteelBar, 1, 32, 38), // Steel mace
        new(new ItemId(311), ItemIds.SteelBar, 1, 33, 38), // Steel med helm
        new(new ItemId(301), ItemIds.SteelBar, 1, 34, 38), // Steel sword
        new(new ItemId(302), ItemIds.SteelBar, 2, 35, 75), // Steel scimitar
        new(new ItemId(310), ItemIds.SteelBar, 2, 37, 75), // Steel full helm
        new(new ItemId(340), ItemIds.SteelBar, 3, 42, 113), // Steel kiteshield
        new(new ItemId(321), ItemIds.SteelBar, 3, 41, 113), // Steel chainbody
        new(new ItemId(330), ItemIds.SteelBar, 3, 46, 113), // Steel platelegs
        new(new ItemId(320), ItemIds.SteelBar, 5, 48, 188) // Steel platebody
    ];

    /// <summary>
    ///     Gets all smithing recipes for a given bar type.
    /// </summary>
    public static SmithingRecipe[] GetRecipesForBar(ItemId barId)
    {
        if (barId == ItemIds.BronzeBar)
        {
            return BronzeRecipes;
        }

        if (barId == ItemIds.IronBar)
        {
            return IronRecipes;
        }

        if (barId == ItemIds.SteelBar)
        {
            return SteelRecipes;
        }

        return [];
    }

    /// <summary>
    ///     Finds a smelting recipe by the bar name.
    /// </summary>
    public static SmeltingRecipe? FindSmeltingRecipe(string barName)
    {
        var normalized = barName.ToLowerInvariant().Trim();

        foreach (var recipe in SmeltingRecipes)
        {
            var recipeName = GetBarName(recipe.ResultBarId).ToLowerInvariant();
            if (recipeName.Contains(normalized) || normalized.Contains(recipeName.Replace(" bar", "")))
            {
                return recipe;
            }
        }

        return null;
    }

    /// <summary>
    ///     Finds a smithing recipe by item name for a given bar type.
    /// </summary>
    public static SmithingRecipe? FindSmithingRecipe(ItemId barId, string itemName)
    {
        var recipes = GetRecipesForBar(barId);
        var normalized = itemName.ToLowerInvariant().Trim();

        foreach (var recipe in recipes)
        {
            var recipeName = GetItemName(recipe.ResultItemId).ToLowerInvariant();
            if (recipeName.Contains(normalized) || normalized.Contains(recipeName))
            {
                return recipe;
            }
        }

        return null;
    }

    /// <summary>
    ///     Gets bar ID from name.
    /// </summary>
    public static ItemId? GetBarIdFromName(string name)
    {
        var normalized = name.ToLowerInvariant().Trim();
        if (normalized.Contains("bronze"))
        {
            return ItemIds.BronzeBar;
        }

        if (normalized.Contains("iron") && !normalized.Contains("steel"))
        {
            return ItemIds.IronBar;
        }

        if (normalized.Contains("steel"))
        {
            return ItemIds.SteelBar;
        }

        if (normalized.Contains("mithril"))
        {
            return ItemIds.MithrilBar;
        }

        if (normalized.Contains("adamant"))
        {
            return ItemIds.AdamantiteBar;
        }

        if (normalized.Contains("rune") || normalized.Contains("runite"))
        {
            return ItemIds.RuniteBar;
        }

        return null;
    }

    private static string GetBarName(ItemId id)
    {
        if (id == ItemIds.BronzeBar)
        {
            return "Bronze bar";
        }

        if (id == ItemIds.IronBar)
        {
            return "Iron bar";
        }

        if (id == ItemIds.SteelBar)
        {
            return "Steel bar";
        }

        if (id == ItemIds.MithrilBar)
        {
            return "Mithril bar";
        }

        if (id == ItemIds.AdamantiteBar)
        {
            return "Adamantite bar";
        }

        if (id == ItemIds.RuniteBar)
        {
            return "Runite bar";
        }

        return "Unknown bar";
    }

    private static string GetItemName(ItemId id)
    {
        // This is a simplified lookup - in a real implementation, use ItemRegistry
        return id.Value switch
        {
            100 => "Bronze dagger",
            101 => "Bronze sword",
            102 => "Bronze scimitar",
            104 => "Bronze mace",
            110 => "Bronze full helm",
            111 => "Bronze med helm",
            120 => "Bronze platebody",
            121 => "Bronze chainbody",
            130 => "Bronze platelegs",
            140 => "Bronze kiteshield",
            141 => "Bronze sq shield",
            200 => "Iron dagger",
            201 => "Iron sword",
            202 => "Iron scimitar",
            204 => "Iron mace",
            210 => "Iron full helm",
            211 => "Iron med helm",
            220 => "Iron platebody",
            221 => "Iron chainbody",
            230 => "Iron platelegs",
            240 => "Iron kiteshield",
            241 => "Iron sq shield",
            300 => "Steel dagger",
            301 => "Steel sword",
            302 => "Steel scimitar",
            304 => "Steel mace",
            310 => "Steel full helm",
            311 => "Steel med helm",
            320 => "Steel platebody",
            321 => "Steel chainbody",
            330 => "Steel platelegs",
            340 => "Steel kiteshield",
            _ => "Unknown item"
        };
    }

    /// <summary>
    ///     Checks if player has a hammer.
    /// </summary>
    public static bool HasHammer(Player player)
    {
        return player.Inventory.GetItems().Any(slot => slot.Item.Id == ItemIds.Hammer);
    }

    /// <summary>
    ///     Gets the name of an ore by ID.
    /// </summary>
    public static string GetOreName(ItemId id)
    {
        if (id == ItemIds.CopperOre)
        {
            return "Copper ore";
        }

        if (id == ItemIds.TinOre)
        {
            return "Tin ore";
        }

        if (id == ItemIds.IronOre)
        {
            return "Iron ore";
        }

        if (id == ItemIds.Coal)
        {
            return "Coal";
        }

        if (id == ItemIds.MithrilOre)
        {
            return "Mithril ore";
        }

        if (id == ItemIds.AdamantiteOre)
        {
            return "Adamantite ore";
        }

        if (id == ItemIds.RuniteOre)
        {
            return "Runite ore";
        }

        return "Unknown ore";
    }

    /// <summary>
    ///     Well-known item IDs for smithing.
    /// </summary>
    private static class ItemIds
    {
        // Ores
        public static readonly ItemId CopperOre = new(1400);
        public static readonly ItemId TinOre = new(1401);
        public static readonly ItemId IronOre = new(1402);
        public static readonly ItemId Coal = new(1403);
        public static readonly ItemId MithrilOre = new(1404);
        public static readonly ItemId AdamantiteOre = new(1405);
        public static readonly ItemId RuniteOre = new(1406);

        // Bars
        public static readonly ItemId BronzeBar = new(1500);
        public static readonly ItemId IronBar = new(1501);
        public static readonly ItemId SteelBar = new(1502);
        public static readonly ItemId MithrilBar = new(1503);
        public static readonly ItemId AdamantiteBar = new(1504);
        public static readonly ItemId RuniteBar = new(1505);

        // Tools
        public static readonly ItemId Hammer = new(703);
    }
}