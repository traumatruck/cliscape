using CliScape.Core;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.World.Resources;

namespace CliScape.Game.Skills;

/// <summary>
///     Handles fishing logic.
/// </summary>
public interface IFishingService
{
    /// <summary>
    ///     Validates if the player can fish at the specified spot.
    /// </summary>
    ServiceResult CanFish(Player player, IFishingSpot spot);

    /// <summary>
    ///     Performs fishing at the specified spot.
    /// </summary>
    FishingResult Fish(Player player, IFishingSpot spot, int count, Func<ItemId, IItem?> itemResolver);
}