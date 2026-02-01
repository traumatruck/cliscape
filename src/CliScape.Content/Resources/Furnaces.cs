using CliScape.Core.World.Resources;

namespace CliScape.Content.Resources;

/// <summary>
///     Furnace definitions for smelting ores into bars.
/// </summary>
public static class Furnaces
{
    /// <summary>
    ///     A standard furnace found in most towns.
    /// </summary>
    public static readonly Furnace StandardFurnace = new()
    {
        Name = "Furnace"
    };

    /// <summary>
    ///     The Lumbridge furnace located in the basement.
    /// </summary>
    public static readonly Furnace LumbridgeFurnace = new()
    {
        Name = "Lumbridge furnace"
    };

    /// <summary>
    ///     The Varrock furnace near the east bank.
    /// </summary>
    public static readonly Furnace VarrockFurnace = new()
    {
        Name = "Varrock furnace"
    };

    /// <summary>
    ///     The Falador furnace near the mining guild.
    /// </summary>
    public static readonly Furnace FaladorFurnace = new()
    {
        Name = "Falador furnace"
    };

    /// <summary>
    ///     The Al Kharid furnace, popular for its proximity to the bank.
    /// </summary>
    public static readonly Furnace AlKharidFurnace = new()
    {
        Name = "Al Kharid furnace"
    };
}