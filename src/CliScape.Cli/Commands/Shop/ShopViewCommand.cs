using CliScape.Content.Items;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

/// <summary>
///     View a shop's inventory.
/// </summary>
public class ShopViewCommand : Command<ShopViewCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "view";

    public override int Execute(CommandContext context, ShopViewCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;

        var shop = location.Shops.FirstOrDefault(s =>
            s.Name.Value.Contains(settings.ShopName, StringComparison.OrdinalIgnoreCase));

        if (shop is null)
        {
            AnsiConsole.MarkupLine($"[red]No shop matching '{settings.ShopName}' found at {location.Name}.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'shop list' to see available shops.[/]");
            return (int)ExitCode.Failure;
        }

        AnsiConsole.MarkupLine($"[bold]{shop.Name}[/]");
        if (shop.IsGeneralStore)
        {
            AnsiConsole.MarkupLine("[green]This shop will buy any tradeable item.[/]");
        }

        var playerGold = player.Inventory.GetQuantity(Materials.Coins);
        AnsiConsole.MarkupLine($"[yellow]Your gold: {playerGold:N0} gp[/]");
        AnsiConsole.WriteLine();

        if (shop.Stock.Count == 0)
        {
            AnsiConsole.MarkupLine("[dim]This shop has no items in stock.[/]");
            return (int)ExitCode.Success;
        }

        var table = new Table()
            .AddColumn("Item")
            .AddColumn("Stock")
            .AddColumn("Buy Price");

        foreach (var stock in shop.Stock)
        {
            var buyPrice = shop.GetBuyPrice(stock.Item);
            var stockDisplay = stock.CurrentStock > 0
                ? stock.CurrentStock.ToString()
                : "[red]Out of stock[/]";
            var priceDisplay = stock.CurrentStock > 0
                ? $"[yellow]{buyPrice:N0} gp[/]"
                : "[dim]-[/]";

            table.AddRow(stock.Item.Name.Value, stockDisplay, priceDisplay);
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Use 'shop buy <item>' to purchase items.[/]");

        return (int)ExitCode.Success;
    }
}