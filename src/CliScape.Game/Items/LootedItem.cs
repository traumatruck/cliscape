using CliScape.Core.Items;

namespace CliScape.Game.Items;

/// <summary>
///     An item that was looted.
/// </summary>
public record LootedItem(ItemId ItemId, string ItemName, int Quantity);
