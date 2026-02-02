using CliScape.Core.Players;

namespace CliScape.Game;

/// <summary>
///     Manages player state and persistence.
/// </summary>
public interface IPlayerManager
{
    /// <summary>
    ///     Gets the current player.
    /// </summary>
    /// <returns>The player.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no player has been loaded.</exception>
    Player GetPlayer();

    /// <summary>
    ///     Loads the player from persistence.
    /// </summary>
    void Load();

    /// <summary>
    ///     Saves the player to persistence.
    /// </summary>
    void Save();
}