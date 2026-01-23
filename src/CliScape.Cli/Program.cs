using CliScape.Cli.Commands.Travel;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(configuration =>
{
    configuration.AddBranch("travel", travel =>
    {
        travel.AddCommand<TravelListCommand>(TravelListCommand.CommandName);
    });
});

app.Run(args);