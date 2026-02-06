using CliScape.Content.Items;
using CliScape.Content.Locations.Towns;
using CliScape.Core.Achievements;
using CliScape.Core.ClueScrolls;
using CliScape.Core.Combat;
using CliScape.Core.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;
using CliScape.Game.Persistence;

namespace CliScape.Game;

/// <summary>
///     Central game state facade providing access to player, location, and combat management.
///     Implements focused interfaces and delegates to specialized managers.
/// </summary>
public class GameState : IPlayerManager, ILocationRegistry, ICombatSessionManager
{
    public static readonly GameState Instance = new();
    private readonly CombatSessionManager _combatSessionManager;

    private readonly LocationRegistry _locationRegistry;
    private string? _saveFilePath;
    private IGameStateStore? _store;

    private GameState()
    {
        _locationRegistry = LocationRegistry.Instance;
        _combatSessionManager = CombatSessionManager.Instance;
    }

    private Player? Player { get; set; }

    /// <summary>
    ///     Gets the path to the save file. Must be configured before use.
    /// </summary>
    public string SaveFilePath => _saveFilePath
                                  ?? throw new InvalidOperationException(
                                      "GameState has not been configured. Call Configure() first.");

    /// <inheritdoc />
    public CombatSession? CurrentCombat => _combatSessionManager.CurrentCombat;

    /// <inheritdoc />
    public bool IsInCombat => _combatSessionManager.IsInCombat;

    /// <inheritdoc />
    public PendingLoot PendingLoot => _combatSessionManager.PendingLoot;

    /// <inheritdoc />
    public CombatSession StartCombat(Player player, ICombatableNpc npc)
    {
        return _combatSessionManager.StartCombat(player, npc);
    }

    /// <inheritdoc />
    public void EndCombat()
    {
        _combatSessionManager.EndCombat();
    }

    /// <inheritdoc />
    public ILocation? GetLocation(string name)
    {
        return _locationRegistry.GetLocation(name);
    }

    /// <inheritdoc />
    public ILocation? GetLocation(LocationName name)
    {
        return _locationRegistry.GetLocation(name);
    }

