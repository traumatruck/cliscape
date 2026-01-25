using CliScape.Content.Locations.Towns;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
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

        var skills = new PlayerSkillCollection();

        // Restore skill experience from the snapshot
        foreach (var skillSnapshot in snapshot.Value.Skills)
        {
            var skill = skills.All.FirstOrDefault(s => s.Name.Name == skillSnapshot.Name);
            skill?.Level = PlayerSkillLevel.FromExperience(skillSnapshot.Experience);
        }

        Player = new Player
        {
            Id = snapshot.Value.Id,
            Name = snapshot.Value.Name,

            CurrentLocation = GetLocation(snapshot.Value.LocationName) ??
                              throw new InvalidOperationException("Location not found."),

            Health = new PlayerHealth
            {
                CurrentHealth = snapshot.Value.CurrentHealth,
                MaximumHealth = snapshot.Value.MaximumHealth
            },

            Skills = skills
        };
    }

    public void Save()
    {
        var player = GetPlayer();

        var skillSnapshots = player.Skills.All
            .Select(skill => new SkillSnapshot(skill.Name.Name, skill.Level.Experience))
            .ToArray();

        var snapshot = new PlayerSnapshot(
            player.Id,
            player.Name,
            player.CurrentLocation.Name.Value,
            player.CurrentHealth,
            player.MaximumHealth,
            skillSnapshots);

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
        return GetLocation(Lumbridge.Name.Value) ?? throw new InvalidOperationException("Location not found.");
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