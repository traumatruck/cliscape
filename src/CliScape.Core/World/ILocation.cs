using System.Reflection;

namespace CliScape.Core.World;

public interface ILocation
{
    LocationName Name 
    { 
        get 
        {
            var prop = GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return prop?.GetValue(null) as LocationName ?? throw new InvalidOperationException($"Location {GetType().Name} is missing static Name");
        }
    }

    IDictionary<Direction, LocationName> AdjacentLocations { get; }
}