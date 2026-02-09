using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents a fish that can be caught at a fishing spot.
/// </summary>
/// <param name="FishItemId">The item ID of the raw fish.</param>
/// <param name="RequiredLevel">The fishing level required to catch this fish.</param>
/// <param name="Experience">The fishing experience gained when catching this fish.</param>
public readonly record struct FishingYield(ItemId FishItemId, int RequiredLevel, int Experience);