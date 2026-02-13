using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Status;

public class StatusOverviewCommand(GameState gameState) : Command, ICommand
{
    public static string CommandName => "overview";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();

        var table = new Table()
            .AddColumn($"{player.Name}")
            .AddColumn($"Combat Level: {player.CombatLevel} | Total Level: {player.TotalLevel}")
            .AddRow("HP", $"{player.CurrentHealth}/{player.MaximumHealth}")
            .AddRow("Location", $"{player.CurrentLocation.Name}");

        AnsiConsole.Write(table);

        return (int)ExitCode.Success;
    }
}