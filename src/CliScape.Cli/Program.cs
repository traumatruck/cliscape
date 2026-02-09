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
using CliScape.Game;
using CliScape.Game.ClueScrolls;
using CliScape.Infrastructure.Configuration;
using CliScape.Infrastructure.Persistence;
using Spectre.Console.Cli;

// Configure application settings
var settings = AppSettings.CreateDefault();

// Configure game state with infrastructure dependencies
var store = new BinaryGameStateStore(settings.Persistence);
GameState.Instance.Configure(store, settings.Persistence.SaveFilePath);

// Initialize the clue scroll system
ClueScrollWiring.Initialize();

var app = new CommandApp();

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