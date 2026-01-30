using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Food items that can heal the player.
/// </summary>
public static class Food
{
    public static readonly IEdible Bread = new EdibleItem
    {
        Id = ItemIds.Bread,
        Name = new ItemName("Bread"),
        ExamineText = "A loaf of bread.",
        BaseValue = 12,
        HealAmount = 5
    };

    public static readonly IEdible Cake = new EdibleItem
    {
        Id = ItemIds.Cake,
        Name = new ItemName("Cake"),
        ExamineText = "A cake.",
        BaseValue = 50,
        HealAmount = 4 // Each slice heals 4, this is the full cake
    };

    public static readonly IEdible Shrimp = new EdibleItem
    {
        Id = ItemIds.Shrimp,
        Name = new ItemName("Shrimps"),
        ExamineText = "Some cooked shrimps.",
        BaseValue = 5,
        HealAmount = 3
    };

    public static readonly IEdible Trout = new EdibleItem
    {
        Id = ItemIds.Trout,
        Name = new ItemName("Trout"),
        ExamineText = "A cooked trout.",
        BaseValue = 20,
        HealAmount = 7
    };

    public static readonly IEdible Salmon = new EdibleItem
    {
        Id = ItemIds.Salmon,
        Name = new ItemName("Salmon"),
        ExamineText = "A cooked salmon.",
        BaseValue = 50,
        HealAmount = 9
    };

    public static readonly IEdible Lobster = new EdibleItem
    {
        Id = ItemIds.Lobster,
        Name = new ItemName("Lobster"),
        ExamineText = "A cooked lobster.",
        BaseValue = 150,
        HealAmount = 12
    };
}