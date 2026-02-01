using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Burnt food items produced when failing to cook.
/// </summary>
public static class BurntFood
{
    public static readonly IItem BurntShrimps = new Item
    {
        Id = ItemIds.BurntShrimps,
        Name = new ItemName("Burnt shrimps"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntAnchovies = new Item
    {
        Id = ItemIds.BurntAnchovies,
        Name = new ItemName("Burnt anchovies"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntSardine = new Item
    {
        Id = ItemIds.BurntSardine,
        Name = new ItemName("Burnt sardine"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntHerring = new Item
    {
        Id = ItemIds.BurntHerring,
        Name = new ItemName("Burnt herring"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntTrout = new Item
    {
        Id = ItemIds.BurntTrout,
        Name = new ItemName("Burnt fish"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntSalmon = new Item
    {
        Id = ItemIds.BurntSalmon,
        Name = new ItemName("Burnt fish"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntLobster = new Item
    {
        Id = ItemIds.BurntLobster,
        Name = new ItemName("Burnt lobster"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntTuna = new Item
    {
        Id = ItemIds.BurntTuna,
        Name = new ItemName("Burnt fish"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntSwordfish = new Item
    {
        Id = ItemIds.BurntSwordfish,
        Name = new ItemName("Burnt fish"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntChicken = new Item
    {
        Id = ItemIds.BurntChicken,
        Name = new ItemName("Burnt chicken"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };

    public static readonly IItem BurntMeat = new Item
    {
        Id = ItemIds.BurntMeat,
        Name = new ItemName("Burnt meat"),
        ExamineText = "Oops!",
        BaseValue = 1,
        IsTradeable = false
    };
}