using CliScape.Core.Players;

namespace CliScape.Core.Items;

/// <summary>
///     Interface for items that can be buried for prayer experience.
/// </summary>
public interface IBuryable : IActionableItem
{
    /// <summary>
    ///     The amount of prayer experience gained when burying this item.
    /// </summary>
    int PrayerExperience { get; }

    /// <summary>
    ///     Buries the item, granting prayer experience to the player.
    /// </summary>
    /// <param name="player">The player burying the item.</param>
    /// <returns>A message describing the result of burying.</returns>
    string Bury(Player player);
}
