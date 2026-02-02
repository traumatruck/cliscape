using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents a mining rock in the world that players can mine for ore.
/// </summary>
public interface IMiningRock
{
    /// <summary>
    ///     The display name of this rock type.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The type of rock.
    /// </summary>
    RockType RockType { get; }

    /// <summary>
    ///     The item ID of the ore produced when mining this rock.
    /// </summary>
    ItemId OreItemId { get; }

    /// <summary>
    ///     The minimum mining level required to mine this rock.
    /// </summary>
    int RequiredLevel { get; }

    /// <summary>
    ///     The mining experience gained when successfully mining this rock.
    /// </summary>
    int Experience { get; }

    /// <summary>
    ///     The maximum number of ores that can be obtained before the rock is depleted.
    /// </summary>
    int MaxActions { get; }

    /// <summary>
    ///     The minimum pickaxe tier required to mine this rock.
    /// </summary>
    PickaxeTier RequiredPickaxe { get; }
}
