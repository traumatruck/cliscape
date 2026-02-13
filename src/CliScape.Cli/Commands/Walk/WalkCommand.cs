using System.ComponentModel;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Walk;

[Description("Travel to a new location")]
public class WalkCommand(GameState gameState) : Command<WalkCommandSettings>, ICommand
{
    public static string CommandName => "walk";

    public override int Execute(CommandContext context, WalkCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
        var destination = gameState.GetLocation(settings.LocationName);

        if (destination == null)
        {
            AnsiConsole.WriteLine($"Unknown location {settings.LocationName}");
            return (int)ExitCode.BadRequest;
        }

        player.Move(destination);
        AnsiConsole.WriteLine($"You walk to {destination.Name}.");

        return (int)ExitCode.Success;
    }
}