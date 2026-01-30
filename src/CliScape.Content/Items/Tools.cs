using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Tools and utility items.
/// </summary>
public static class Tools
{
    public static readonly IItem Tinderbox = new Item
    {
        Id = ItemIds.Tinderbox,
        Name = new ItemName("Tinderbox"),
        ExamineText = "Useful for lighting a fire.",
        BaseValue = 1
    };

    public static readonly IItem SmallFishingNet = new Item
    {
        Id = ItemIds.SmallFishingNet,
        Name = new ItemName("Small fishing net"),
        ExamineText = "Useful for catching small fish.",
        BaseValue = 5
    };

    public static readonly IItem Hammer = new Item
    {
        Id = ItemIds.Hammer,
        Name = new ItemName("Hammer"),
        ExamineText = "Good for hitting things!",
        BaseValue = 1
    };
}
