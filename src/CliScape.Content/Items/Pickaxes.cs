using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Pickaxe items used for mining.
/// </summary>
public static class Pickaxes
{
    public static readonly IItem IronPickaxe = new Item
    {
        Id = ItemIds.IronPickaxe,
        Name = new ItemName("Iron pickaxe"),
        ExamineText = "Used for mining.",
        BaseValue = 140
    };

    public static readonly IItem SteelPickaxe = new Item
    {
        Id = ItemIds.SteelPickaxe,
        Name = new ItemName("Steel pickaxe"),
        ExamineText = "Used for mining.",
        BaseValue = 500
    };

    public static readonly IItem MithrilPickaxe = new Item
    {
        Id = ItemIds.MithrilPickaxe,
        Name = new ItemName("Mithril pickaxe"),
        ExamineText = "Used for mining.",
        BaseValue = 1300
    };

    public static readonly IItem AdamantPickaxe = new Item
    {
        Id = ItemIds.AdamantPickaxe,
        Name = new ItemName("Adamant pickaxe"),
        ExamineText = "Used for mining.",
        BaseValue = 3200
    };

    public static readonly IItem RunePickaxe = new Item
    {
        Id = ItemIds.RunePickaxe,
        Name = new ItemName("Rune pickaxe"),
        ExamineText = "Used for mining.",
        BaseValue = 32000
    };
}