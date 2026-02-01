using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Log items obtained from woodcutting.
/// </summary>
public static class Logs
{
    public static readonly IItem NormalLogs = new Item
    {
        Id = ItemIds.Logs,
        Name = new ItemName("Logs"),
        ExamineText = "A pile of logs.",
        BaseValue = 1
    };

    public static readonly IItem OakLogs = new Item
    {
        Id = ItemIds.OakLogs,
        Name = new ItemName("Oak logs"),
        ExamineText = "Logs cut from an oak tree.",
        BaseValue = 20
    };

    public static readonly IItem WillowLogs = new Item
    {
        Id = ItemIds.WillowLogs,
        Name = new ItemName("Willow logs"),
        ExamineText = "Logs cut from a willow tree.",
        BaseValue = 32
    };

    public static readonly IItem MapleLogs = new Item
    {
        Id = ItemIds.MapleLogs,
        Name = new ItemName("Maple logs"),
        ExamineText = "Logs cut from a maple tree.",
        BaseValue = 40
    };

    public static readonly IItem YewLogs = new Item
    {
        Id = ItemIds.YewLogs,
        Name = new ItemName("Yew logs"),
        ExamineText = "Logs cut from a yew tree.",
        BaseValue = 160
    };

    public static readonly IItem MagicLogs = new Item
    {
        Id = ItemIds.MagicLogs,
        Name = new ItemName("Magic logs"),
        ExamineText = "Logs cut from a magic tree.",
        BaseValue = 1020
    };
}