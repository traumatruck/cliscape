using CliScape.Content.Items;
using CliScape.Core.World.Resources;

namespace CliScape.Content.Resources;

/// <summary>
///     Mining rock definitions for the game world.
/// </summary>
public static class MiningRocks
{
    /// <summary>
    ///     Copper rock requiring level 1 mining.
    /// </summary>
    public static readonly MiningRock CopperRock = new()
    {
        Name = "Copper rock",
        RockType = RockType.Copper,
        OreItemId = ItemIds.CopperOre,
        RequiredLevel = 1,
        Experience = 18, // OSRS gives 17.5
        RequiredPickaxe = PickaxeTier.Bronze,
        MaxActions = 3
    };

    /// <summary>
    ///     Tin rock requiring level 1 mining.
    /// </summary>
    public static readonly MiningRock TinRock = new()
    {
        Name = "Tin rock",
        RockType = RockType.Tin,
        OreItemId = ItemIds.TinOre,
        RequiredLevel = 1,
        Experience = 18, // OSRS gives 17.5
        RequiredPickaxe = PickaxeTier.Bronze,
        MaxActions = 3
    };

    /// <summary>
    ///     Iron rock requiring level 15 mining.
    /// </summary>
    public static readonly MiningRock IronRock = new()
    {
        Name = "Iron rock",
        RockType = RockType.Iron,
        OreItemId = ItemIds.IronOre,
        RequiredLevel = 15,
        Experience = 35,
        RequiredPickaxe = PickaxeTier.Bronze,
        MaxActions = 2
    };

    /// <summary>
    ///     Coal rock requiring level 30 mining.
    /// </summary>
    public static readonly MiningRock CoalRock = new()
    {
        Name = "Coal rock",
        RockType = RockType.Coal,
        OreItemId = ItemIds.Coal,
        RequiredLevel = 30,
        Experience = 50,
        RequiredPickaxe = PickaxeTier.Iron,
        MaxActions = 3
    };

    /// <summary>
    ///     Mithril rock requiring level 55 mining.
    /// </summary>
    public static readonly MiningRock MithrilRock = new()
    {
        Name = "Mithril rock",
        RockType = RockType.Mithril,
        OreItemId = ItemIds.MithrilOre,
        RequiredLevel = 55,
        Experience = 80,
        RequiredPickaxe = PickaxeTier.Steel,
        MaxActions = 2
    };

    /// <summary>
    ///     Adamantite rock requiring level 70 mining.
    /// </summary>
    public static readonly MiningRock AdamantiteRock = new()
    {
        Name = "Adamantite rock",
        RockType = RockType.Adamantite,
        OreItemId = ItemIds.AdamantiteOre,
        RequiredLevel = 70,
        Experience = 95,
        RequiredPickaxe = PickaxeTier.Mithril,
        MaxActions = 2
    };

    /// <summary>
    ///     Runite rock requiring level 85 mining.
    /// </summary>
    public static readonly MiningRock RuniteRock = new()
    {
        Name = "Runite rock",
        RockType = RockType.Runite,
        OreItemId = ItemIds.RuniteOre,
        RequiredLevel = 85,
        Experience = 125,
        RequiredPickaxe = PickaxeTier.Adamant,
        MaxActions = 1
    };
}