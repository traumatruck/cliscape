using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Bank;

/// <summary>
///     Withdraws items from the bank into inventory.
/// </summary>
[Description("Withdraw an item from your bank")]
public class BankWithdrawCommand : Command<BankWithdrawCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "withdraw";

    public override int Execute(CommandContext context, BankWithdrawCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Check if bank exists at current location
        if (!location.HasBank)
        {
            AnsiConsole.MarkupLine("[red]There is no bank at {0}.[/]", location.Name.Value);
            return (int)ExitCode.Failure;
        }

        var inventory = player.Inventory;
        var bank = player.Bank;

        // Find the item by name
        var item = ItemRegistry.GetByName(settings.ItemName);
        if (item is null)
        {
            AnsiConsole.MarkupLine($"[red]Unknown item: '{settings.ItemName}'.[/]");
            return (int)ExitCode.Failure;
        }

        var bankQuantity = bank.GetQuantity(item);
        if (bankQuantity == 0)
        {
            AnsiConsole.MarkupLine($"[red]You don't have any {item.Name} in your bank.[/]");
            return (int)ExitCode.Failure;
        }

        var quantityToWithdraw = settings.All ? bankQuantity : settings.Quantity;

        if (quantityToWithdraw > bankQuantity)
        {
            AnsiConsole.MarkupLine($"[red]You only have {bankQuantity} {item.Name} in your bank.[/]");
            return (int)ExitCode.Failure;
        }

        if (!inventory.CanAdd(item, quantityToWithdraw))
        {
            AnsiConsole.MarkupLine("[red]Your inventory is full.[/]");
            return (int)ExitCode.Failure;
        }

        var withdrawn = bank.Withdraw(item, quantityToWithdraw);
        inventory.TryAdd(item, withdrawn);

        if (quantityToWithdraw == 1)
        {
            AnsiConsole.MarkupLine($"[green]Withdrew {item.Name} from bank.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]Withdrew {withdrawn} x {item.Name} from bank.[/]");
        }

        return (int)ExitCode.Success;
    }
}