namespace CliScape.Core.Npcs;

/// <summary>
///     Drop rarity categories matching OSRS conventions.
/// </summary>
public enum DropRarity
{
    /// <summary>
    ///     Always drops (100% chance).
    /// </summary>
    Always,

    /// <summary>
    ///     Common drop (~1/1 to 1/10).
    /// </summary>
    Common,

    /// <summary>
    ///     Uncommon drop (~1/10 to 1/50).
    /// </summary>
    Uncommon,

    /// <summary>
    ///     Rare drop (~1/50 to 1/200).
    /// </summary>
    Rare,

    /// <summary>
    ///     Very rare drop (~1/200 to 1/1000).
    /// </summary>
    VeryRare,

    /// <summary>
    ///     Custom drop rate specified by CustomRate.
    /// </summary>
    Custom
}