using CliScape.Core.Items;

namespace CliScape.Game.Items;

/// <summary>
///     Result of an equip action.
/// </summary>
public record EquipResult(
    bool Success,
    string Message,
    IEquippable? EquippedItem = null,
    IEquippable? UnequippedItem = null);
