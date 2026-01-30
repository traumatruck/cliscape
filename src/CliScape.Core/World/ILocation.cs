using System.Reflection;
using CliScape.Core.Npcs;

namespace CliScape.Core.World;

public interface ILocation
{
    LocationName Name
    {
        get
        {
            var prop = GetType().GetProperty(nameof(Name),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return prop?.GetValue(null) as LocationName ??
                   throw new InvalidOperationException($"Location {GetType().Name} is missing static Name.");
        }
    }

    IReadOnlyList<INpc> AvailableNpcs { get; }

    /// <summary>
    ///     The shops available at this location.
    /// </summary>
    IReadOnlyList<Shop> Shops => [];

    /// <summary>
    ///     The bank at this location, if any.
    /// </summary>
    Bank? Bank => null;
}