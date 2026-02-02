using CliScape.Core.World;

namespace CliScape.Game;

/// <summary>
///     Provides access to game locations.
/// </summary>
public interface ILocationRegistry
{
    /// <summary>
    ///     Gets a location by name.
    /// </summary>
    /// <param name="name">The name of the location.</param>
    /// <returns>The location if found, otherwise null.</returns>
    ILocation? GetLocation(string name);

    /// <summary>
    ///     Gets a location by its strongly-typed name.
    /// </summary>
    /// <param name="name">The location name.</param>
    /// <returns>The location if found, otherwise null.</returns>
    ILocation? GetLocation(LocationName name);

    /// <summary>
    ///     Gets all registered locations.
    /// </summary>
    IEnumerable<ILocation> GetAllLocations();
}