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
            .AddColumn("Value");

        var slotNum = 1;
        foreach (var (item, quantity, _) in inventory.GetItems())
        {
            var totalValue = item.BaseValue * quantity;
            
            table.AddRow(
                slotNum.ToString(),
                item.Name.Value,
                quantity.ToString(),
                $"{totalValue:N0} gp"
            );
            slotNum++;
        }

        AnsiConsole.Write(table);
        return (int)ExitCode.Success;
    }
}