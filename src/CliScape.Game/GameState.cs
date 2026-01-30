using CliScape.Content.Items;
using CliScape.Content.Locations.Towns;
using CliScape.Core.Combat;
using CliScape.Core.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;
using CliScape.Game.Persistence;

namespace CliScape.Game;

public class GameState
{
    public static readonly GameState Instance = new();
    private string? _saveFilePath;
    private IGameStateStore? _store;

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
                                  ?? throw new InvalidOperationException(
                                      "GameState has not been configured. Call Configure() first.");

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

        // Restore inventory
        var inventory = new Inventory();
        if (snapshot.Value.InventorySlots is not null)
        {
            foreach (var slotSnapshot in snapshot.Value.InventorySlots)
            {
                var item = ItemRegistry.GetById(new ItemId(slotSnapshot.ItemId));
                if (item is not null)
                {
                    inventory.TryAdd(item, slotSnapshot.Quantity);
                }
            }
        }

        // Restore equipment
        var equipment = new Equipment();
        if (snapshot.Value.EquippedItems is not null)
        {
            foreach (var equippedSnapshot in snapshot.Value.EquippedItems)
            {
                var item = ItemRegistry.GetById(new ItemId(equippedSnapshot.ItemId));
                if (item is IEquippable equippable)
                {
                    equipment.Equip(equippable);
                }
            }
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

            SkillCollection = skills,
            Inventory = inventory,
            Equipment = equipment
        };
    }

    public void Save()
    {
        var store = GetStore();
        var player = GetPlayer();

        var skillSnapshots = player.Skills
            .Select(skill => new SkillSnapshot(skill.Name.Name, skill.Level.Experience))
            .ToArray();

        // Create inventory snapshots
        var inventorySnapshots = player.Inventory.GetItems()
            .Select(i => new InventorySlotSnapshot(i.Item.Id.Value, i.Quantity))
            .ToArray();

        // Create equipment snapshots
        var equipmentSnapshots = player.Equipment.GetAllEquippedWithSlots()
            .Select(e => new EquippedItemSnapshot((int)e.Slot, e.Item.Id.Value))
            .ToArray();

        var snapshot = new PlayerSnapshot(
            player.Id,
            player.Name,
            player.CurrentLocation.Name.Value,
            player.CurrentHealth,
            player.MaximumHealth,
            skillSnapshots,
            inventorySnapshots,
            equipmentSnapshots);

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
        var player = new Player
        {
            Id = 0,
            Name = "Trauma Truck",
            CurrentLocation = GetCurrentLocation()
        };

        return player;
    }

    private IGameStateStore GetStore()
    {
        return _store ??
               throw new InvalidOperationException("GameState has not been configured. Call Configure() first.");
    }
}