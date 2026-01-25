using CliScape.Cli.Commands;
using CliScape.Cli.Commands.Save;
using CliScape.Cli.Commands.Status;
using CliScape.Cli.Commands.Walk;
using Spectre.Console.Cli;

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
});

app.Run(args);
