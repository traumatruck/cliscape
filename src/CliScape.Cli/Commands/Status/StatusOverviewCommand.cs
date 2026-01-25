using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Status;

public class StatusOverviewCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;
    
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();

        var table = new Table()
            .AddColumn($"{player.Name}")
            .AddColumn($"Combat Level: 126 | Total Level: 986")
            .AddRow("HP", $"{player.Health.CurrentHealth}/{player.Health.MaxHealth}")
            .AddRow("Location", $"{player.CurrentLocation.Name}");
        
        AnsiConsole.Write(table);

        return (int) ExitCode.Success;
    }

    public static string CommandName => "overview";
}