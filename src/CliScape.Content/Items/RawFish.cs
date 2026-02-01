using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Raw fish items obtained from fishing.
/// </summary>
public static class RawFish
{
    public static readonly IItem RawShrimps = new Item
    {
        Id = ItemIds.RawShrimps,
        Name = new ItemName("Raw shrimps"),
        ExamineText = "Some raw shrimps.",
        BaseValue = 5
    };

    public static readonly IItem RawAnchovies = new Item
    {
        Id = ItemIds.RawAnchovies,
        Name = new ItemName("Raw anchovies"),
        ExamineText = "Some raw anchovies.",
        BaseValue = 15
    };

    public static readonly IItem RawSardine = new Item
    {
        Id = ItemIds.RawSardine,
        Name = new ItemName("Raw sardine"),
        ExamineText = "A raw sardine.",
        BaseValue = 10
    };

    public static readonly IItem RawHerring = new Item
    {
        Id = ItemIds.RawHerring,
        Name = new ItemName("Raw herring"),
        ExamineText = "A raw herring.",
        BaseValue = 20
    };

    public static readonly IItem RawTrout = new Item
    {
        Id = ItemIds.RawTrout,
        Name = new ItemName("Raw trout"),
        ExamineText = "A raw trout.",
        BaseValue = 26
    };

    public static readonly IItem RawSalmon = new Item
    {
        Id = ItemIds.RawSalmon,
        Name = new ItemName("Raw salmon"),
        ExamineText = "A raw salmon.",
        BaseValue = 50
    };

    public static readonly IItem RawLobster = new Item
    {
        Id = ItemIds.RawLobster,
        Name = new ItemName("Raw lobster"),
        ExamineText = "A raw lobster.",
        BaseValue = 120
    };

    public static readonly IItem RawTuna = new Item
    {
        Id = ItemIds.RawTuna,
        Name = new ItemName("Raw tuna"),
        ExamineText = "A very large fish.",
        BaseValue = 100
    };

    public static readonly IItem RawSwordfish = new Item
    {
        Id = ItemIds.RawSwordfish,
        Name = new ItemName("Raw swordfish"),
        ExamineText = "An uncooked swordfish.",
        BaseValue = 200
    };
}