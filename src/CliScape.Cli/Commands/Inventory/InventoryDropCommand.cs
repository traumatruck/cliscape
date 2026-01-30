using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

/// <summary>
///     Drop an item from inventory.
/// </summary>
public class InventoryDropCommand : Command<InventoryDropCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "drop";

    public override int Execute(CommandContext context, InventoryDropCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var inventory = player.Inventory;

        // Find the item by name
        var itemEntry = inventory.GetItems()
            .FirstOrDefault(i => i.Item.Name.Value.Equals(settings.ItemName, StringComparison.OrdinalIgnoreCase));

        if (itemEntry == default)
        {
            AnsiConsole.MarkupLine($"[red]You don't have any '{settings.ItemName}' in your inventory.[/]");
            return (int)ExitCode.Failure;
        }

        var (item, available, _) = itemEntry;
        var quantity = settings.Quantity ?? available;

        if (quantity > available)
        {
            AnsiConsole.MarkupLine($"[red]You only have {available} {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        var removed = inventory.Remove(item, quantity);

        if (quantity == 1)
        {
            AnsiConsole.MarkupLine($"You drop the [yellow]{item.Name}[/].");
        }
        else
        {
            AnsiConsole.MarkupLine($"You drop [yellow]{removed} {item.Name}[/].");
        }

        return (int)ExitCode.Success;
    }
}