using System.Reflection;
using CliScape.Core.Npcs;
using CliScape.Core.World.Resources;

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
    ///     Whether a bank is available at this location.
    /// </summary>
    bool HasBank => false;

    /// <summary>
    ///     The fishing spots available at this location.
    /// </summary>
    IReadOnlyList<IFishingSpot> FishingSpots => [];

    /// <summary>
    ///     The trees available at this location.
    /// </summary>
    IReadOnlyList<ITree> Trees => [];

    /// <summary>
    ///     The mining rocks available at this location.
    /// </summary>
    IReadOnlyList<IMiningRock> MiningRocks => [];

    /// <summary>
    ///     The furnaces available at this location for smelting.
    /// </summary>
    IReadOnlyList<IFurnace> Furnaces => [];

    /// <summary>
    ///     The anvils available at this location for smithing.
    /// </summary>
    IReadOnlyList<IAnvil> Anvils => [];

    /// <summary>
    ///     The cooking ranges or fires available at this location.
    /// </summary>
    IReadOnlyList<ICookingRange> CookingRanges => [];

    /// <summary>
    ///     The thieving targets available at this location.
    /// </summary>
    IReadOnlyList<IThievingTarget> ThievingTargets => [];
}