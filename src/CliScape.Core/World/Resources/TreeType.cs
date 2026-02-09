namespace CliScape.Core.World.Resources;

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