namespace CliScape.Game.Persistence;

/// <summary>
///     Represents a snapshot of the player's state at a point in time.
///     This is used for serialization and persistence of player data.
/// </summary>
/// <param name="Id">The unique identifier for the player.</param>
/// <param name="Name">The player's character name.</param>
/// <param name="LocationName">The name of the location where the player is currently located.</param>
/// <param name="CurrentHealth">The player's current health points.</param>
/// <param name="MaximumHealth">The player's maximum health points.</param>
/// <param name="Skills">The player's skill experience values.</param>
/// <param name="InventorySlots">The player's inventory slots.</param>
/// <param name="EquippedItems">The player's equipped items.</param>
/// <param name="SlayerTask">The player's current slayer task, if any.</param>
/// <param name="DiaryProgress">The player's achievement diary progress.</param>
/// <param name="ClaimedDiaryRewards">The diary tier rewards that have been claimed.</param>
public readonly record struct PlayerSnapshot(
    int Id,
    string Name,
    string LocationName,
    int CurrentHealth,
    int MaximumHealth,
    SkillSnapshot[] Skills,
    InventorySlotSnapshot[]? InventorySlots = null,
    EquippedItemSnapshot[]? EquippedItems = null,
    SlayerTaskSnapshot? SlayerTask = null,
    DiaryProgressSnapshot[]? DiaryProgress = null,
    string[]? ClaimedDiaryRewards = null);