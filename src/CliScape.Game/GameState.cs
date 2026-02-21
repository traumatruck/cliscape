using CliScape.Core.Achievements;
using CliScape.Core.ClueScrolls;
using CliScape.Core.Combat;
using CliScape.Core.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Slayer;
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

    private IItemRegistry? _itemRegistry;
    private string? _saveFilePath;
    private IGameStateStore? _store;

    private GameState()
    {
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
        return LocationRegistry.Instance.GetLocation(name);
    }

    /// <inheritdoc />
    public ILocation? GetLocation(LocationName name)
    {
        return LocationRegistry.Instance.GetLocation(name);
    }

    /// <inheritdoc />
    public IEnumerable<ILocation> GetAllLocations()
    {
        return LocationRegistry.Instance.GetAllLocations();
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
                var item = GetItemRegistry().GetById(new ItemId(slotSnapshot.ItemId));
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
                var item = GetItemRegistry().GetById(new ItemId(equippedSnapshot.ItemId));
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
                var item = GetItemRegistry().GetById(new ItemId(bankSlotSnapshot.ItemId));
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
                Category = new SlayerCategory(taskSnapshot.Category),
                RemainingKills = taskSnapshot.RemainingKills,
                TotalKills = taskSnapshot.TotalKills,
                SlayerMaster = new SlayerMasterName(taskSnapshot.SlayerMaster)
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
        var claimedDiaryRewards = new HashSet<DiaryRewardId>();
        if (snapshot.Value.ClaimedDiaryRewards is not null)
        {
            claimedDiaryRewards = new HashSet<DiaryRewardId>(
                snapshot.Value.ClaimedDiaryRewards.Select(r => new DiaryRewardId(r)));
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

            CurrentLocation = GetLocation(snapshot.Value.LocationName)
                              ?? GetCurrentLocation(),

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
                player.SlayerTask.Category.Value,
                player.SlayerTask.RemainingKills,
                player.SlayerTask.TotalKills,
                player.SlayerTask.SlayerMaster.Value);
        }

        // Create diary progress snapshots
        var diaryProgressSnapshots = player.DiaryProgress.GetAllProgress()
            .Select(p => new DiaryProgressSnapshot(
                p.Location.Value,
                p.CompletedAchievements.Select(a => a.Value).ToArray()))
            .ToArray();

        // Create claimed diary rewards snapshot
        var claimedRewardsSnapshot = player.ClaimedDiaryRewards.Select(r => r.Value).ToArray();

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
    ///     The default player spawn location name.
    /// </summary>
    public static readonly LocationName DefaultLocation = new("Lumbridge");

    /// <summary>
    ///     Configures the game state with the required dependencies.
    /// </summary>
    /// <param name="store">The game state store implementation.</param>
    /// <param name="saveFilePath">The path to the save file.</param>
    /// <param name="itemRegistry">The item registry for resolving items.</param>
    public void Configure(IGameStateStore store, string saveFilePath, IItemRegistry? itemRegistry = null)
    {
        _store = store;
        _saveFilePath = saveFilePath;
        _itemRegistry = itemRegistry;
    }

    private IItemRegistry GetItemRegistry()
    {
        return _itemRegistry ??
               throw new InvalidOperationException("GameState has not been configured with an IItemRegistry.");
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
        return GetLocation(DefaultLocation.Value) ?? throw new InvalidOperationException("Location not found.");
    }

    private Player CreateDefaultPlayer()
    {
        var inventory = new Inventory();
        var items = GetItemRegistry();

        // Add starter items like OSRS (IDs from Content.Items.ItemIds)
        inventory.TryAdd(items.GetById(new ItemId(1))!, 500);     // Coins
        inventory.TryAdd(items.GetById(new ItemId(101))!);        // Bronze Sword
        inventory.TryAdd(items.GetById(new ItemId(600))!);        // Wooden Shield
        inventory.TryAdd(items.GetById(new ItemId(601))!);        // Shortbow
        inventory.TryAdd(items.GetById(new ItemId(800))!, 50);    // Bronze Arrow
        inventory.TryAdd(items.GetById(new ItemId(105))!);        // Bronze Hatchet
        inventory.TryAdd(items.GetById(new ItemId(701))!);        // Bronze Pickaxe
        inventory.TryAdd(items.GetById(new ItemId(700))!);        // Tinderbox
        inventory.TryAdd(items.GetById(new ItemId(702))!);        // Small Fishing Net
        inventory.TryAdd(items.GetById(new ItemId(703))!);        // Hammer

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