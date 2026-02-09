namespace CliScape.Core.World.Resources;

/// <summary>
///     A standard cooking range implementation.
/// </summary>
public class CookingRange : ICookingRange
{
    public required string Name { get; init; }

    public required CookingSourceType SourceType { get; init; }

    public required int BurnChanceReduction { get; init; }
}