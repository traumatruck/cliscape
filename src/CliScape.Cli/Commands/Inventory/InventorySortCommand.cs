using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

/// <summary>
///     Sorts inventory items by swapping or inserting them at specific positions.
/// </summary>
public class InventorySortCommand : Command<InventorySortCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "sort";

    public override int Execute(CommandContext context, InventorySortCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var inventory = player.Inventory;

        // Validate slot numbers (convert from 1-based to 0-based)
        if (settings.FromSlot < 1 || settings.FromSlot > Core.Items.Inventory.MaxSlots)
        {
            AnsiConsole.MarkupLine(
                $"[red]Invalid from slot. Must be between 1 and {Core.Items.Inventory.MaxSlots}.[/]");
            
            return (int)ExitCode.Failure;
        }

        if (settings.ToSlot < 1 || settings.ToSlot > Core.Items.Inventory.MaxSlots)
        {
            AnsiConsole.MarkupLine($"[red]Invalid to slot. Must be between 1 and {Core.Items.Inventory.MaxSlots}.[/]");
            return (int)ExitCode.Failure;
        }

        var fromIndex = settings.FromSlot - 1;
        var toIndex = settings.ToSlot - 1;

        // Check if from slot is empty
        var fromSlot = inventory.GetSlot(fromIndex);
        if (fromSlot.IsEmpty)
        {
            AnsiConsole.MarkupLine($"[yellow]Slot {settings.FromSlot} is empty.[/]");
            return (int)ExitCode.Failure;
        }

        var itemName = fromSlot.Item!.Name.Value;

        try
        {
            if (settings.Insert)
            {
                inventory.MoveSlot(fromIndex, toIndex);
                AnsiConsole.MarkupLine(
                    $"[green]Moved {itemName} from slot {settings.FromSlot} to slot {settings.ToSlot}.[/]");
            }
            else
            {
                inventory.SwapSlots(fromIndex, toIndex);
                var toSlot = inventory.GetSlot(toIndex);

                if (toSlot.IsEmpty)
                {
                    AnsiConsole.MarkupLine(
                        $"[green]Moved {itemName} from slot {settings.FromSlot} to slot {settings.ToSlot}.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine(
                        $"[green]Swapped {itemName} (slot {settings.FromSlot}) with {toSlot.Item!.Name.Value} (slot {settings.ToSlot}).[/]");
                }
            }

            return (int)ExitCode.Success;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to sort inventory: {ex.Message}[/]");
            return (int)ExitCode.Failure;
        }
    }
}