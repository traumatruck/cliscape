using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents a tree in the world that players can chop for logs.
/// </summary>
public interface ITree
{
    /// <summary>
    ///     The display name of this tree type.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The type of tree.
    /// </summary>
    TreeType TreeType { get; }

    /// <summary>
    ///     The item ID of the logs produced when chopping this tree.
    /// </summary>
    ItemId LogItemId { get; }

    /// <summary>
    ///     The minimum woodcutting level required to chop this tree.
    /// </summary>
    int RequiredLevel { get; }

    /// <summary>
    ///     The woodcutting experience gained when successfully chopping this tree.
    /// </summary>
    int Experience { get; }

    /// <summary>
    ///     The maximum number of logs that can be obtained before the tree is depleted.
    ///     Use 1 for trees that deplete quickly (normal trees), higher values for better trees.
    /// </summary>
    int MaxActions { get; }
}

/// <summary>
///     The type of tree, affecting which axe levels can chop it.
/// </summary>
public enum TreeType
{
    /// <summary>
    ///     Regular trees found throughout the world.
    /// </summary>
    Normal,

    /// <summary>
    ///     Oak trees requiring level 15.
    /// </summary>
    Oak,

    /// <summary>
    ///     Willow trees requiring level 30.
    /// </summary>
    Willow,

    /// <summary>
    ///     Maple trees requiring level 45.
    /// </summary>
    Maple,

    /// <summary>
    ///     Yew trees requiring level 60.
    /// </summary>
    Yew,

    /// <summary>
    ///     Magic trees requiring level 75.
    /// </summary>
    Magic
}