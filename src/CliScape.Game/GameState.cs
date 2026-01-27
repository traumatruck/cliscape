using CliScape.Content.Locations.Towns;
using CliScape.Core.Combat;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;
using CliScape.Game.Persistence;

namespace CliScape.Game;

public class GameState
{
    public static readonly GameState Instance = new();

    private IGameStateStore? _store;
    private string? _saveFilePath;

    private GameState()
    {
        LocationLibrary.LoadFrom(typeof(Lumbridge).Assembly);
    }

    private LocationLibrary LocationLibrary { get; } = new();

    private Player? Player { get; set; }

    /// <summary>
    ///     The current active combat session, if any.
    /// </summary>
    public CombatSession? CurrentCombat { get; private set; }

    /// <summary>
    ///     Whether the player is currently in combat.
    /// </summary>
    public bool IsInCombat => CurrentCombat is { IsComplete: false };

    /// <summary>
    ///     Gets the path to the save file. Must be configured before use.
    /// </summary>
    public string SaveFilePath => _saveFilePath 
        ?? throw new InvalidOperationException("GameState has not been configured. Call Configure() first.");

    /// <summary>
    ///     Configures the game state with the required dependencies.
    /// </summary>
    /// <param name="store">The game state store implementation.</param>
    /// <param name="saveFilePath">The path to the save file.</param>
    public void Configure(IGameStateStore store, string saveFilePath)
    {
        _store = store;
        _saveFilePath = saveFilePath;
    }

    /// <summary>
    ///     Start a combat session with the specified NPC.
    /// </summary>
    public CombatSession StartCombat(ICombatableNpc npc)
    {
        var player = GetPlayer();
        CurrentCombat = new CombatSession(player, npc);
        return CurrentCombat;
    }

    /// <summary>
    ///     End the current combat session.
    /// </summary>
    public void EndCombat()
    {
        CurrentCombat = null;
    }

    /// <summary>
    ///     Loads the saved data.
    /// </summary>
    public void Load()
    {
        var store = GetStore();
        var snapshot = store.LoadPlayer();

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

            SkillCollection = skills
        };
    }

    public void Save()
    {
        var store = GetStore();
        var player = GetPlayer();

        var skillSnapshots = player.Skills
            .Select(skill => new SkillSnapshot(skill.Name.Name, skill.Level.Experience))
            .ToArray();

        var snapshot = new PlayerSnapshot(
            player.Id,
            player.Name,
            player.CurrentLocation.Name.Value,
            player.CurrentHealth,
            player.MaximumHealth,
            skillSnapshots);

        store.SavePlayer(snapshot);
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

    private IGameStateStore GetStore()
    {
        return _store ?? throw new InvalidOperationException("GameState has not been configured. Call Configure() first.");
    }
}