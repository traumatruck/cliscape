using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Core.Skills;

/// <summary>
///     Default implementation of <see cref="IToolChecker" />.
/// </summary>
public sealed class ToolChecker : IToolChecker
{
    /// <summary>
    ///     Singleton instance.
    /// </summary>
    public static readonly ToolChecker Instance = new();

    private ToolChecker()
    {
    }

    /// <inheritdoc />
    public bool HasTool(Player player, ItemId toolId)
    {
        // Check inventory
        var hasInInventory = player.Inventory.GetItems()
            .Any(slot => slot.Item.Id == toolId);

        if (hasInInventory)
        {
            return true;
        }

        // Check equipped items
        return player.Equipment.GetAllEquipped()
            .Any(item => item.Id == toolId);
    }

    /// <inheritdoc />
    public bool HasAnyTool(Player player, IEnumerable<ItemId> toolIds, out ItemId? foundToolId)
    {
        foundToolId = null;

        // Check from best to worst (assuming toolIds are ordered worst to best)
        foreach (var toolId in toolIds.Reverse())
        {
            if (HasTool(player, toolId))
            {
                foundToolId = toolId;
                return true;
            }
        }

        return false;
    }
}