using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Travel;

public class TravelListCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "list";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();

        var table = new Table()
            .AddColumn("Direction")
            .AddColumn("Location");

        var adjacentLocations = player.CurrentLocation.AdjacentLocations;

        foreach (var adjacentLocation in adjacentLocations)
        {
            table.AddRow($"{adjacentLocation.Key}", $"{adjacentLocation.Value}");
        }

        AnsiConsole.Write(table);

        return (int)ExitCode.Success;
    }
}