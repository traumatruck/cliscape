using CliScape.Content.Items;
using CliScape.Core.ClueScrolls;

namespace CliScape.Content.ClueScrolls;

/// <summary>
///     Defines the reward tables for each clue scroll tier.
/// </summary>
public sealed class ClueRewardTable : IClueRewardTable
{
    public static readonly ClueRewardTable Instance = new();

    private static readonly IReadOnlyList<ClueReward> EasyRewards =
    [
        new() { ItemId = ItemIds.Coins, MinQuantity = 50, MaxQuantity = 500, Weight = 10 },
        new() { ItemId = ItemIds.Bread, MinQuantity = 3, MaxQuantity = 5, Weight = 8 },
        new() { ItemId = ItemIds.BronzeSword, Weight = 5 },
        new() { ItemId = ItemIds.BronzeFullHelm, Weight = 5 },
        new() { ItemId = ItemIds.BronzePlatebody, Weight = 4 },
        new() { ItemId = ItemIds.BronzeKiteshield, Weight = 4 },
        new() { ItemId = ItemIds.LeatherBody, Weight = 5 },
        new() { ItemId = ItemIds.LeatherChaps, Weight = 5 },
        new() { ItemId = ItemIds.Shortbow, Weight = 4 },
        new() { ItemId = ItemIds.BronzeArrow, MinQuantity = 10, MaxQuantity = 50, Weight = 6 }
    ];

    private static readonly IReadOnlyList<ClueReward> MediumRewards =
    [
        new() { ItemId = ItemIds.Coins, MinQuantity = 200, MaxQuantity = 2000, Weight = 10 },
        new() { ItemId = ItemIds.IronSword, Weight = 5 },
        new() { ItemId = ItemIds.IronFullHelm, Weight = 5 },
        new() { ItemId = ItemIds.IronPlatebody, Weight = 4 },
        new() { ItemId = ItemIds.IronKiteshield, Weight = 4 },
        new() { ItemId = ItemIds.SteelDagger, Weight = 3 },
        new() { ItemId = ItemIds.SteelMedHelm, Weight = 3 },
        new() { ItemId = ItemIds.Lobster, MinQuantity = 3, MaxQuantity = 5, Weight = 5 },
        new() { ItemId = ItemIds.Trout, MinQuantity = 5, MaxQuantity = 10, Weight = 6 },
        new() { ItemId = ItemIds.BronzeArrow, MinQuantity = 50, MaxQuantity = 150, Weight = 5 }
    ];

    private static readonly IReadOnlyList<ClueReward> HardRewards =
    [
        new() { ItemId = ItemIds.Coins, MinQuantity = 1000, MaxQuantity = 10000, Weight = 10 },
        new() { ItemId = ItemIds.SteelSword, Weight = 5 },
        new() { ItemId = ItemIds.SteelFullHelm, Weight = 5 },
        new() { ItemId = ItemIds.SteelPlatebody, Weight = 4 },
        new() { ItemId = ItemIds.SteelKiteshield, Weight = 4 },
        new() { ItemId = ItemIds.SteelScimitar, Weight = 3 },
        new() { ItemId = ItemIds.Lobster, MinQuantity = 5, MaxQuantity = 10, Weight = 5 },
        new() { ItemId = ItemIds.Salmon, MinQuantity = 5, MaxQuantity = 10, Weight = 5 },
        new() { ItemId = ItemIds.BronzeArrow, MinQuantity = 100, MaxQuantity = 300, Weight = 4 }
    ];

    private static readonly IReadOnlyList<ClueReward> EliteRewards =
    [
        new() { ItemId = ItemIds.Coins, MinQuantity = 5000, MaxQuantity = 50000, Weight = 10 },
        new() { ItemId = ItemIds.SteelPlatebody, Weight = 5 },
        new() { ItemId = ItemIds.SteelPlatelegs, Weight = 5 },
        new() { ItemId = ItemIds.SteelScimitar, Weight = 4 },
        new() { ItemId = ItemIds.SteelKiteshield, Weight = 4 },
        new() { ItemId = ItemIds.Lobster, MinQuantity = 10, MaxQuantity = 20, Weight = 5 },
        new() { ItemId = ItemIds.Swordfish, MinQuantity = 5, MaxQuantity = 10, Weight = 4 },
        new() { ItemId = ItemIds.BronzeArrow, MinQuantity = 200, MaxQuantity = 500, Weight = 3 }
    ];

    /// <inheritdoc />
    public IReadOnlyList<ClueReward> GetRewards(ClueScrollTier tier)
    {
        return tier switch
        {
            ClueScrollTier.Easy => EasyRewards,
            ClueScrollTier.Medium => MediumRewards,
            ClueScrollTier.Hard => HardRewards,
            ClueScrollTier.Elite => EliteRewards,
            _ => EasyRewards
        };
    }

    /// <inheritdoc />
    public int GetRewardRollCount(ClueScrollTier tier)
    {
        return tier switch
        {
            ClueScrollTier.Easy => 2,
            ClueScrollTier.Medium => 3,
            ClueScrollTier.Hard => 4,
            ClueScrollTier.Elite => 5,
            _ => 2
        };
    }
}