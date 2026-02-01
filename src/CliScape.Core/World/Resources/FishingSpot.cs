using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     A standard fishing spot implementation.
/// </summary>
public class FishingSpot : IFishingSpot
{
    public required string Name { get; init; }

    public required FishingSpotType SpotType { get; init; }

    public required IReadOnlyList<FishingYield> PossibleCatches { get; init; }

    public required int RequiredLevel { get; init; }

    public required ItemId RequiredTool { get; init; }

    public required int MaxActions { get; init; }
}