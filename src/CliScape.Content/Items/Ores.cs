using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Ore items obtained from mining.
/// </summary>
public static class Ores
{
    public static readonly IItem CopperOre = new Item
    {
        Id = ItemIds.CopperOre,
        Name = new ItemName("Copper ore"),
        ExamineText = "Some copper ore.",
        BaseValue = 17
    };

    public static readonly IItem TinOre = new Item
    {
        Id = ItemIds.TinOre,
        Name = new ItemName("Tin ore"),
        ExamineText = "Some tin ore.",
        BaseValue = 18
    };

    public static readonly IItem IronOre = new Item
    {
        Id = ItemIds.IronOre,
        Name = new ItemName("Iron ore"),
        ExamineText = "Some iron ore.",
        BaseValue = 28
    };

    public static readonly IItem Coal = new Item
    {
        Id = ItemIds.Coal,
        Name = new ItemName("Coal"),
        ExamineText = "A heap of coal.",
        BaseValue = 67
    };

    public static readonly IItem MithrilOre = new Item
    {
        Id = ItemIds.MithrilOre,
        Name = new ItemName("Mithril ore"),
        ExamineText = "Some mithril ore.",
        BaseValue = 162
    };

    public static readonly IItem AdamantiteOre = new Item
    {
        Id = ItemIds.AdamantiteOre,
        Name = new ItemName("Adamantite ore"),
        ExamineText = "Some adamantite ore.",
        BaseValue = 640
    };

    public static readonly IItem RuniteOre = new Item
    {
        Id = ItemIds.RuniteOre,
        Name = new ItemName("Runite ore"),
        ExamineText = "Some runite ore.",
        BaseValue = 6400
    };

    public static readonly IItem SilverOre = new Item
    {
        Id = ItemIds.SilverOre,
        Name = new ItemName("Silver ore"),
        ExamineText = "Some silver ore.",
        BaseValue = 75
    };

    public static readonly IItem GoldOre = new Item
    {
        Id = ItemIds.GoldOre,
        Name = new ItemName("Gold ore"),
        ExamineText = "Some gold ore.",
        BaseValue = 150
    };
}