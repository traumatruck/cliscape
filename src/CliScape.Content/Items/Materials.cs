using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Raw materials and drops from monsters.
/// </summary>
public static class Materials
{
    public static readonly IItem Coins = new Item
    {
        Id = ItemIds.Coins,
        Name = new ItemName("Coins"),
        ExamineText = "Lovely money!",
        BaseValue = 1,
        IsStackable = true,
        IsTradeable = false
    };

    public static readonly IBuryable Bones = new BuryableItem
    {
        Id = ItemIds.Bones,
        Name = new ItemName("Bones"),
        ExamineText = "Ew, it's a pile of bones.",
        BaseValue = 1,
        PrayerExperience = 5 // Standard bones give 4.5 XP in OSRS, we round to 5
    };

    public static readonly IItem RawBeef = new Item
    {
        Id = ItemIds.RawBeef,
        Name = new ItemName("Raw beef"),
        ExamineText = "Some raw meat.",
        BaseValue = 1
    };

    public static readonly IItem Cowhide = new Item
    {
        Id = ItemIds.Cowhide,
        Name = new ItemName("Cowhide"),
        ExamineText = "I could make leather from this.",
        BaseValue = 2
    };

    public static readonly IItem Feather = new Item
    {
        Id = ItemIds.Feather,
        Name = new ItemName("Feather"),
        ExamineText = "A feather.",
        BaseValue = 2,
        IsStackable = true
    };

    public static readonly IItem RawChicken = new Item
    {
        Id = ItemIds.RawChicken,
        Name = new ItemName("Raw chicken"),
        ExamineText = "Some raw meat.",
        BaseValue = 1
    };
}