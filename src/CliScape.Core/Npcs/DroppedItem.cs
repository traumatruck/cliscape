using CliScape.Core.Items;

namespace CliScape.Core.Npcs;

/// <summary>
///     Represents an item that has been dropped.
/// </summary>
public record DroppedItem(ItemId ItemId, int Quantity);
