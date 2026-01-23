using CliScape.Content.Locations.Towns;
using CliScape.Core.Players;
using CliScape.Core.World;

namespace CliScape.Game;

public class GameState
{
    public static GameState Instance = new();

    private GameState()
    {
        LocationLibrary.LoadFrom(typeof(Lumbridge).Assembly);
    }

    private LocationLibrary LocationLibrary { get; } = new();

    private Player? Player { get; set; }

    /// <summary>
    ///     Loads the saved data.
    /// </summary>
    public void Load()
    {
        Player = new Player
        {
            Id = 0,
            Name = "Trauma Truck",
            CurrentLocation = GetCurrentLocation()
        };
    }

    public Player GetPlayer()
    {
        return Player ?? throw new InvalidOperationException("A player has not been loaded.");
    }

    private ILocation GetCurrentLocation()
    {
        return LocationLibrary.GetLocation(Lumbridge.Name) ??
               throw new InvalidOperationException("Location not found.");
    }
}