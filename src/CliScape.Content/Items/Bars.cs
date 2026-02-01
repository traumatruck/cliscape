using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Metal bar items created from smelting ores.
/// </summary>
public static class Bars
{
    public static readonly IItem BronzeBar = new Item
    {
        Id = ItemIds.BronzeBar,
        Name = new ItemName("Bronze bar"),
        ExamineText = "It's a bar of bronze.",
        BaseValue = 38
    };

    public static readonly IItem IronBar = new Item
    {
        Id = ItemIds.IronBar,
        Name = new ItemName("Iron bar"),
        ExamineText = "It's a bar of iron.",
        BaseValue = 84
    };

    public static readonly IItem SteelBar = new Item
    {
        Id = ItemIds.SteelBar,
        Name = new ItemName("Steel bar"),
        ExamineText = "It's a bar of steel.",
        BaseValue = 300
    };

    public static readonly IItem MithrilBar = new Item
    {
        Id = ItemIds.MithrilBar,
        Name = new ItemName("Mithril bar"),
        ExamineText = "It's a bar of mithril.",
        BaseValue = 900
    };

    public static readonly IItem AdamantiteBar = new Item
    {
        Id = ItemIds.AdamantiteBar,
        Name = new ItemName("Adamantite bar"),
        ExamineText = "It's a bar of adamantite.",
        BaseValue = 3200
    };

    public static readonly IItem RuniteBar = new Item
    {
        Id = ItemIds.RuniteBar,
        Name = new ItemName("Runite bar"),
        ExamineText = "It's a bar of runite.",
        BaseValue = 12800
    };
}