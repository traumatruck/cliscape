namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents a furnace in the world where players can smelt ores into bars.
/// </summary>
public interface IFurnace
{
    /// <summary>
    ///     The display name of this furnace.
    /// </summary>
    string Name { get; }
}

/// <summary>
///     A standard furnace implementation.
/// </summary>
public class Furnace : IFurnace
{
    public required string Name { get; init; }
}