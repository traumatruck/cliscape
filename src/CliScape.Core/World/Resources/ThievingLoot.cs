using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents possible loot from a thieving target.
/// </summary>
/// <param name="ItemId">The item ID of the loot.</param>
/// <param name="MinQuantity">The minimum quantity of items.</param>
/// <param name="MaxQuantity">The maximum quantity of items.</param>
/// <param name="Weight">The relative weight of this loot (higher = more common).</param>
public readonly record struct ThievingLoot(ItemId ItemId, int MinQuantity, int MaxQuantity, int Weight);
