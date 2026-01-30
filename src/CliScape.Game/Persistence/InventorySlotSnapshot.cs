namespace CliScape.Game.Persistence;

/// <summary>
///     Represents a snapshot of an inventory slot for persistence.
/// </summary>
/// <param name="ItemId">The ID of the item in this slot.</param>
/// <param name="Quantity">The quantity of items in this slot.</param>
public readonly record struct InventorySlotSnapshot(int ItemId, int Quantity);