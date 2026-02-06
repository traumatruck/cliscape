using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Bank;

/// <summary>
///     Displays the contents of the player's bank.
/// </summary>
public class BankViewCommand : Command<BankViewCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "view";

    public override int Execute(CommandContext context, BankViewCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Check if bank exists at current location
        if (!location.HasBank)
        {
            AnsiConsole.MarkupLine("[red]There is no bank at {0}.[/]", location.Name.Value);
            AnsiConsole.MarkupLine(
                "[dim]Banks are available at: Lumbridge, Varrock, Falador, Edgeville, Al Kharid, Draynor Village, Catherby[/]");
            return (int)ExitCode.Failure;
        }

        var bank = player.Bank;
        var items = bank.GetItems().ToList();

        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]Your bank is empty.[/]");
            AnsiConsole.MarkupLine($"[dim]Bank capacity: {bank.UsedSlots}/{Core.World.Bank.MaxSlots} slots used[/]");
            return (int)ExitCode.Success;
        }

        // Pagination: 28 items per page (matching inventory)
        const int itemsPerPage = 28;
        var totalPages = (int)Math.Ceiling(items.Count / (double)itemsPerPage);
        var page = Math.Clamp(settings.Page, 1, totalPages);

        var pageItems = items
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToList();

        var table = new Table()
            .AddColumn("#")
            .AddColumn("Item")
            .AddColumn("Quantity")
            .AddColumn("Total Value");

        foreach (var (item, quantity, slotIndex) in pageItems)
        {
            var totalValue = item.BaseValue * quantity;
            table.AddRow(
                (slotIndex + 1).ToString(),
                item.Name.Value,
                quantity.ToString("N0"),
                $"{totalValue:N0} gp"
            );
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine($"[dim]Bank: {bank.UsedSlots}/{Core.World.Bank.MaxSlots} slots used[/]");

        if (totalPages > 1)
        {
            AnsiConsole.MarkupLine($"[dim]Page {page} of {totalPages} | Use --page to view other pages[/]");
        }

        return (int)ExitCode.Success;
    }
}