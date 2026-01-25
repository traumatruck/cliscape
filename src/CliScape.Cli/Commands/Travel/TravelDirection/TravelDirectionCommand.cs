using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Travel.TravelDirection;

public class TravelDirectionCommand : Command<TravelDirectionCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "direction";

    public override int Execute(CommandContext context, TravelDirectionCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var direction = settings.Direction;
        var currentLocation = player.CurrentLocation;

        if (!currentLocation.AdjacentLocations.ContainsKey(direction))
        {
            AnsiConsole.WriteLine($"You fail to go {direction}.");
        }

        var targetLocationName = currentLocation.AdjacentLocations[direction];
        var targetLocation = _gameState.GetLocation(targetLocationName.Value);
        player.Move(targetLocation);
        AnsiConsole.WriteLine($"You travel to {targetLocationName}.");

        return (int)ExitCode.Success;
    }
}