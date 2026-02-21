using System.Reflection;
using CliScape.Core.World;

namespace CliScape.Game;

/// <summary>
///     Default implementation of <see cref="ILocationRegistry" />.
/// </summary>
public sealed class LocationRegistry : ILocationRegistry
{
    /// <summary>
    ///     Singleton instance for simple access patterns.
    ///     Initialized with the Content assembly by <see cref="Initialize" />.
    /// </summary>
    public static LocationRegistry Instance { get; private set; } = null!;

    private readonly LocationLibrary _library = new();

    /// <summary>
    ///     Initializes the singleton with the specified content assembly.
    /// </summary>
    public static void Initialize(Assembly contentAssembly)
    {
        Instance = new LocationRegistry(contentAssembly);
    }

    /// <summary>
    ///     Creates a new location registry with locations loaded from the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for locations.</param>
    public LocationRegistry(Assembly assembly)
    {
        _library.LoadFrom(assembly);
    }

    /// <inheritdoc />
    public ILocation? GetLocation(string name)
    {
        return _library.GetLocation(new LocationName(name));
    }

    /// <inheritdoc />
    public ILocation? GetLocation(LocationName name)
    {
        return _library.GetLocation(name);
    }

    /// <inheritdoc />
    public IEnumerable<ILocation> GetAllLocations()
    {
        return _library.Locations.Values;
    }
}