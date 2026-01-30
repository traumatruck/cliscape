using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

/// <summary>
///     Check how much an item is worth to sell to shops at the current location.
/// </summary>
public class ShopValueCommand : Command<ShopValueCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "value";

    public override int Execute(CommandContext context, ShopValueCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;
        var inventory = player.Inventory;

        // Find the item in inventory
        var itemEntry = inventory.GetItems()
            .FirstOrDefault(i => i.Item.Name.Value.Equals(settings.ItemName, StringComparison.OrdinalIgnoreCase));

        if (itemEntry == default)
        {
            AnsiConsole.MarkupLine($"[red]You don't have any '{settings.ItemName}' in your inventory.[/]");
            return (int)ExitCode.Failure;
        }

        var item = itemEntry.Item;
        var available = itemEntry.Quantity;

        AnsiConsole.MarkupLine($"[bold]{item.Name}[/] (you have {available})");
        AnsiConsole.MarkupLine($"Base value: [yellow]{item.BaseValue:N0} gp[/]");
        AnsiConsole.WriteLine();

        if (!item.IsTradeable)
        {
            AnsiConsole.MarkupLine("[red]This item cannot be traded.[/]");
            return (int)ExitCode.Success;
        }

        if (location.Shops.Count == 0)
        {
            AnsiConsole.MarkupLine("[dim]There are no shops at this location.[/]");
            return (int)ExitCode.Success;
        }

        var table = new Table()
            .AddColumn("Shop")
            .AddColumn("Sell Price");

        var anyBuyers = false;
        foreach (var shop in location.Shops)
        {
            var sellPrice = shop.GetSellPrice(item);
            if (sellPrice > 0 || shop.IsGeneralStore)
            {
                anyBuyers = true;
                table.AddRow(
                    shop.Name.Value,
                    sellPrice > 0 ? $"[yellow]{sellPrice:N0} gp[/]" : "[dim]0 gp[/]"
                );
            }
        }

        if (anyBuyers)
        {
            AnsiConsole.Write(table);
        }
        else
        {
            AnsiConsole.MarkupLine("[dim]No shops at this location will buy this item.[/]");
        }

        return (int)ExitCode.Success;
    }
}