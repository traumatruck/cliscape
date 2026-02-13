using CliScape.Cli;
using CliScape.Cli.Commands;
using CliScape.Cli.Commands.Bank;
using CliScape.Cli.Commands.Combat;
using CliScape.Cli.Commands.Diary;
using CliScape.Cli.Commands.Equipment;
using CliScape.Cli.Commands.Inventory;
using CliScape.Cli.Commands.Item;
using CliScape.Cli.Commands.Save;
using CliScape.Cli.Commands.Shop;
using CliScape.Cli.Commands.Skills;
using CliScape.Cli.Commands.Slayer;
using CliScape.Cli.Commands.Status;
using CliScape.Cli.Commands.Walk;
using CliScape.Content.Achievements;
using CliScape.Core;
using CliScape.Core.Combat;
using CliScape.Core.Events;
using CliScape.Core.Skills;
using CliScape.Game;
using CliScape.Game.Achievements;
using CliScape.Game.ClueScrolls;
using CliScape.Game.Combat;
using CliScape.Game.Items;
using CliScape.Game.Skills;
using CliScape.Game.Slayer;
using CliScape.Infrastructure.Configuration;
using CliScape.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

// Configure application settings
var settings = AppSettings.CreateDefault();

// Configure game state with infrastructure dependencies
var store = new BinaryGameStateStore(settings.Persistence);
GameState.Instance.Configure(store, settings.Persistence.SaveFilePath);

// Initialize the clue scroll system
ClueScrollWiring.Initialize();

// Register services in DI container
var services = new ServiceCollection();

// Core singletons
services.AddSingleton(GameState.Instance);
services.AddSingleton<IPlayerManager>(GameState.Instance);
services.AddSingleton<ILocationRegistry>(GameState.Instance);
services.AddSingleton<ICombatSessionManager>(GameState.Instance);
services.AddSingleton<IRandomProvider>(RandomProvider.Instance);
services.AddSingleton<IDomainEventDispatcher>(DomainEventDispatcher.Instance);
services.AddSingleton<ICombatCalculator>(CombatCalculator.Instance);
services.AddSingleton<IToolChecker>(ToolChecker.Instance);

// Game services
services.AddSingleton(CombatEngine.Instance);
services.AddSingleton<ICombatEngine>(CombatEngine.Instance);
services.AddSingleton(CombatSessionManager.Instance);
services.AddSingleton(LocationRegistry.Instance);
services.AddSingleton(FishingService.Instance);
services.AddSingleton(WoodcuttingService.Instance);
services.AddSingleton(MiningService.Instance);
services.AddSingleton(SmeltingService.Instance);
services.AddSingleton(CookingService.Instance);
services.AddSingleton(FiremakingService.Instance);
services.AddSingleton(EquipmentService.Instance);
services.AddSingleton(LootService.Instance);
services.AddSingleton(SlayerService.Instance);
services.AddSingleton(DiaryService.Instance);
services.AddSingleton(DiaryRewardService.Instance);
services.AddSingleton(DiaryRegistry.Instance);

if (ClueScrollService.Instance is not null)
    services.AddSingleton(ClueScrollService.Instance);

// Wire domain event subscribers
var events = DomainEventDispatcher.Instance;

events.Register<LevelUpEvent>(e =>
    AnsiConsole.MarkupLine(
        $"[bold yellow]Congratulations! Your {e.SkillName.Name} level is now {e.NewLevel}![/]"));

events.Register<PlayerDiedEvent>(_ =>
    AnsiConsole.MarkupLine(MessageConstants.DeathMessage));

events.Register<SlayerTaskCompletedEvent>(e =>
    AnsiConsole.MarkupLine(
        $"[bold green]You have completed your slayer task! ({e.TotalKills} {e.Category} killed)[/]"));

events.Register<ClueScrollCompletedEvent>(e =>
    AnsiConsole.MarkupLine(
        $"[bold gold1]You have completed a {e.Tier} clue scroll![/]"));

events.Register<AchievementCompletedEvent>(e =>
    AnsiConsole.MarkupLine(
        $"[bold green]Achievement complete: {e.AchievementName}![/]"));

events.Register<DiaryTierCompletedEvent>(e =>
    AnsiConsole.MarkupLine(
        $"[bold green]{e.Tier} {e.Location.Value} diary complete![/]"));

var registrar = new TypeRegistrar(services);

var app = new CommandApp(registrar);

