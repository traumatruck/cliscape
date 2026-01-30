using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
/// Food items that can heal the player.
/// </summary>
public static class Food
{
    public static readonly IItem Bread = new Item
    {
        Id = ItemIds.Bread,
        Name = new ItemName("Bread"),
        ExamineText = "A loaf of bread.",
        BaseValue = 12
    };

    public static readonly IItem Cake = new Item
    {
        Id = ItemIds.Cake,
        Name = new ItemName("Cake"),
        ExamineText = "A cake.",
        BaseValue = 50
    };

    public static readonly IItem Shrimp = new Item
    {
        Id = ItemIds.Shrimp,
        Name = new ItemName("Shrimps"),
        ExamineText = "Some cooked shrimps.",
        BaseValue = 5
    };

    public static readonly IItem Trout = new Item
    {
        Id = ItemIds.Trout,
        Name = new ItemName("Trout"),
        ExamineText = "A cooked trout.",
        BaseValue = 20
    };

    public static readonly IItem Salmon = new Item
    {
        Id = ItemIds.Salmon,
        Name = new ItemName("Salmon"),
        ExamineText = "A cooked salmon.",
        BaseValue = 50
    };

    public static readonly IItem Lobster = new Item
    {
        Id = ItemIds.Lobster,
        Name = new ItemName("Lobster"),
        ExamineText = "A cooked lobster.",
        BaseValue = 150
    };

    public static readonly IItem[] All =
    [
        Bread, Cake, Shrimp, Trout, Salmon, Lobster
    ];
}
