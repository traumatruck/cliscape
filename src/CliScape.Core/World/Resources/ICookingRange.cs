namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents a cooking range or fire in the world where players can cook food.
/// </summary>
public interface ICookingRange
{
    /// <summary>
    ///     The display name of this cooking source.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The type of cooking source.
    /// </summary>
    CookingSourceType SourceType { get; }

    /// <summary>
    ///     The burn chance reduction compared to a regular fire.
    ///     Ranges provide better burn protection than fires.
    /// </summary>
    int BurnChanceReduction { get; }
}

/// <summary>
///     The type of cooking source.
/// </summary>
public enum CookingSourceType
{
    /// <summary>
    ///     A fire made from logs.
    /// </summary>
    Fire,

    /// <summary>
    ///     A proper cooking range.
    /// </summary>
    Range
}

/// <summary>
///     A standard cooking range implementation.
/// </summary>
public class CookingRange : ICookingRange
{
    public required string Name { get; init; }

    public required CookingSourceType SourceType { get; init; }

    public required int BurnChanceReduction { get; init; }
}