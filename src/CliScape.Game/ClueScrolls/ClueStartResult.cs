using CliScape.Core.ClueScrolls;

namespace CliScape.Game.ClueScrolls;

/// <summary>
///     Result of attempting to start a new clue scroll.
/// </summary>
public abstract record ClueStartResult
{
    /// <summary>
    ///     A clue scroll was successfully started.
    /// </summary>
    public sealed record Started(ClueScroll Clue) : ClueStartResult;

    /// <summary>
    ///     The player already has an active clue scroll.
    /// </summary>
    public sealed record AlreadyHaveClue(ClueScrollTier CurrentTier) : ClueStartResult;
}