app.Configure(configuration =>
{
    configuration.SetInterceptor(new CommandInterceptor());

    configuration.AddCommand<StatusCommand>(StatusCommand.CommandName)
        .WithDescription("View your character status");

    // Skills command branch - skill listing and training
    configuration.AddBranch("skills", skills =>
    {
        skills.AddCommand<SkillsListCommand>(SkillsListCommand.CommandName);
        skills.AddCommand<FishCommand>(FishCommand.CommandName);
        skills.AddCommand<ChopCommand>(ChopCommand.CommandName);
        skills.AddCommand<MineCommand>(MineCommand.CommandName);
        skills.AddCommand<SmeltCommand>(SmeltCommand.CommandName);
        skills.AddCommand<SmithCommand>(SmithCommand.CommandName);
        skills.AddCommand<CookCommand>(CookCommand.CommandName);
        skills.AddCommand<ThieveCommand>(ThieveCommand.CommandName);

        skills.SetDefaultCommand<SkillsListCommand>();
        skills.SetDescription("Perform skill related actions");
    });

    configuration.AddCommand<WalkCommand>(WalkCommand.CommandName)
        .WithDescription("Travel to a new location");

    configuration.AddBranch("save", save =>
    {
        save.AddCommand<SaveDeleteCommand>(SaveDeleteCommand.CommandName);
        save.SetDescription("Manage your save");
    });

    configuration.AddBranch("combat", combat =>
    {
        combat.AddCommand<CombatListCommand>(CombatListCommand.CommandName);
        combat.AddCommand<CombatAttackCommand>(CombatAttackCommand.CommandName);
        combat.AddCommand<CombatLootCommand>(CombatLootCommand.CommandName);

        combat.SetDefaultCommand<CombatListCommand>();
        combat.SetDescription("Manage combat encounters");
    });

    configuration.AddBranch("inventory", inventory =>
    {
        inventory.AddCommand<InventoryListCommand>(InventoryListCommand.CommandName);
        inventory.AddCommand<InventoryDropCommand>(InventoryDropCommand.CommandName);
        inventory.AddCommand<InventorySortCommand>(InventorySortCommand.CommandName);

        inventory.SetDefaultCommand<InventoryListCommand>();
        inventory.SetDescription("View and manage your inventory");
    }).WithAlias("inv");

    configuration.AddCommand<ItemCommand>(ItemCommand.CommandName)
        .WithDescription("Interact with items in your inventory");

    configuration.AddBranch("shop", shop =>
    {
        shop.AddCommand<ShopListCommand>(ShopListCommand.CommandName);
        shop.AddCommand<ShopViewCommand>(ShopViewCommand.CommandName);
        shop.AddCommand<ShopBuyCommand>(ShopBuyCommand.CommandName);
        shop.AddCommand<ShopSellCommand>(ShopSellCommand.CommandName);
        shop.AddCommand<ShopValueCommand>(ShopValueCommand.CommandName);

        shop.SetDefaultCommand<ShopListCommand>();
        shop.SetDescription("Browse and trade with shops");
    });

    configuration.AddBranch("equipment", equipment =>
    {
        equipment.AddCommand<EquipmentViewCommand>(EquipmentViewCommand.CommandName);
        equipment.AddCommand<UnequipCommand>(UnequipCommand.CommandName);

        equipment.SetDefaultCommand<EquipmentViewCommand>();
        equipment.SetDescription("View and manage equipped items");
    });

    configuration.AddCommand<SlayerCommand>(SlayerCommand.CommandName)
        .WithDescription("Get or manage slayer tasks");

    configuration.AddBranch("diary", diary =>
    {
        diary.AddCommand<DiaryListCommand>(DiaryListCommand.CommandName);
        diary.AddCommand<DiaryViewCommand>(DiaryViewCommand.CommandName);
        diary.AddCommand<DiaryClaimCommand>(DiaryClaimCommand.CommandName);

        diary.SetDefaultCommand<DiaryListCommand>();
        diary.SetDescription("View and claim achievement diary rewards");
    });

    configuration.AddBranch("bank", bank =>
    {
        bank.AddCommand<BankViewCommand>(BankViewCommand.CommandName);
        bank.AddCommand<BankDepositCommand>(BankDepositCommand.CommandName);
        bank.AddCommand<BankWithdrawCommand>(BankWithdrawCommand.CommandName);

        bank.SetDefaultCommand<BankViewCommand>();
        bank.SetDescription("Access your bank to store and retrieve items");
    });
});

app.Run(args);