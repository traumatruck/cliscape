namespace CliScape.Game.Persistence;

/// <summary>
///     Represents a snapshot of an equipped item for persistence.
/// </summary>
/// <param name="Slot">The equipment slot (as int from EquipmentSlot enum).</param>
/// <param name="ItemId">The ID of the equipped item.</param>
public readonly record struct EquippedItemSnapshot(int Slot, int ItemId);