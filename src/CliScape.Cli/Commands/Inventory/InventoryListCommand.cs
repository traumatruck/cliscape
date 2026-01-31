using CliScape.Core.Items;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

/// <summary>
///     Lists all items in the player's inventory.
/// </summary>
public class InventoryListCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "list";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var inventory = player.Inventory;

        // Check if inventory is completely empty
        if (inventory.UsedSlots == 0)
        {
            AnsiConsole.MarkupLine("Your inventory is empty.");
            return (int)ExitCode.Success;
        }

        AnsiConsole.MarkupLine($"[bold]Inventory ({inventory.UsedSlots}/{Core.Items.Inventory.MaxSlots})[/]");
        AnsiConsole.WriteLine();

        var table = new Table()
            .AddColumn("#")
            .AddColumn("Item")
            .AddColumn("Qty")
            .AddColumn("Value")
            .AddColumn("Actions");

        var slotNum = 1;
        foreach (var (item, quantity, _) in inventory.GetItems())
        {
            var totalValue = item.BaseValue * quantity;
            
            // Get available actions for the item
            var actionsText = GetAvailableActionsText(item);
            
            table.AddRow(
                slotNum.ToString(),
                item.Name.Value,
                quantity.ToString(),
                $"{totalValue:N0} gp",
                actionsText
            );
            slotNum++;
        }

        AnsiConsole.Write(table);
        return (int)ExitCode.Success;
    }

    /// <summary>
    ///     Gets a formatted string of available actions for an item.
    /// </summary>
    private static string GetAvailableActionsText(IItem item)
    {
        if (item is IActionableItem actionable && actionable.Actions.Count > 0)
        {
            var actions = actionable.Actions
                .Select(action => action.ActionType.ToString().ToLowerInvariant())
                .ToList();
            return string.Join(", ", actions);
        }

        return "[dim]None[/]";
    }
}