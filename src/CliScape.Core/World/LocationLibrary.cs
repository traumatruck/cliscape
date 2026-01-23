using System.Reflection;

namespace CliScape.Core.World;

public class LocationLibrary
{
    private readonly Dictionary<LocationName, ILocation> _locations = new();
    private bool _isLoaded;

    public IReadOnlyDictionary<LocationName, ILocation> Locations => _locations;

    public void LoadFrom(Assembly assembly)
    {
        var locationTypes = assembly.GetTypes()
            .Where(type =>
                typeof(ILocation).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

        foreach (var type in locationTypes)
        {
            var nameProperty = type.GetProperty(nameof(ILocation.Name),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            if (nameProperty == null)
            {
                continue;
            }

            if (nameProperty.GetValue(null) is not LocationName name)
            {
                continue;
            }

            if (Activator.CreateInstance(type) is ILocation location)
            {
                _locations[name] = location;
            }
        }

        _isLoaded = true;
    }

    public ILocation? GetLocation(LocationName name)
    {
        return !_isLoaded
            ? throw new InvalidOperationException("LocationLibrary is not loaded")
            : _locations.GetValueOrDefault(name);
    }
}