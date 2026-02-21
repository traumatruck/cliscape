using CliScape.Content.Items;
using CliScape.Core.Items;
using CliScape.Core.World;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

/// <summary>
///     Buy an item from a shop.
/// </summary>
public class ShopBuyCommand(GameState gameState, IItemRegistry itemRegistry) : Command<ShopBuyCommandSettings>, ICommand
{
    public static string CommandName => "buy";

    public override int Execute(CommandContext context, ShopBuyCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Find the shop that has this item
        Core.World.Shop? targetShop = null;
        ShopStock? targetStock = null;

        foreach (var shop in location.Shops)
        {
            var stock = shop.GetStockByName(settings.ItemName);
            if (stock is not null)
            {
                targetShop = shop;
                targetStock = stock;
                break;
            }
        }

        if (targetShop is null || targetStock is null)
        {
            AnsiConsole.MarkupLine($"[red]No shop at {location.Name} sells '{settings.ItemName}'.[/]");
            return (int)ExitCode.Failure;
        }

        var item = targetStock.Item;
        var quantity = settings.Quantity;
        var pricePerItem = targetShop.GetBuyPrice(item);
        var totalPrice = pricePerItem * quantity;
        var playerGold = player.Inventory.GetQuantity(itemRegistry.GetById(ItemIds.Coins)!);

        // Validate
        if (targetStock.CurrentStock < quantity)
        {
            AnsiConsole.MarkupLine($"[red]The shop only has {targetStock.CurrentStock} in stock.[/]");
            return (int)ExitCode.Failure;
        }

        if (playerGold < totalPrice)
        {
            AnsiConsole.MarkupLine($"[red]You need {totalPrice:N0} gp but only have {playerGold:N0} gp.[/]");
            return (int)ExitCode.Failure;
        }

        if (!player.Inventory.CanAdd(item, quantity))
        {
            AnsiConsole.MarkupLine("[red]You don't have enough inventory space.[/]");
            return (int)ExitCode.Failure;
        }

        // Process purchase
        if (targetShop.TryBuy(item, itemRegistry.GetById(ItemIds.Coins)!, quantity, player.Inventory))
        {
            if (quantity == 1)
            {
                AnsiConsole.MarkupLine($"You buy a [yellow]{item.Name}[/] for [yellow]{totalPrice:N0} gp[/].");
            }
            else
            {
                AnsiConsole.MarkupLine($"You buy [yellow]{quantity} {item.Name}[/] for [yellow]{totalPrice:N0} gp[/].");
            }

            var remainingGold = player.Inventory.GetQuantity(itemRegistry.GetById(ItemIds.Coins)!);
            AnsiConsole.MarkupLine($"[dim]Remaining gold: {remainingGold:N0} gp[/]");
            return (int)ExitCode.Success;
        }

        AnsiConsole.MarkupLine("[red]Failed to complete the purchase.[/]");
        return (int)ExitCode.Failure;
    }
}