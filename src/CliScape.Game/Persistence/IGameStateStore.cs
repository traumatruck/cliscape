namespace CliScape.Game.Persistence;

/// <summary>
///     Defines the contract for storing and retrieving game state data.
/// </summary>
public interface IGameStateStore
{
    /// <summary>
    ///     Loads the player's saved game data.
    /// </summary>
    /// <returns>
    ///     A <see cref="PlayerSnapshot" /> containing the player's saved state,
    ///     or <c>null</c> if no saved data exists.
    /// </returns>
    PlayerSnapshot? LoadPlayer();

    /// <summary>
    ///     Saves the current state of the player to persistent storage.
    /// </summary>
    /// <param name="snapshot">The player data to save.</param>
    void SavePlayer(PlayerSnapshot snapshot);
}