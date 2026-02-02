using CliScape.Core.Items;

namespace CliScape.Core.Skills;

/// <summary>
///     Represents a cooking recipe.
/// </summary>
/// <param name="RawItemId">The raw food item ID.</param>
/// <param name="CookedItemId">The cooked food item ID.</param>
/// <param name="BurntItemId">The burnt food item ID.</param>
/// <param name="RequiredLevel">The cooking level required.</param>
/// <param name="Experience">The XP granted on success.</param>
/// <param name="StopBurnLevel">The level at which burning stops (for ranges).</param>
public readonly record struct CookingRecipe(
    ItemId RawItemId,
    ItemId CookedItemId,
    ItemId BurntItemId,
    int RequiredLevel,
    int Experience,
    int StopBurnLevel);