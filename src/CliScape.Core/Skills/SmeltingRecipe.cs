using CliScape.Core.Items;

namespace CliScape.Core.Skills;

/// <summary>
///     Represents a smelting recipe.
/// </summary>
/// <param name="ResultBarId">The bar produced.</param>
/// <param name="PrimaryOreId">The main ore required.</param>
/// <param name="SecondaryOreId">Secondary ore if any (coal for steel/mithril/adamant/rune).</param>
/// <param name="SecondaryOreCount">How many of the secondary ore is needed.</param>
/// <param name="RequiredLevel">Smithing level required.</param>
/// <param name="Experience">XP granted.</param>
public readonly record struct SmeltingRecipe(
    ItemId ResultBarId,
    ItemId PrimaryOreId,
    ItemId? SecondaryOreId,
    int SecondaryOreCount,
    int RequiredLevel,
    int Experience);