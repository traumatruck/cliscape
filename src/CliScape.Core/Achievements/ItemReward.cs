using CliScape.Core.Items;

namespace CliScape.Core.Achievements;

/// <summary>
///     Represents an item reward from completing a diary tier.
/// </summary>
public sealed class ItemReward : DiaryReward
{
    public required ItemId ItemId { get; init; }
    public required int Quantity { get; init; }
}