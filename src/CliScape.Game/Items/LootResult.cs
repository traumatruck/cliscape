namespace CliScape.Game.Items;

/// <summary>
///     Result of a loot action.
/// </summary>
public record LootResult(
    bool Success,
    string Message,
    IReadOnlyList<LootedItem> ItemsLooted);