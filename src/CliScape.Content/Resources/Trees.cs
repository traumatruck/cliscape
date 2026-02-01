using CliScape.Content.Items;
using CliScape.Core.World.Resources;

namespace CliScape.Content.Resources;

/// <summary>
///     Tree definitions for the game world.
/// </summary>
public static class Trees
{
    /// <summary>
    ///     Normal trees found throughout the world.
    /// </summary>
    public static readonly Tree NormalTree = new()
    {
        Name = "Tree",
        TreeType = TreeType.Normal,
        LogItemId = ItemIds.Logs,
        RequiredLevel = 1,
        Experience = 25,
        MaxActions = 1 // Normal trees fall after one log
    };

    /// <summary>
    ///     Oak trees requiring level 15 woodcutting.
    /// </summary>
    public static readonly Tree OakTree = new()
    {
        Name = "Oak tree",
        TreeType = TreeType.Oak,
        LogItemId = ItemIds.OakLogs,
        RequiredLevel = 15,
        Experience = 38, // OSRS gives 37.5, we round
        MaxActions = 3 // Oak trees give a few logs before falling
    };

    /// <summary>
    ///     Willow trees requiring level 30 woodcutting.
    /// </summary>
    public static readonly Tree WillowTree = new()
    {
        Name = "Willow tree",
        TreeType = TreeType.Willow,
        LogItemId = ItemIds.WillowLogs,
        RequiredLevel = 30,
        Experience = 68, // OSRS gives 67.5, we round
        MaxActions = 8 // Willows are great for training
    };

    /// <summary>
    ///     Maple trees requiring level 45 woodcutting.
    /// </summary>
    public static readonly Tree MapleTree = new()
    {
        Name = "Maple tree",
        TreeType = TreeType.Maple,
        LogItemId = ItemIds.MapleLogs,
        RequiredLevel = 45,
        Experience = 100,
        MaxActions = 10
    };

    /// <summary>
    ///     Yew trees requiring level 60 woodcutting.
    /// </summary>
    public static readonly Tree YewTree = new()
    {
        Name = "Yew tree",
        TreeType = TreeType.Yew,
        LogItemId = ItemIds.YewLogs,
        RequiredLevel = 60,
        Experience = 175,
        MaxActions = 12
    };

    /// <summary>
    ///     Magic trees requiring level 75 woodcutting.
    /// </summary>
    public static readonly Tree MagicTree = new()
    {
        Name = "Magic tree",
        TreeType = TreeType.Magic,
        LogItemId = ItemIds.MagicLogs,
        RequiredLevel = 75,
        Experience = 250,
        MaxActions = 15
    };
}