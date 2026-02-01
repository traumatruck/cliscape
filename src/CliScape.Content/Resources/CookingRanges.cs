using CliScape.Core.World.Resources;

namespace CliScape.Content.Resources;

/// <summary>
///     Cooking range definitions for cooking food.
/// </summary>
public static class CookingRanges
{
    /// <summary>
    ///     A standard cooking range found in kitchens.
    /// </summary>
    public static readonly CookingRange StandardRange = new()
    {
        Name = "Cooking range",
        SourceType = CookingSourceType.Range,
        BurnChanceReduction = 5 // Ranges reduce burn chance by 5%
    };

    /// <summary>
    ///     The Lumbridge range in the castle kitchen.
    /// </summary>
    public static readonly CookingRange LumbridgeRange = new()
    {
        Name = "Lumbridge range",
        SourceType = CookingSourceType.Range,
        BurnChanceReduction = 10 // Cook's range has better burn protection in OSRS
    };

    /// <summary>
    ///     A campfire - basic cooking source with higher burn chance.
    /// </summary>
    public static readonly CookingRange Campfire = new()
    {
        Name = "Fire",
        SourceType = CookingSourceType.Fire,
        BurnChanceReduction = 0
    };
}