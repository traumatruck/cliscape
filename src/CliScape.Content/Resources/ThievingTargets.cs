using CliScape.Content.Items;
using CliScape.Core.World.Resources;

namespace CliScape.Content.Resources;

/// <summary>
///     Thieving target definitions for the game world.
/// </summary>
public static class ThievingTargets
{
    /// <summary>
    ///     Bakery stall - steals bread and cakes.
    /// </summary>
    public static readonly ThievingTarget BakeryStall = new()
    {
        Name = "Bakery stall",
        TargetType = ThievingTargetType.Stall,
        RequiredLevel = 5,
        Experience = 16,
        BaseSuccessChance = 0.7,
        FailureDamage = 1,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Bread, 1, 1, 60),
            new ThievingLoot(ItemIds.Cake, 1, 1, 40)
        ]
    };

    /// <summary>
    ///     Tea stall - steals tea.
    /// </summary>
    public static readonly ThievingTarget TeaStall = new()
    {
        Name = "Tea stall",
        TargetType = ThievingTargetType.Stall,
        RequiredLevel = 5,
        Experience = 16,
        BaseSuccessChance = 0.75,
        FailureDamage = 1,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 5, 15, 100)
        ]
    };

    /// <summary>
    ///     Silk stall - steals silk (represented as coins here).
    /// </summary>
    public static readonly ThievingTarget SilkStall = new()
    {
        Name = "Silk stall",
        TargetType = ThievingTargetType.Stall,
        RequiredLevel = 20,
        Experience = 24,
        BaseSuccessChance = 0.6,
        FailureDamage = 2,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 20, 60, 100)
        ]
    };

    /// <summary>
    ///     Fur stall - steals furs (represented as coins here).
    /// </summary>
    public static readonly ThievingTarget FurStall = new()
    {
        Name = "Fur stall",
        TargetType = ThievingTargetType.Stall,
        RequiredLevel = 35,
        Experience = 36,
        BaseSuccessChance = 0.55,
        FailureDamage = 2,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 30, 90, 100)
        ]
    };

    /// <summary>
    ///     Silver stall - steals silver (represented as coins here).
    /// </summary>
    public static readonly ThievingTarget SilverStall = new()
    {
        Name = "Silver stall",
        TargetType = ThievingTargetType.Stall,
        RequiredLevel = 50,
        Experience = 54,
        BaseSuccessChance = 0.5,
        FailureDamage = 3,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 50, 150, 100)
        ]
    };

    /// <summary>
    ///     Gem stall - steals gems (represented as coins here).
    /// </summary>
    public static readonly ThievingTarget GemStall = new()
    {
        Name = "Gem stall",
        TargetType = ThievingTargetType.Stall,
        RequiredLevel = 75,
        Experience = 160,
        BaseSuccessChance = 0.4,
        FailureDamage = 5,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 100, 300, 100)
        ]
    };

    /// <summary>
    ///     Man - basic pickpocket target.
    /// </summary>
    public static readonly ThievingTarget Man = new()
    {
        Name = "Man",
        TargetType = ThievingTargetType.Npc,
        RequiredLevel = 1,
        Experience = 8,
        BaseSuccessChance = 0.8,
        FailureDamage = 1,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 3, 10, 100)
        ]
    };

    /// <summary>
    ///     Farmer - slightly better loot than man.
    /// </summary>
    public static readonly ThievingTarget Farmer = new()
    {
        Name = "Farmer",
        TargetType = ThievingTargetType.Npc,
        RequiredLevel = 10,
        Experience = 15, // OSRS gives 14.5
        BaseSuccessChance = 0.75,
        FailureDamage = 1,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 5, 20, 100)
        ]
    };

    /// <summary>
    ///     Guard - medium difficulty pickpocket.
    /// </summary>
    public static readonly ThievingTarget Guard = new()
    {
        Name = "Guard",
        TargetType = ThievingTargetType.Npc,
        RequiredLevel = 40,
        Experience = 47, // OSRS gives 46.8
        BaseSuccessChance = 0.55,
        FailureDamage = 2,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 20, 50, 100)
        ]
    };

    /// <summary>
    ///     Knight of Ardougne - high level target.
    /// </summary>
    public static readonly ThievingTarget Knight = new()
    {
        Name = "Knight",
        TargetType = ThievingTargetType.Npc,
        RequiredLevel = 55,
        Experience = 84, // OSRS gives 84.3
        BaseSuccessChance = 0.45,
        FailureDamage = 3,
        PossibleLoot =
        [
            new ThievingLoot(ItemIds.Coins, 30, 80, 100)
        ]
    };
}