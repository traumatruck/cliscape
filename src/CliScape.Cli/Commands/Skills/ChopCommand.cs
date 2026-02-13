using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game;
using CliScape.Game.Skills;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Chop trees in the current location for logs.
/// </summary>
[Description("Chop trees in your current location")]
public class ChopCommand(GameState gameState, WoodcuttingService woodcuttingService) : Command<ChopCommandSettings>,
    ICommand
{
    public static string CommandName => "chop";

    public override int Execute(CommandContext context, ChopCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
        var location = player.CurrentLocation;

        // If no tree specified, list available trees
        if (string.IsNullOrWhiteSpace(settings.TreeType))
        {
            return ListTrees(location.Trees, player);
        }

        // Find the matching tree
        var tree = FindTree(location.Trees, settings.TreeType);
        if (tree is null)
        {
            AnsiConsole.MarkupLine($"[red]There is no '{settings.TreeType}' tree here.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'skills chop' without arguments to see available trees.[/]");
            return (int)ExitCode.Failure;
        }

        // Validate via service
        var canChopResult = woodcuttingService.CanChop(player, tree.RequiredLevel);
        if (!canChopResult.Success)
        {
            AnsiConsole.MarkupLine($"[red]{canChopResult.Message}[/]");
            return (int)ExitCode.Failure;
        }

        // Determine random number of logs from this tree
        var count = Random.Shared.Next(1, tree.MaxActions + 1);

        // Execute via service
        var result = woodcuttingService.Chop(player, tree.LogItemId, tree.Experience, count, ItemRegistry.GetById);

        if (!result.Success)
        {
            AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            return (int)ExitCode.Failure;
        }

        // Display result
        if (result.LogsObtained == 1)
        {
            AnsiConsole.MarkupLine(
                $"[green]You chop down the {tree.Name.ToLower()} and get some {result.LogName}.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine(
                $"[green]You chop down the {tree.Name.ToLower()} and get {result.LogsObtained}x {result.LogName}.[/]");
        }

        AnsiConsole.MarkupLine($"[dim]You gain {result.TotalExperience:N0} Woodcutting experience.[/]");

        if (result.LevelUp is { } levelUp)
        {
            AnsiConsole.MarkupLine(
                $"[bold yellow]Congratulations! Your Woodcutting level is now {levelUp.NewLevel}![/]");
        }

        return (int)ExitCode.Success;
    }

    private static int ListTrees(IReadOnlyList<ITree> trees, Player player)
    {
        if (trees.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no trees here.[/]");
            return (int)ExitCode.Success;
        }

        var woodcuttingLevel = player.GetSkillLevel(SkillConstants.WoodcuttingSkillName).Value;

        var table = new Table()
            .AddColumn("Tree")
            .AddColumn("Req. Level")
            .AddColumn("Experience")
            .AddColumn("Logs");

        foreach (var tree in trees)
        {
            var levelColor = woodcuttingLevel >= tree.RequiredLevel ? "green" : "red";
            var logName = GetItemName(tree.LogItemId);

            table.AddRow(
                tree.Name,
                $"[{levelColor}]{tree.RequiredLevel}[/]",
                tree.Experience.ToString(),
                logName
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[dim]Use 'skills chop <tree>' to chop a tree (e.g., 'skills chop oak').[/]");

        return (int)ExitCode.Success;
    }

    private static ITree? FindTree(IReadOnlyList<ITree> trees, string treeType)
    {
        return trees.FirstOrDefault(t =>
            t.TreeType.ToString().Equals(treeType, StringComparison.OrdinalIgnoreCase) ||
            t.Name.Contains(treeType, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetItemName(ItemId logId)
    {
        var item = ItemRegistry.GetById(logId);
        return item?.Name.Value ?? "unknown";
    }
}