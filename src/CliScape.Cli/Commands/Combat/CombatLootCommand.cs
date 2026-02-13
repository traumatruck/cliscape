using CliScape.Content.Items;
using CliScape.Core.Combat;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Combat;

/// <summary>
///     Pick up loot from defeated enemies.
/// </summary>
public class CombatLootCommand(GameState gameState) : Command<CombatLootCommandSettings>, ICommand
{
    public static string CommandName => "loot";

    public override int Execute(CommandContext context, CombatLootCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var pendingLoot = gameState.PendingLoot;
        var player = gameState.GetPlayer();

        // Check if there's any loot
        if (!pendingLoot.HasItems)
        {
            AnsiConsole.MarkupLine("[yellow]There's nothing to pick up.[/]");
            return (int)ExitCode.Success;
        }

        // If no item specified, show available loot
        if (string.IsNullOrEmpty(settings.ItemName))
        {
            return ShowAvailableLoot(pendingLoot);
        }

        // Handle 'all' keyword
        if (settings.ItemName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return LootAll(pendingLoot, player);
        }

        // Find the item by name
        return LootSpecificItem(pendingLoot, player, settings.ItemName, settings.Amount);
    }

    private static int ShowAvailableLoot(PendingLoot pendingLoot)
    {
        AnsiConsole.MarkupLine("[bold]Available loot:[/]");

        foreach (var lootItem in pendingLoot.Items)
        {
            var item = ItemRegistry.GetById(lootItem.ItemId);
            if (item == null)
            {
                continue;
            }

            var quantityText = lootItem.Quantity > 1 ? $" x{lootItem.Quantity}" : "";
            AnsiConsole.MarkupLine($"  [yellow]{item.Name}{quantityText}[/]");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Use 'combat loot <item>' or 'combat loot all' to pick up items.[/]");

        return (int)ExitCode.Success;
    }

    private static int LootAll(PendingLoot pendingLoot, Player player)
    {
        var itemsLooted = 0;
        var itemsSkipped = 0;

        // Create a copy of the list since we'll be modifying it
        var itemsToLoot = pendingLoot.Items.ToList();

        foreach (var lootItem in itemsToLoot)
        {
            var item = ItemRegistry.GetById(lootItem.ItemId);
            if (item == null)
            {
                continue;
            }

            if (player.Inventory.CanAdd(item, lootItem.Quantity))
            {
                player.Inventory.TryAdd(item, lootItem.Quantity);
                pendingLoot.Remove(lootItem.ItemId, lootItem.Quantity);

                var quantityText = lootItem.Quantity > 1 ? $" x{lootItem.Quantity}" : "";
                AnsiConsole.MarkupLine($"[green]+[/] [yellow]{item.Name}{quantityText}[/]");
                itemsLooted++;
            }
            else
            {
                var quantityText = lootItem.Quantity > 1 ? $" x{lootItem.Quantity}" : "";
                AnsiConsole.MarkupLine($"[red]![/] [dim]{item.Name}{quantityText} (inventory full)[/]");
                itemsSkipped++;
            }
        }

        if (itemsLooted == 0 && itemsSkipped > 0)
        {
            AnsiConsole.MarkupLine("[red]Your inventory is full![/]");
        }
        else if (itemsLooted > 0)
        {
            AnsiConsole.MarkupLine($"[green]Picked up {itemsLooted} item(s).[/]");
        }

        return (int)ExitCode.Success;
    }

    private static int LootSpecificItem(PendingLoot pendingLoot, Player player,
        string itemName, int? requestedAmount)
    {
        // Find matching item in pending loot
        LootItem? matchingLoot = null;
        IItem? matchingItem = null;

        foreach (var lootItem in pendingLoot.Items)
        {
            var item = ItemRegistry.GetById(lootItem.ItemId);
            if (item != null && item.Name.Value.Contains(itemName, StringComparison.OrdinalIgnoreCase))
            {
                matchingLoot = lootItem;
                matchingItem = item;
                break;
            }
        }

        if (matchingLoot == null || matchingItem == null)
        {
            AnsiConsole.MarkupLine($"[red]Could not find '{itemName}' in the loot.[/]");
            return (int)ExitCode.BadRequest;
        }

        // Determine quantity to pick up
        var quantityToLoot = requestedAmount ?? matchingLoot.Quantity;
        quantityToLoot = Math.Min(quantityToLoot, matchingLoot.Quantity);

        if (quantityToLoot <= 0)
        {
            AnsiConsole.MarkupLine("[yellow]Nothing to pick up.[/]");
            return (int)ExitCode.Success;
        }

        // Check inventory space
        if (!player.Inventory.CanAdd(matchingItem, quantityToLoot))
        {
            AnsiConsole.MarkupLine("[red]Your inventory is full![/]");
            return (int)ExitCode.BadRequest;
        }

        // Pick up the item
        player.Inventory.TryAdd(matchingItem, quantityToLoot);
        pendingLoot.Remove(matchingLoot.ItemId, quantityToLoot);

        var quantityText = quantityToLoot > 1 ? $" x{quantityToLoot}" : "";
        AnsiConsole.MarkupLine($"[green]+[/] [yellow]{matchingItem.Name}{quantityText}[/]");

        return (int)ExitCode.Success;
    }
}