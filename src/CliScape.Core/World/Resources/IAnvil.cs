namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents an anvil in the world where players can smith bars into equipment.
/// </summary>
public interface IAnvil
{
    /// <summary>
    ///     The display name of this anvil.
    /// </summary>
    string Name { get; }
}

/// <summary>
///     A standard anvil implementation.
/// </summary>
public class Anvil : IAnvil
{
    public required string Name { get; init; }
}