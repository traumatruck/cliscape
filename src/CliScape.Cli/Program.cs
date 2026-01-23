using CliScape.Cli.Commands;
using CliScape.Cli.Commands.Status;
using CliScape.Cli.Commands.Travel;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(configuration =>
{
    configuration.SetInterceptor(new CommandInterceptor());

    configuration.AddCommand<StatusCommand>(StatusCommand.CommandName);
    
    configuration.AddBranch("travel", travel =>
    {
        travel.AddCommand<TravelListCommand>(TravelListCommand.CommandName);
    });
});

app.Run(args);