using CliScape.Core.Items;

namespace CliScape.Core.Npcs;

/// <summary>
///     Represents a potential drop from an NPC.
/// </summary>
public record NpcDrop
{
    /// <summary>
    ///     The item that can be dropped.
    /// </summary>
    public required ItemId ItemId { get; init; }

    /// <summary>
    ///     The minimum quantity that can be dropped.
    /// </summary>
    public int MinQuantity { get; init; } = 1;

    /// <summary>
    ///     The maximum quantity that can be dropped.
    /// </summary>
    public int MaxQuantity { get; init; } = 1;

    /// <summary>
    ///     The rarity of this drop.
    /// </summary>
    public DropRarity Rarity { get; init; } = DropRarity.Always;

    /// <summary>
    ///     Custom drop rate (1/x chance). Only used when Rarity is Custom.
    /// </summary>
    public int CustomRate { get; init; } = 1;
}
