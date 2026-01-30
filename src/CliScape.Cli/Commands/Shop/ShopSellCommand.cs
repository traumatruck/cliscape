using CliScape.Content.Items;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

/// <summary>
///     Sell an item from inventory to a shop.
/// </summary>
public class ShopSellCommand : Command<ShopSellCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "sell";

    public override int Execute(CommandContext context, ShopSellCommandSettings settings,
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

        var (item, available, _) = itemEntry;
        var quantity = settings.Quantity ?? 1;

        if (quantity > available)
        {
            AnsiConsole.MarkupLine($"[red]You only have {available} {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        if (!item.IsTradeable)
        {
            AnsiConsole.MarkupLine($"[red]You cannot sell {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        // Find a shop that will buy this item
        Core.World.Shop? targetShop = null;

        // First try specialty shops that stock this item
        foreach (var shop in location.Shops.Where(s => !s.IsGeneralStore))
        {
            if (shop.GetSellPrice(item) > 0)
            {
                targetShop = shop;
                break;
            }
        }

        // Then try general stores
        targetShop ??= location.Shops.FirstOrDefault(s => s.IsGeneralStore);

        if (targetShop is null)
        {
            AnsiConsole.MarkupLine($"[red]No shop at {location.Name} will buy {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        var pricePerItem = targetShop.GetSellPrice(item);
        var totalPrice = pricePerItem * quantity;

        // Process sale
        if (targetShop.TrySell(item, Materials.Coins, quantity, inventory))
        {
            if (quantity == 1)
            {
                AnsiConsole.MarkupLine(
                    $"You sell your [yellow]{item.Name}[/] to {targetShop.Name} for [yellow]{totalPrice:N0} gp[/].");
            }
            else
            {
                AnsiConsole.MarkupLine(
                    $"You sell [yellow]{quantity} {item.Name}[/] to {targetShop.Name} for [yellow]{totalPrice:N0} gp[/].");
            }

            var totalGold = player.Inventory.GetQuantity(Materials.Coins);
            AnsiConsole.MarkupLine($"[dim]Total gold: {totalGold:N0} gp[/]");
            return (int)ExitCode.Success;
        }

        AnsiConsole.MarkupLine("[red]Failed to complete the sale.[/]");
        return (int)ExitCode.Failure;
    }
}