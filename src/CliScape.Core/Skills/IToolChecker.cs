using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Core.Skills;

/// <summary>
///     Checks if a player has a required tool in their inventory or equipment.
/// </summary>
public interface IToolChecker
{
    /// <summary>
    ///     Checks if the player has the specified tool.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="toolId">The tool item ID to look for.</param>
    /// <returns>True if the player has the tool.</returns>
    bool HasTool(Player player, ItemId toolId);

    /// <summary>
    ///     Checks if the player has any of the specified tools, returning the best one.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="toolIds">The tool item IDs to look for, in order from worst to best.</param>
    /// <param name="foundToolId">The ID of the best tool found.</param>
    /// <returns>True if any tool was found.</returns>
    bool HasAnyTool(Player player, IEnumerable<ItemId> toolIds, out ItemId? foundToolId);
}