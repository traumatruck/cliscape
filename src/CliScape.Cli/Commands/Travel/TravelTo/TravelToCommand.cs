using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Travel.TravelTo;

public class TravelToCommand : Command<TravelToCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "to";

    public override int Execute(CommandContext context, TravelToCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var locationName = settings.LocationName;
        var location = _gameState.GetLocation(locationName);
        player.Move(location);
        AnsiConsole.Write($"You travel to {location.Name}.");

        return 0;
    }
}