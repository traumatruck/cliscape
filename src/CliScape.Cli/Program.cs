using CliScape.Cli.Commands;
using CliScape.Cli.Commands.Save;
using CliScape.Cli.Commands.Status;
using CliScape.Cli.Commands.Travel;
using CliScape.Cli.Commands.Travel.TravelDirection;
using CliScape.Cli.Commands.Travel.TravelTo;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(configuration =>
{
    configuration.SetInterceptor(new CommandInterceptor());
    
    configuration.AddBranch("status", status =>
    {
        status.AddCommand<StatusOverviewCommand>(StatusOverviewCommand.CommandName);
        status.AddCommand<StatusStatsCommand>(StatusStatsCommand.CommandName);
    });
    
    configuration.AddBranch("travel", travel =>
    {
        travel.AddCommand<TravelListCommand>(TravelListCommand.CommandName);
        travel.AddCommand<TravelToCommand>(TravelToCommand.CommandName);
        travel.AddCommand<TravelDirectionCommand>(TravelDirectionCommand.CommandName);
    });
    
    configuration.AddBranch("save", save =>
    {
        save.AddCommand<SaveDeleteCommand>(SaveDeleteCommand.CommandName);
    });
});

app.Run(args);