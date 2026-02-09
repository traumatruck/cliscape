namespace CliScape.Core.World.Resources;

/// <summary>
///     A standard thieving target implementation.
/// </summary>
public class ThievingTarget : IThievingTarget
{
    public required string Name { get; init; }

    public required ThievingTargetType TargetType { get; init; }

    public required int RequiredLevel { get; init; }

    public required int Experience { get; init; }

    public required IReadOnlyList<ThievingLoot> PossibleLoot { get; init; }

    public required double BaseSuccessChance { get; init; }

    public required int FailureDamage { get; init; }
}