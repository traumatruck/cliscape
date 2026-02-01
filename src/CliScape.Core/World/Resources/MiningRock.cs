using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     A standard mining rock implementation.
/// </summary>
public class MiningRock : IMiningRock
{
    public required string Name { get; init; }

    public required RockType RockType { get; init; }

    public required ItemId OreItemId { get; init; }

    public required int RequiredLevel { get; init; }

    public required int Experience { get; init; }

    public required int MaxActions { get; init; }

    public required PickaxeTier RequiredPickaxe { get; init; }
}