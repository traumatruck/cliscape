using CliScape.Core.Items;

namespace CliScape.Core.Skills;

/// <summary>
///     Represents a smithing recipe for creating equipment.
/// </summary>
/// <param name="ResultItemId">The item produced.</param>
/// <param name="BarId">The bar type required.</param>
/// <param name="BarCount">How many bars are needed.</param>
/// <param name="RequiredLevel">Smithing level required.</param>
/// <param name="Experience">XP granted.</param>
public readonly record struct SmithingRecipe(
    ItemId ResultItemId,
    ItemId BarId,
    int BarCount,
    int RequiredLevel,
    int Experience);