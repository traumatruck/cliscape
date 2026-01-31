using CliScape.Cli.Commands;
using CliScape.Cli.Commands.Combat;
using CliScape.Cli.Commands.Equipment;
using CliScape.Cli.Commands.Inventory;
using CliScape.Cli.Commands.Item;
using CliScape.Cli.Commands.Save;
using CliScape.Cli.Commands.Shop;
using CliScape.Cli.Commands.Status;
using CliScape.Cli.Commands.Walk;
using CliScape.Game;
using CliScape.Infrastructure.Configuration;
using CliScape.Infrastructure.Persistence;
using Spectre.Console.Cli;

// Configure application settings
var settings = AppSettings.CreateDefault();

// Configure game state with infrastructure dependencies
var store = new BinaryGameStateStore(settings.Persistence);
GameState.Instance.Configure(store, settings.Persistence.SaveFilePath);

var app = new CommandApp();

app.Configure(configuration =>
{
    configuration.SetInterceptor(new CommandInterceptor());
    
    configuration.AddBranch("status", status =>
    {
        status.AddCommand<StatusOverviewCommand>(StatusOverviewCommand.CommandName);
        status.AddCommand<StatusSkillsCommand>(StatusSkillsCommand.CommandName);
        
        status.SetDefaultCommand<StatusOverviewCommand>();
    });
    
    configuration.AddCommand<WalkCommand>(WalkCommand.CommandName);
    
    configuration.AddBranch("save", save =>
    {
        save.AddCommand<SaveDeleteCommand>(SaveDeleteCommand.CommandName);
        save.SetDescription("Manage your save");
    });
    
    configuration.AddBranch("combat", combat =>
    {
        combat.AddCommand<CombatListCommand>(CombatListCommand.CommandName);
        combat.AddCommand<CombatAttackCommand>(CombatAttackCommand.CommandName);
        
        combat.SetDefaultCommand<CombatListCommand>();
    });

    configuration.AddBranch("inventory", inventory =>
    {
        inventory.AddCommand<InventoryListCommand>(InventoryListCommand.CommandName);
        inventory.AddCommand<InventoryDropCommand>(InventoryDropCommand.CommandName);
        inventory.AddCommand<InventorySortCommand>(InventorySortCommand.CommandName);

        inventory.SetDefaultCommand<InventoryListCommand>();
    }).WithAlias("inv");

    configuration.AddCommand<ItemCommand>(ItemCommand.CommandName);

    configuration.AddBranch("shop", shop =>
    {
        shop.AddCommand<ShopListCommand>(ShopListCommand.CommandName);
        shop.AddCommand<ShopViewCommand>(ShopViewCommand.CommandName);
        shop.AddCommand<ShopBuyCommand>(ShopBuyCommand.CommandName);
        shop.AddCommand<ShopSellCommand>(ShopSellCommand.CommandName);
        shop.AddCommand<ShopValueCommand>(ShopValueCommand.CommandName);

        shop.SetDefaultCommand<ShopListCommand>();
    });

    configuration.AddBranch("equipment", equipment =>
    {
        equipment.AddCommand<EquipmentViewCommand>(EquipmentViewCommand.CommandName);
        equipment.AddCommand<UnequipCommand>(UnequipCommand.CommandName);

        equipment.SetDefaultCommand<EquipmentViewCommand>();
    });
});

app.Run(args);
