using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Status;

/// <summary>
///     Displays a holistic overview of the player's state.
/// </summary>
public class StatusCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "status";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();

        // Single table with player status
        var statusTable = new Table()
            .AddColumn($"{player.Name}")
            .AddColumn($"Combat Level: [yellow]{player.CombatLevel}[/] | Total Level: [yellow]{player.TotalLevel}[/]")
            .Border(TableBorder.Rounded);

        statusTable.AddRow("Current Health", $"{player.CurrentHealth} / {player.MaximumHealth}");
        statusTable.AddRow("Prayer Points", $"{player.CurrentPrayerPoints} / {player.MaximumPrayerPoints}");
        statusTable.AddRow("Current Location", $"{player.CurrentLocation.Name}");
        AnsiConsole.Write(statusTable);

        return (int)ExitCode.Success;
    }
}