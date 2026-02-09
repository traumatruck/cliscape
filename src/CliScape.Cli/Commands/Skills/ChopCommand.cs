using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Core;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Chop trees in the current location for logs.
/// </summary>
[Description("Chop trees in your current location")]
public class ChopCommand : Command<ChopCommandSettings>, ICommand
{
    /// <summary>
    ///     Well-known hatchet item IDs in order of effectiveness.
    /// </summary>
    private static readonly ItemId[] HatchetIds =
    [
        new(105) // Bronze hatchet
        // TODO: Add iron, steel, mithril, adamant, rune, dragon hatchets
    ];

    private readonly GameState _gameState = GameState.Instance;
    private readonly IRandomProvider _random = RandomProvider.Instance;

    public static string CommandName => "chop";

    public override int Execute(CommandContext context, ChopCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
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

        return ChopTree(player, tree);
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
            var logName = GetLogName(tree.LogItemId);

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

    private int ChopTree(Player player, ITree tree)
    {
        var woodcuttingSkill = player.GetSkill(SkillConstants.WoodcuttingSkillName);
        var woodcuttingLevel = woodcuttingSkill.Level.Value;

        // Check level requirement
        if (woodcuttingLevel < tree.RequiredLevel)
        {
            AnsiConsole.MarkupLine($"[red]You need level {tree.RequiredLevel} Woodcutting to chop this tree.[/]");
            return (int)ExitCode.Failure;
        }

        // Check for hatchet
        if (!HasHatchet(player, out _))
        {
            AnsiConsole.MarkupLine("[red]You need a hatchet to chop trees.[/]");
            return (int)ExitCode.Failure;
        }

        // Get the log item
        var logItem = ItemRegistry.GetById(tree.LogItemId);
        if (logItem is null)
        {
            AnsiConsole.MarkupLine("[red]Something went wrong while chopping.[/]");
            return (int)ExitCode.Failure;
        }

        // Determine random number of logs from this tree
        var potentialLogs = _random.Next(1, tree.MaxActions + 1);
        
        // Limit by available inventory space
        var availableSlots = player.Inventory.FreeSlots;
        if (availableSlots == 0)
        {
            AnsiConsole.MarkupLine("[red]Your inventory is full.[/]");
            return (int)ExitCode.Failure;
        }
        
        var logsObtained = Math.Min(potentialLogs, availableSlots);
        var totalXp = 0;

        // Add all logs to inventory and calculate total XP
        for (var i = 0; i < logsObtained; i++)
        {
            if (!player.Inventory.TryAdd(logItem))
            {
                // Should not happen since we checked space, but handle it anyway
                break;
            }
            
            totalXp += tree.Experience;
        }
        
        // Grant all experience at once
        if (totalXp > 0)
        {
            Player.AddExperience(woodcuttingSkill, totalXp);
        }

        if (logsObtained == 1)
        {
            AnsiConsole.MarkupLine($"[green]You chop down the {tree.Name.ToLower()} and get some {logItem.Name}.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine(
                $"[green]You chop down the {tree.Name.ToLower()} and get {logsObtained}x {logItem.Name}.[/]");
        }

        AnsiConsole.MarkupLine($"[dim]You gain {totalXp:N0} Woodcutting experience.[/]");

        _gameState.Save();
        return (int)ExitCode.Success;
    }

    private static ITree? FindTree(IReadOnlyList<ITree> trees, string treeType)
    {
        // Match by tree type name or tree name
        return trees.FirstOrDefault(t =>
            t.TreeType.ToString().Equals(treeType, StringComparison.OrdinalIgnoreCase) ||
            t.Name.Contains(treeType, StringComparison.OrdinalIgnoreCase));
    }

    private static bool HasHatchet(Player player, out string? hatchetName)
    {
        hatchetName = null;

        // Check inventory for any hatchet
        foreach (var hatchetId in HatchetIds)
        {
            var inInventory = player.Inventory.GetItems()
                .FirstOrDefault(slot => slot.Item.Id == hatchetId);

            if (inInventory != default)
            {
                hatchetName = inInventory.Item.Name.Value;
                return true;
            }
        }

        // Check equipped weapon slot for hatchet
        foreach (var hatchetId in HatchetIds)
        {
            var equipped = player.Equipment.GetAllEquipped()
                .FirstOrDefault(item => item.Id == hatchetId);

            if (equipped is not null)
            {
                hatchetName = equipped.Name.Value;
                return true;
            }
        }

        return false;
    }

    private static string GetLogName(ItemId logId)
    {
        var item = ItemRegistry.GetById(logId);
        return item?.Name.Value ?? "unknown logs";
    }
}