    /// <inheritdoc />
    public IEnumerable<ILocation> GetAllLocations()
    {
        return _locationRegistry.GetAllLocations();
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
                    inventory.TrySetSlot(slotSnapshot.SlotIndex, item, slotSnapshot.Quantity);
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

        // Restore bank
        var bank = new Bank();
        if (snapshot.Value.BankSlots is not null)
        {
            foreach (var bankSlotSnapshot in snapshot.Value.BankSlots)
            {
                var item = ItemRegistry.GetById(new ItemId(bankSlotSnapshot.ItemId));
                if (item is not null)
                {
                    bank.TrySetSlot(bankSlotSnapshot.SlotIndex, item, bankSlotSnapshot.Quantity);
                }
            }
        }

        // Restore slayer task
        SlayerTask? slayerTask = null;
        if (snapshot.Value.SlayerTask is not null)
        {
            var taskSnapshot = snapshot.Value.SlayerTask.Value;
            slayerTask = new SlayerTask
            {
                Category = taskSnapshot.Category,
                RemainingKills = taskSnapshot.RemainingKills,
                TotalKills = taskSnapshot.TotalKills,
                SlayerMaster = taskSnapshot.SlayerMaster
            };
        }

        // Restore diary progress
        var diaryProgressCollection = new DiaryProgressCollection();
        if (snapshot.Value.DiaryProgress is not null)
        {
            var progressList = new List<DiaryProgress>();
            foreach (var progressSnapshot in snapshot.Value.DiaryProgress)
            {
                var location = new LocationName(progressSnapshot.LocationName);
                var achievementIds = progressSnapshot.CompletedAchievementIds
                    .Select(id => new AchievementId(id));
                progressList.Add(new DiaryProgress(location, achievementIds));
            }

            diaryProgressCollection = new DiaryProgressCollection(progressList);
        }

        // Restore claimed diary rewards
        var claimedDiaryRewards = new HashSet<string>();
        if (snapshot.Value.ClaimedDiaryRewards is not null)
        {
            claimedDiaryRewards = new HashSet<string>(snapshot.Value.ClaimedDiaryRewards);
        }

        // Restore active clue scroll
        ClueScroll? activeClue = null;
        if (snapshot.Value.ActiveClue is not null)
        {
            var clueSnapshot = snapshot.Value.ActiveClue.Value;
            var steps = clueSnapshot.Steps.Select(s => new ClueStep
            {
                StepType = Enum.Parse<ClueStepType>(s.StepType),
                HintText = s.HintText,
                RequiredLocation = new LocationName(s.RequiredLocation),
                CompletionText = s.CompletionText
            }).ToArray();

            activeClue = new ClueScroll
            {
                Tier = Enum.Parse<ClueScrollTier>(clueSnapshot.Tier),
                Steps = steps,
                CurrentStepIndex = clueSnapshot.CurrentStepIndex
            };
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
            Equipment = equipment,
            Bank = bank,
            SlayerTask = slayerTask,
            DiaryProgress = diaryProgressCollection,
            ClaimedDiaryRewards = claimedDiaryRewards,
            ActiveClue = activeClue
        };

        Player.SetPrayerPoints(Player.MaximumPrayerPoints);
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
            .Select(i => new InventorySlotSnapshot(i.SlotIndex, i.Item.Id.Value, i.Quantity))
            .ToArray();

        // Create equipment snapshots
        var equipmentSnapshots = player.Equipment.GetAllEquippedWithSlots()
            .Select(e => new EquippedItemSnapshot((int)e.Slot, e.Item.Id.Value))
            .ToArray();

        // Create bank snapshots
        var bankSnapshots = player.Bank.GetItems()
            .Select(i => new BankSlotSnapshot(i.SlotIndex, i.Item.Id.Value, i.Quantity))
            .ToArray();

        // Create slayer task snapshot
        SlayerTaskSnapshot? slayerTaskSnapshot = null;
        if (player.SlayerTask != null)
        {
            slayerTaskSnapshot = new SlayerTaskSnapshot(
                player.SlayerTask.Category,
                player.SlayerTask.RemainingKills,
                player.SlayerTask.TotalKills,
                player.SlayerTask.SlayerMaster);
        }

        // Create diary progress snapshots
        var diaryProgressSnapshots = player.DiaryProgress.GetAllProgress()
            .Select(p => new DiaryProgressSnapshot(
                p.Location.Value,
                p.CompletedAchievements.Select(a => a.Value).ToArray()))
            .ToArray();

        // Create claimed diary rewards snapshot
        var claimedRewardsSnapshot = player.ClaimedDiaryRewards.ToArray();

        // Create active clue scroll snapshot
        ClueScrollSnapshot? activeClueSnapshot = null;
        if (player.ActiveClue != null)
        {
            var clue = player.ActiveClue;
            var stepSnapshots = clue.Steps.Select(s => new ClueStepSnapshot(
                s.StepType.ToString(),
                s.HintText,
                s.RequiredLocation.Value,
                s.CompletionText)).ToArray();

            activeClueSnapshot = new ClueScrollSnapshot(
                clue.Tier.ToString(),
                stepSnapshots,
                clue.CurrentStepIndex);
        }

        var snapshot = new PlayerSnapshot(
            player.Id,
            player.Name,
            player.CurrentLocation.Name.Value,
            player.CurrentHealth,
            player.MaximumHealth,
            skillSnapshots,
            inventorySnapshots,
            equipmentSnapshots,
            bankSnapshots,
            slayerTaskSnapshot,
            diaryProgressSnapshots,
            claimedRewardsSnapshot,
            activeClueSnapshot);

        store.SavePlayer(snapshot);
    }

    /// <inheritdoc />
    public Player GetPlayer()
    {
        return Player ?? throw new InvalidOperationException("A player has not been loaded.");
    }

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
    ///     Start a combat session with the specified NPC using the current player.
    /// </summary>
    public CombatSession StartCombat(ICombatableNpc npc)
    {
        var player = GetPlayer();
        return _combatSessionManager.StartCombat(player, npc);
    }

    private ILocation GetCurrentLocation()
    {
        return GetLocation(Lumbridge.Name.Value) ?? throw new InvalidOperationException("Location not found.");
    }

    private Player CreateDefaultPlayer()
    {
        var inventory = new Inventory();

        // Add starter items like OSRS
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.Coins)!, 500);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.BronzeSword)!);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.WoodenShield)!);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.Shortbow)!);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.BronzeArrow)!, 50);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.BronzeHatchet)!);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.BronzePickaxe)!);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.Tinderbox)!);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.SmallFishingNet)!);
        inventory.TryAdd(ItemRegistry.GetById(ItemIds.Hammer)!);

        var player = new Player
        {
            Id = 0,
            Name = "Trauma Truck",
            CurrentLocation = GetCurrentLocation(),
            Inventory = inventory
        };

        player.SetPrayerPoints(player.MaximumPrayerPoints);

        return player;
    }

    private IGameStateStore GetStore()
    {
        return _store ??
               throw new InvalidOperationException("GameState has not been configured. Call Configure() first.");
    }
}