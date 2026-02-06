using CliScape.Core.ClueScrolls;
using CliScape.Core.Players;

namespace CliScape.Core.Items.Actions;

/// <summary>
///     Static delegates for clue scroll item actions. Wired at startup by the Game layer
///     so that Content-layer items can invoke game services without a direct dependency.
/// </summary>
public static class ClueScrollActions
{
    /// <summary>
    ///     Delegate invoked when a player uses a clue scroll item.
    ///     Should start the clue trail or attempt the current step.
    ///     Parameters: item, player, tier. Returns the display message.
    /// </summary>
    public static Func<IItem, Player, ClueScrollTier, string>? OnUseClueScroll { get; set; }

    /// <summary>
    ///     Delegate invoked when a player uses (opens) a reward casket.
    ///     Parameters: item, player, tier. Returns the display message.
    /// </summary>
    public static Func<IItem, Player, ClueScrollTier, string>? OnOpenRewardCasket { get; set; }
}