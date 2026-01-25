using CliScape.Content.Locations.Towns;
using CliScape.Core.Players;
using CliScape.Core.World;
using CliScape.Game.Persistence;

namespace CliScape.Game;

public class GameState
{
    public static readonly GameState Instance = new();

    private readonly IGameStateStore _store;

    private GameState()
    {
        LocationLibrary.LoadFrom(typeof(Lumbridge).Assembly);
        _store = new BinaryGameStateStore(GetSaveFilePath());
    }

    private LocationLibrary LocationLibrary { get; } = new();

    private Player? Player { get; set; }

    /// <summary>
    ///     Loads the saved data.
    /// </summary>
    public void Load()
    {
        var snapshot = _store.LoadPlayer();

        if (snapshot == null)
        {
            Player = CreateDefaultPlayer();
            Save();
            return;
        }

        Player = new Player
        {
            Id = snapshot.Value.Id,
            Name = snapshot.Value.Name,
            CurrentLocation = GetLocation(snapshot.Value.LocationName),
            Health = new PlayerHealth(snapshot.Value.CurrentHealth, snapshot.Value.MaxHealth)
        };
    }

    public void Save()
    {
        var player = GetPlayer();

        var snapshot = new PlayerSnapshot(
            player.Id,
            player.Name,
            player.CurrentLocation.Name.Value,
            player.Health.CurrentHealth,
            player.Health.MaxHealth);

        _store.SavePlayer(snapshot);
    }

    public ILocation? GetLocation(string name)
    {
        return LocationLibrary.GetLocation(new LocationName(name));
    }

    public Player GetPlayer()
    {
        return Player ?? throw new InvalidOperationException("A player has not been loaded.");
    }

    private ILocation GetCurrentLocation()
    {
        return GetLocation(Lumbridge.Name.Value);
    }

    private Player CreateDefaultPlayer()
    {
        return new Player
        {
            Id = 0,
            Name = "Trauma Truck",
            CurrentLocation = GetCurrentLocation()
        };
    }

    private static string GetSaveFilePath()
    {
        var root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CliScape");

        Directory.CreateDirectory(root);
        return Path.Combine(root, "save.bin");
    }
}
