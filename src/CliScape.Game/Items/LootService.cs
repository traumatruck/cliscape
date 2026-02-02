using CliScape.Core.Combat;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Game.Items;

/// <summary>
///     Result of a loot action.
/// </summary>
public record LootResult(
    bool Success,
    string Message,
    IReadOnlyList<LootedItem> ItemsLooted);

/// <summary>
///     An item that was looted.
/// </summary>
public record LootedItem(ItemId ItemId, string ItemName, int Quantity);

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

/// <summary>
///     Default implementation of <see cref="ILootService" />.
/// </summary>
public sealed class LootService : ILootService
{
    public static readonly LootService Instance = new(DomainEventDispatcher.Instance);
    private readonly IDomainEventDispatcher _eventDispatcher;

    public LootService(IDomainEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public LootResult LootAll(PendingLoot pendingLoot, Player player, Func<ItemId, IItem?> itemResolver)
    {
        var itemsLooted = new List<LootedItem>();
        var itemsSkipped = 0;

        // Create a copy of the list since we'll be modifying it
        var itemsToLoot = pendingLoot.Items.ToList();

        foreach (var lootItem in itemsToLoot)
        {
            var item = itemResolver(lootItem.ItemId);
            if (item == null)
            {
                continue;
            }

            if (player.Inventory.CanAdd(item, lootItem.Quantity))
            {
                player.Inventory.TryAdd(item, lootItem.Quantity);
                pendingLoot.Remove(lootItem.ItemId, lootItem.Quantity);

                itemsLooted.Add(new LootedItem(lootItem.ItemId, item.Name.Value, lootItem.Quantity));
                _eventDispatcher.Raise(new ItemPickedUpEvent(lootItem.ItemId, lootItem.Quantity));
            }
            else
            {
                itemsSkipped++;
            }
        }

        if (itemsLooted.Count == 0 && itemsSkipped > 0)
        {
            return new LootResult(false, "Your inventory is full!", []);
        }

        var message = itemsLooted.Count > 0
            ? $"Picked up {itemsLooted.Count} item(s)."
            : "Nothing to pick up.";

        return new LootResult(true, message, itemsLooted);
    }

    /// <inheritdoc />
    public LootResult LootItem(PendingLoot pendingLoot, Player player, string itemName, int? amount,
        Func<ItemId, IItem?> itemResolver)
    {
        // Find matching item in pending loot
        LootItem? matchingLoot = null;
        IItem? matchingItem = null;

        foreach (var lootItem in pendingLoot.Items)
        {
            var item = itemResolver(lootItem.ItemId);
            if (item != null && item.Name.Value.Contains(itemName, StringComparison.OrdinalIgnoreCase))
            {
                matchingLoot = lootItem;
                matchingItem = item;
                break;
            }
        }

        if (matchingLoot == null || matchingItem == null)
        {
            return new LootResult(false, $"Could not find '{itemName}' in the loot.", []);
        }

        // Determine quantity to pick up
        var quantityToLoot = amount ?? matchingLoot.Quantity;
        quantityToLoot = Math.Min(quantityToLoot, matchingLoot.Quantity);

        if (quantityToLoot <= 0)
        {
            return new LootResult(true, "Nothing to pick up.", []);
        }

        // Check inventory space
        if (!player.Inventory.CanAdd(matchingItem, quantityToLoot))
        {
            return new LootResult(false, "Your inventory is full!", []);
        }

        // Pick up the item
        player.Inventory.TryAdd(matchingItem, quantityToLoot);
        pendingLoot.Remove(matchingLoot.ItemId, quantityToLoot);

        _eventDispatcher.Raise(new ItemPickedUpEvent(matchingLoot.ItemId, quantityToLoot));

        var quantityText = quantityToLoot > 1 ? $" x{quantityToLoot}" : "";
        var message = $"Picked up {matchingItem.Name}{quantityText}.";

        return new LootResult(true, message,
            [new LootedItem(matchingLoot.ItemId, matchingItem.Name.Value, quantityToLoot)]);
    }
}