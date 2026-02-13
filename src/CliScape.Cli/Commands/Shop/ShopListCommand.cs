using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

/// <summary>
///     Lists all shops available at the current location.
/// </summary>
public class ShopListCommand(GameState gameState) : Command, ICommand
{
    public static string CommandName => "list";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
        var location = player.CurrentLocation;
        var shops = location.Shops;

        if (shops.Count == 0)
        {
            AnsiConsole.MarkupLine("[dim]There are no shops at this location.[/]");
            return (int)ExitCode.Success;
        }

        var table = new Table()
            .AddColumn("#")
            .AddColumn("Shop")
            .AddColumn("Type");

        for (var i = 0; i < shops.Count; i++)
        {
            var shop = shops[i];
            var shopType = shop.IsGeneralStore ? "[green]General Store[/]" : "[cyan]Specialty[/]";
            table.AddRow((i + 1).ToString(), shop.Name.Value, shopType);
        }

        AnsiConsole.Write(table);

        return (int)ExitCode.Success;
    }
}