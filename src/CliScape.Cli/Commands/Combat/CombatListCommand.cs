using CliScape.Core.Npcs;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Combat;

/// <summary>
///     Lists all combatable NPCs at the player's current location.
/// </summary>
public class CombatListCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "list";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;
        var npcs = location.AvailableNpcs;

        if (npcs.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no NPCs here.[/]");
            return (int)ExitCode.Success;
        }

        var combatableNpcs = npcs.OfType<ICombatableNpc>().ToList();

        if (combatableNpcs.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no combatable NPCs here.[/]");
            return (int)ExitCode.Success;
        }

        AnsiConsole.MarkupLine($"[bold]NPCs at {location.Name}:[/]");
        AnsiConsole.WriteLine();

        foreach (var npc in combatableNpcs)
        {
            var aggroMarker = npc.IsAggressive ? "[red]*[/]" : "";

            AnsiConsole.MarkupLine($"  {aggroMarker}[green]{npc.Name.Value}[/] [dim](level-{npc.CombatLevel})[/]");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Use 'combat attack <NpcName>' to start combat.[/]");

        return (int)ExitCode.Success;
    }
}