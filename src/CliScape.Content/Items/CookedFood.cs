using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Additional cooked food items (beyond what was already in Food.cs).
/// </summary>
public static class CookedFood
{
    public static readonly EdibleItem CookedChicken = new()
    {
        Id = ItemIds.CookedChicken,
        Name = new ItemName("Cooked chicken"),
        ExamineText = "Tasty.",
        BaseValue = 12,
        HealAmount = 3
    };

    public static readonly EdibleItem CookedMeat = new()
    {
        Id = ItemIds.CookedMeat,
        Name = new ItemName("Cooked meat"),
        ExamineText = "Nicely cooked meat.",
        BaseValue = 10,
        HealAmount = 3
    };

    public static readonly EdibleItem Anchovies = new()
    {
        Id = ItemIds.Anchovies,
        Name = new ItemName("Anchovies"),
        ExamineText = "Some cooked anchovies.",
        BaseValue = 15,
        HealAmount = 1
    };

    public static readonly EdibleItem Sardine = new()
    {
        Id = ItemIds.Sardine,
        Name = new ItemName("Sardine"),
        ExamineText = "A cooked sardine.",
        BaseValue = 20,
        HealAmount = 4
    };

    public static readonly EdibleItem Herring = new()
    {
        Id = ItemIds.Herring,
        Name = new ItemName("Herring"),
        ExamineText = "A cooked herring.",
        BaseValue = 30,
        HealAmount = 5
    };

    public static readonly EdibleItem Tuna = new()
    {
        Id = ItemIds.Tuna,
        Name = new ItemName("Tuna"),
        ExamineText = "A cooked tuna.",
        BaseValue = 100,
        HealAmount = 10
    };

    public static readonly EdibleItem Swordfish = new()
    {
        Id = ItemIds.Swordfish,
        Name = new ItemName("Swordfish"),
        ExamineText = "A cooked swordfish.",
        BaseValue = 200,
        HealAmount = 14
    };
}