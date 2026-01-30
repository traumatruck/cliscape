using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Ammunition items for ranged combat.
/// </summary>
public static class Ammunition
{
    public static readonly IItem BronzeArrow = new Item
    {
        Id = ItemIds.BronzeArrow,
        Name = new ItemName("Bronze arrow"),
        ExamineText = "Arrows with bronze heads.",
        BaseValue = 1,
        IsStackable = true
    };
}
