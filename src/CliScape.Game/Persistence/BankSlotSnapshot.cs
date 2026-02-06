namespace CliScape.Game.Persistence;

/// <summary>
///     Snapshot of a single bank slot for persistence.
/// </summary>
public readonly record struct BankSlotSnapshot(
    int SlotIndex,
    int ItemId,
    int Quantity);