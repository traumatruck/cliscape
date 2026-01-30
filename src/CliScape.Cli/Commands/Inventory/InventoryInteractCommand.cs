using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

/// <summary>
///     Interact with an item in inventory using a specific action (eat, bury, use, etc.).
/// </summary>
public class InventoryInteractCommand : Command<InventoryInteractCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "interact";

    public override int Execute(CommandContext context, InventoryInteractCommandSettings settings,
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

        var item = itemEntry.Item;

        // Determine which action was requested
        var action = DetermineAction(settings);

        if (action is null)
        {
            return ShowAvailableActions(item);
        }

        return ExecuteAction(item, action.Value, player, inventory);
    }

    private static ItemAction? DetermineAction(InventoryInteractCommandSettings settings)
    {
        if (settings.Eat)
        {
            return ItemAction.Eat;
        }

        if (settings.Bury)
        {
            return ItemAction.Bury;
        }

        if (settings.Use)
        {
            return ItemAction.Use;
        }

        if (settings.Drink)
        {
            return ItemAction.Drink;
        }

        if (settings.Read)
        {
            return ItemAction.Read;
        }

        return null;
    }

    private static int ShowAvailableActions(IItem item)
    {
        if (item is not IActionableItem actionableItem || actionableItem.AvailableActions.Count == 0)
        {
            AnsiConsole.MarkupLine($"[yellow]{item.Name} has no special actions available.[/]");
            AnsiConsole.MarkupLine("[dim]You can examine or drop it from your inventory.[/]");
            return (int)ExitCode.Success;
        }

        AnsiConsole.MarkupLine($"[yellow]{item.Name}[/] can be used with the following actions:");
        foreach (var availableAction in actionableItem.AvailableActions)
        {
            var actionFlag = GetActionFlag(availableAction);
            AnsiConsole.MarkupLine($"  [green]--{actionFlag}[/] - {GetActionDescription(availableAction)}");
        }

        return (int)ExitCode.Success;
    }

    private int ExecuteAction(IItem item, ItemAction action, Player player, Core.Items.Inventory inventory)
    {
        // Check if item supports the action
        if (item is IActionableItem actionableItem && !actionableItem.SupportsAction(action))
        {
            AnsiConsole.MarkupLine($"[red]You can't {action.ToString().ToLower()} a {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        switch (action)
        {
            case ItemAction.Eat:
                return HandleEat(item, player, inventory);

            case ItemAction.Bury:
                return HandleBury(item, player, inventory);

            case ItemAction.Use:
            case ItemAction.Drink:
            case ItemAction.Read:
                AnsiConsole.MarkupLine(
                    $"[yellow]The {action.ToString().ToLower()} action is not yet implemented for this item.[/]");
                return (int)ExitCode.Failure;

            default:
                AnsiConsole.MarkupLine($"[red]Unknown action: {action}[/]");
                return (int)ExitCode.Failure;
        }
    }

    private int HandleEat(IItem item, Player player, Core.Items.Inventory inventory)
    {
        if (item is not IEdible edible)
        {
            AnsiConsole.MarkupLine($"[red]You can't eat a {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        // Remove the item from inventory (consuming it)
        var removed = inventory.Remove(item);
        if (removed == 0)
        {
            AnsiConsole.MarkupLine("[red]Failed to consume the item.[/]");
            return (int)ExitCode.Failure;
        }

        // Apply the eat effect
        var message = edible.Eat(player);
        AnsiConsole.MarkupLine($"[green]{message}[/]");
        AnsiConsole.MarkupLine($"[dim]Health: {player.CurrentHealth}/{player.MaximumHealth}[/]");

        return (int)ExitCode.Success;
    }

    private int HandleBury(IItem item, Player player, Core.Items.Inventory inventory)
    {
        if (item is not IBuryable buryable)
        {
            AnsiConsole.MarkupLine($"[red]You can't bury a {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        // Remove the item from inventory (consuming it)
        var removed = inventory.Remove(item);
        if (removed == 0)
        {
            AnsiConsole.MarkupLine("[red]Failed to bury the item.[/]");
            return (int)ExitCode.Failure;
        }

        // Apply the bury effect
        var message = buryable.Bury(player);
        AnsiConsole.MarkupLine($"[green]{message}[/]");

        return (int)ExitCode.Success;
    }

    private static string GetActionFlag(ItemAction action)
    {
        return action switch
        {
            ItemAction.Use => "use",
            ItemAction.Eat => "eat",
            ItemAction.Bury => "bury",
            ItemAction.Drink => "drink",
            ItemAction.Read => "read",
            _ => action.ToString().ToLower()
        };
    }

    private static string GetActionDescription(ItemAction action)
    {
        return action switch
        {
            ItemAction.Use => "Use the item",
            ItemAction.Eat => "Eat to restore hitpoints",
            ItemAction.Bury => "Bury for prayer experience",
            ItemAction.Drink => "Drink the item",
            ItemAction.Read => "Read the item",
            _ => $"Perform {action}"
        };
    }
}