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
        var actionType = DetermineAction(settings);

        if (actionType is null)
        {
            return ShowAvailableActions(item);
        }

        return ExecuteAction(item, actionType.Value, player, inventory);
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

        if (settings.Equip)
        {
            return ItemAction.Equip;
        }

        if (settings.Wield)
        {
            return ItemAction.Wield;
        }

        return null;
    }

    private static int ShowAvailableActions(IItem item)
    {
        if (item is not IActionableItem actionableItem || actionableItem.Actions.Count == 0)
        {
            AnsiConsole.MarkupLine($"[yellow]{item.Name} has no special actions available.[/]");
            AnsiConsole.MarkupLine("[dim]You can examine or drop it from your inventory.[/]");
            return (int)ExitCode.Success;
        }

        AnsiConsole.MarkupLine($"[yellow]{item.Name}[/] can be used with the following actions:");
        foreach (var action in actionableItem.Actions)
        {
            var actionFlag = GetActionFlag(action.ActionType);
            AnsiConsole.MarkupLine($"  [green]--{actionFlag}[/] - {action.Description}");
        }

        return (int)ExitCode.Success;
    }

    private int ExecuteAction(IItem item, ItemAction actionType, Player player, Core.Items.Inventory inventory)
    {
        // Special handling for equip/wield (not part of the IItemAction system)
        if (actionType is ItemAction.Equip or ItemAction.Wield)
        {
            return HandleEquip(item, player, inventory);
        }

        // Get the action from the item
        if (item is not IActionableItem actionableItem)
        {
            AnsiConsole.MarkupLine($"[red]You can't {actionType.ToString().ToLower()} a {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        var action = actionableItem.GetAction(actionType);
        if (action is null)
        {
            AnsiConsole.MarkupLine($"[red]You can't {actionType.ToString().ToLower()} a {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        // Remove the item if the action consumes it
        if (action.ConsumesItem)
        {
            var removed = inventory.Remove(item);
            if (removed == 0)
            {
                AnsiConsole.MarkupLine("[red]Failed to use the item.[/]");
                return (int)ExitCode.Failure;
            }
        }

        // Execute the action
        var message = action.Execute(item, player);
        AnsiConsole.MarkupLine($"[green]{message}[/]");

        // Show health for eat actions
        if (actionType == ItemAction.Eat)
        {
            AnsiConsole.MarkupLine($"[dim]Health: {player.CurrentHealth}/{player.MaximumHealth}[/]");
        }

        return (int)ExitCode.Success;
    }

    private int HandleEquip(IItem item, Player player, Core.Items.Inventory inventory)
    {
        if (item is not IEquippable equippable)
        {
            AnsiConsole.MarkupLine($"[red]You cannot equip {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        // Check requirements
        if (!MeetsRequirements(player, equippable, out var failMessage))
        {
            AnsiConsole.MarkupLine($"[red]{failMessage}[/]");
            return (int)ExitCode.Failure;
        }

        // Remove from inventory
        inventory.Remove(item);

        // Equip the item, getting any previously equipped item back
        var previous = player.Equipment.Equip(equippable);

        // If there was a previous item, put it in inventory
        if (previous is not null)
        {
            AnsiConsole.MarkupLine(!inventory.TryAdd(previous)
                ? $"[yellow]Warning: Could not return {previous.Name} to inventory.[/]"
                : $"You unequip your [yellow]{previous.Name}[/].");
        }

        AnsiConsole.MarkupLine($"You equip the [yellow]{equippable.Name}[/].");
        return (int)ExitCode.Success;
    }

    private static bool MeetsRequirements(Player player, IEquippable equippable, out string message)
    {
        var skills = player.Skills;

        var attack = skills.FirstOrDefault(s => s.Name.Name == "Attack")?.Level.Value ?? 1;
        var strength = skills.FirstOrDefault(s => s.Name.Name == "Strength")?.Level.Value ?? 1;
        var defence = skills.FirstOrDefault(s => s.Name.Name == "Defence")?.Level.Value ?? 1;
        var ranged = skills.FirstOrDefault(s => s.Name.Name == "Ranged")?.Level.Value ?? 1;
        var magic = skills.FirstOrDefault(s => s.Name.Name == "Magic")?.Level.Value ?? 1;

        if (equippable.RequiredAttackLevel > attack)
        {
            message = $"You need {equippable.RequiredAttackLevel} Attack to equip this.";
            return false;
        }

        if (equippable.RequiredStrengthLevel > strength)
        {
            message = $"You need {equippable.RequiredStrengthLevel} Strength to equip this.";
            return false;
        }

        if (equippable.RequiredDefenceLevel > defence)
        {
            message = $"You need {equippable.RequiredDefenceLevel} Defence to equip this.";
            return false;
        }

        if (equippable.RequiredRangedLevel > ranged)
        {
            message = $"You need {equippable.RequiredRangedLevel} Ranged to equip this.";
            return false;
        }

        if (equippable.RequiredMagicLevel > magic)
        {
            message = $"You need {equippable.RequiredMagicLevel} Magic to equip this.";
            return false;
        }

        message = string.Empty;
        return true;
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
            ItemAction.Equip => "equip",
            ItemAction.Wield => "wield",
            _ => action.ToString().ToLower()
        };
    }
}