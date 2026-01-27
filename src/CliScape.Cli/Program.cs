using CliScape.Cli.Commands;
using CliScape.Cli.Commands.Combat;
using CliScape.Cli.Commands.Save;
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
    });
    
    configuration.AddBranch("combat", combat =>
    {
        combat.AddCommand<CombatListCommand>(CombatListCommand.CommandName);
        combat.AddCommand<CombatAttackCommand>(CombatAttackCommand.CommandName);
        
        combat.SetDefaultCommand<CombatListCommand>();
    });
});

app.Run(args);
