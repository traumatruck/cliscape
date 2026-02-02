using CliScape.Core.Combat;
using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Game.Items;

/// <summary>
///     Handles looting logic.
/// </summary>
public interface ILootService
{
    /// <summary>
    ///     Loots all available items.
    /// </summary>
    LootResult LootAll(PendingLoot pendingLoot, Player player, Func<ItemId, IItem?> itemResolver);

    /// <summary>
    ///     Loots a specific item by name.
    /// </summary>
    LootResult LootItem(PendingLoot pendingLoot, Player player, string itemName, int? amount,
        Func<ItemId, IItem?> itemResolver);
}
