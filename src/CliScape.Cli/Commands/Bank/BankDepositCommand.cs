using System.ComponentModel;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Bank;

/// <summary>
///     Deposits items from inventory into the bank.
/// </summary>
[Description("Deposit items in your bank")]
public class BankDepositCommand : Command<BankDepositCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "deposit";

    public override int Execute(CommandContext context, BankDepositCommandSettings settings,
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

        // Handle --all flag
        if (settings.All)
        {
            var items = inventory.GetItems().ToList();
            if (items.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]Your inventory is empty.[/]");
                return (int)ExitCode.Success;
            }

            var totalDeposited = 0;
            foreach (var (item, quantity, _) in items)
            {
                if (!bank.CanDeposit(item, quantity))
                {
                    AnsiConsole.MarkupLine($"[red]Not enough bank space for {item.Name}. Stopped depositing.[/]");
                    break;
                }

                var removed = inventory.Remove(item, quantity);
                bank.TryDeposit(item, removed);
                totalDeposited += removed;
            }

            AnsiConsole.MarkupLine($"[green]Deposited all items to bank ({totalDeposited} items total).[/]");
            return (int)ExitCode.Success;
        }

        // Find the item by name
        var itemEntry = inventory.GetItems()
            .FirstOrDefault(i => i.Item.Name.Value.Equals(settings.ItemName, StringComparison.OrdinalIgnoreCase));

        if (itemEntry == default)
        {
            AnsiConsole.MarkupLine($"[red]You don't have any '{settings.ItemName}' in your inventory.[/]");
            return (int)ExitCode.Failure;
        }

        var (targetItem, available, _) = itemEntry;
        var quantityToDeposit = settings.Quantity ?? available;

        if (quantityToDeposit > available)
        {
            AnsiConsole.MarkupLine($"[red]You only have {available} {targetItem.Name} in your inventory.[/]");
            return (int)ExitCode.Failure;
        }

        if (!bank.CanDeposit(targetItem, quantityToDeposit))
        {
            AnsiConsole.MarkupLine("[red]Your bank is full.[/]");
            return (int)ExitCode.Failure;
        }

        var removedAmount = inventory.Remove(targetItem, quantityToDeposit);
        bank.TryDeposit(targetItem, removedAmount);

        AnsiConsole.MarkupLine(quantityToDeposit == 1
            ? $"[green]Deposited {targetItem.Name} to bank.[/]"
            : $"[green]Deposited {removedAmount} x {targetItem.Name} to bank.[/]");

        return (int)ExitCode.Success;
    }
}