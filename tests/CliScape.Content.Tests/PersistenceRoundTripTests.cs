using CliScape.Game.Persistence;
using CliScape.Infrastructure.Configuration;
using CliScape.Infrastructure.Persistence;

namespace CliScape.Content.Tests;

public class PersistenceRoundTripTests : IDisposable
{
    private readonly string _tempDir;
    private readonly BinaryGameStateStore _store;

    public PersistenceRoundTripTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"cliscape-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);
        var settings = new PersistenceSettings { SaveDirectory = _tempDir };
        _store = new BinaryGameStateStore(settings);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, true);
        }
    }

    [Fact]
    public void SaveAndLoad_BasicPlayer_RoundTrips()
    {
        var snapshot = new PlayerSnapshot(
            Id: 1,
            Name: "TestHero",
            LocationName: "Lumbridge",
            CurrentHealth: 10,
            MaximumHealth: 10,
            Skills: new[]
            {
                new SkillSnapshot("Attack", 0),
                new SkillSnapshot("Hitpoints", 1154)
            },
            InventorySlots: null,
            EquippedItems: null,
            BankSlots: null,
            SlayerTask: null,
            DiaryProgress: null,
            ClaimedDiaryRewards: null,
            ActiveClue: null
        );

        _store.SavePlayer(snapshot);
        var loaded = _store.LoadPlayer();

        Assert.NotNull(loaded);
        Assert.Equal(1, loaded.Value.Id);
        Assert.Equal("TestHero", loaded.Value.Name);
        Assert.Equal("Lumbridge", loaded.Value.LocationName);
        Assert.Equal(10, loaded.Value.CurrentHealth);
        Assert.Equal(10, loaded.Value.MaximumHealth);
        Assert.Equal(2, loaded.Value.Skills.Length);
        Assert.Equal("Attack", loaded.Value.Skills[0].Name);
        Assert.Equal(0, loaded.Value.Skills[0].Experience);
        Assert.Equal("Hitpoints", loaded.Value.Skills[1].Name);
        Assert.Equal(1154, loaded.Value.Skills[1].Experience);
    }

    [Fact]
    public void SaveAndLoad_WithInventory_RoundTrips()
    {
        var snapshot = new PlayerSnapshot(
            Id: 1,
            Name: "TestHero",
            LocationName: "Lumbridge",
            CurrentHealth: 10,
            MaximumHealth: 10,
            Skills: new[] { new SkillSnapshot("Attack", 0) },
            InventorySlots: new[]
            {
                new InventorySlotSnapshot(0, 100, 1),
                new InventorySlotSnapshot(3, 200, 5)
            },
            EquippedItems: null,
            BankSlots: null,
            SlayerTask: null,
            DiaryProgress: null,
            ClaimedDiaryRewards: null,
            ActiveClue: null
        );

        _store.SavePlayer(snapshot);
        var loaded = _store.LoadPlayer();

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Value.InventorySlots);
        Assert.Equal(2, loaded.Value.InventorySlots.Length);
        Assert.Equal(0, loaded.Value.InventorySlots[0].SlotIndex);
        Assert.Equal(100, loaded.Value.InventorySlots[0].ItemId);
        Assert.Equal(1, loaded.Value.InventorySlots[0].Quantity);
        Assert.Equal(3, loaded.Value.InventorySlots[1].SlotIndex);
        Assert.Equal(200, loaded.Value.InventorySlots[1].ItemId);
        Assert.Equal(5, loaded.Value.InventorySlots[1].Quantity);
    }

    [Fact]
    public void SaveAndLoad_WithEquipment_RoundTrips()
    {
        var snapshot = new PlayerSnapshot(
            Id: 1,
            Name: "TestHero",
            LocationName: "Lumbridge",
            CurrentHealth: 10,
            MaximumHealth: 10,
            Skills: new[] { new SkillSnapshot("Attack", 0) },
            InventorySlots: null,
            EquippedItems: new[]
            {
                new EquippedItemSnapshot(0, 500), // Head slot
                new EquippedItemSnapshot(4, 600) // Weapon slot
            },
            BankSlots: null,
            SlayerTask: null,
            DiaryProgress: null,
            ClaimedDiaryRewards: null,
            ActiveClue: null
        );

        _store.SavePlayer(snapshot);
        var loaded = _store.LoadPlayer();

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Value.EquippedItems);
        Assert.Equal(2, loaded.Value.EquippedItems.Length);
        Assert.Equal(0, loaded.Value.EquippedItems[0].Slot);
        Assert.Equal(500, loaded.Value.EquippedItems[0].ItemId);
    }

    [Fact]
    public void SaveAndLoad_WithSlayerTask_RoundTrips()
    {
        var snapshot = new PlayerSnapshot(
            Id: 1,
            Name: "TestHero",
            LocationName: "Lumbridge",
            CurrentHealth: 10,
            MaximumHealth: 10,
            Skills: new[] { new SkillSnapshot("Attack", 0) },
            InventorySlots: null,
            EquippedItems: null,
            BankSlots: null,
            SlayerTask: new SlayerTaskSnapshot("Goblins", 15, 40, "Turael"),
            DiaryProgress: null,
            ClaimedDiaryRewards: null,
            ActiveClue: null
        );

        _store.SavePlayer(snapshot);
        var loaded = _store.LoadPlayer();

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Value.SlayerTask);
        Assert.Equal("Goblins", loaded.Value.SlayerTask.Value.Category);
        Assert.Equal(15, loaded.Value.SlayerTask.Value.RemainingKills);
        Assert.Equal(40, loaded.Value.SlayerTask.Value.TotalKills);
        Assert.Equal("Turael", loaded.Value.SlayerTask.Value.SlayerMaster);
    }

    [Fact]
    public void SaveAndLoad_WithBankSlots_RoundTrips()
    {
        var snapshot = new PlayerSnapshot(
            Id: 1,
            Name: "TestHero",
            LocationName: "Lumbridge",
            CurrentHealth: 10,
            MaximumHealth: 10,
            Skills: new[] { new SkillSnapshot("Attack", 0) },
            InventorySlots: null,
            EquippedItems: null,
            BankSlots: new[]
            {
                new BankSlotSnapshot(0, 995, 10000),
                new BankSlotSnapshot(1, 100, 50)
            },
            SlayerTask: null,
            DiaryProgress: null,
            ClaimedDiaryRewards: null,
            ActiveClue: null
        );

        _store.SavePlayer(snapshot);
        var loaded = _store.LoadPlayer();

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Value.BankSlots);
        Assert.Equal(2, loaded.Value.BankSlots.Length);
        Assert.Equal(995, loaded.Value.BankSlots[0].ItemId);
        Assert.Equal(10000, loaded.Value.BankSlots[0].Quantity);
    }

    [Fact]
    public void SaveAndLoad_WithDiaryProgress_RoundTrips()
    {
        var snapshot = new PlayerSnapshot(
            Id: 1,
            Name: "TestHero",
            LocationName: "Lumbridge",
            CurrentHealth: 10,
            MaximumHealth: 10,
            Skills: new[] { new SkillSnapshot("Attack", 0) },
            InventorySlots: null,
            EquippedItems: null,
            BankSlots: null,
            SlayerTask: null,
            DiaryProgress: new[]
            {
                new DiaryProgressSnapshot("Lumbridge", new[] { "task1", "task2" })
            },
            ClaimedDiaryRewards: new[] { "reward1" },
            ActiveClue: null
        );

        _store.SavePlayer(snapshot);
        var loaded = _store.LoadPlayer();

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Value.DiaryProgress);
        Assert.Single(loaded.Value.DiaryProgress);
        Assert.Equal("Lumbridge", loaded.Value.DiaryProgress[0].LocationName);
        Assert.Equal(2, loaded.Value.DiaryProgress[0].CompletedAchievementIds.Length);
        Assert.NotNull(loaded.Value.ClaimedDiaryRewards);
        Assert.Single(loaded.Value.ClaimedDiaryRewards);
        Assert.Equal("reward1", loaded.Value.ClaimedDiaryRewards[0]);
    }

    [Fact]
    public void SaveAndLoad_WithActiveClue_RoundTrips()
    {
        var snapshot = new PlayerSnapshot(
            Id: 1,
            Name: "TestHero",
            LocationName: "Lumbridge",
            CurrentHealth: 10,
            MaximumHealth: 10,
            Skills: new[] { new SkillSnapshot("Attack", 0) },
            InventorySlots: null,
            EquippedItems: null,
            BankSlots: null,
            SlayerTask: null,
            DiaryProgress: null,
            ClaimedDiaryRewards: null,
            ActiveClue: new ClueScrollSnapshot(
                "Easy",
                new[]
                {
                    new ClueStepSnapshot("Dig", "Dig near the castle", "Lumbridge", "You found something!")
                },
                0
            )
        );

        _store.SavePlayer(snapshot);
        var loaded = _store.LoadPlayer();

        Assert.NotNull(loaded);
        Assert.NotNull(loaded.Value.ActiveClue);
        Assert.Equal("Easy", loaded.Value.ActiveClue.Value.Tier);
        Assert.Single(loaded.Value.ActiveClue.Value.Steps);
        Assert.Equal("Dig", loaded.Value.ActiveClue.Value.Steps[0].StepType);
        Assert.Equal("Dig near the castle", loaded.Value.ActiveClue.Value.Steps[0].HintText);
        Assert.Equal(0, loaded.Value.ActiveClue.Value.CurrentStepIndex);
    }

    [Fact]
    public void LoadPlayer_NoSaveFile_ReturnsNull()
    {
        var loaded = _store.LoadPlayer();

        Assert.Null(loaded);
    }

    [Fact]
    public void SaveAndLoad_OverwritesSavedData()
    {
        var first = new PlayerSnapshot(
            Id: 1,
            Name: "First",
            LocationName: "Lumbridge",
            CurrentHealth: 10,
            MaximumHealth: 10,
            Skills: new[] { new SkillSnapshot("Attack", 0) },
            InventorySlots: null,
            EquippedItems: null,
            BankSlots: null,
            SlayerTask: null,
            DiaryProgress: null,
            ClaimedDiaryRewards: null,
            ActiveClue: null
        );

        var second = new PlayerSnapshot(
            Id: 2,
            Name: "Second",
            LocationName: "Varrock",
            CurrentHealth: 50,
            MaximumHealth: 99,
            Skills: new[] { new SkillSnapshot("Attack", 5000) },
            InventorySlots: null,
            EquippedItems: null,
            BankSlots: null,
            SlayerTask: null,
            DiaryProgress: null,
            ClaimedDiaryRewards: null,
            ActiveClue: null
        );

        _store.SavePlayer(first);
        _store.SavePlayer(second);
        var loaded = _store.LoadPlayer();

        Assert.NotNull(loaded);
        Assert.Equal("Second", loaded.Value.Name);
        Assert.Equal("Varrock", loaded.Value.LocationName);
        Assert.Equal(50, loaded.Value.CurrentHealth);
    }
}
