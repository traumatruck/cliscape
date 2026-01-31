using CliScape.Core.Items;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

/// <summary>
///     Lists all items in the player's inventory.
/// </summary>
public class InventoryListCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "list";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var inventory = player.Inventory;

        // Check if inventory is completely empty
        if (inventory.UsedSlots == 0)
        {
            AnsiConsole.MarkupLine("Your inventory is empty.");
            return (int)ExitCode.Success;
        }

        var table = new Table()
            .AddColumn("#")
            .AddColumn("Item")
            .AddColumn("Qty")
            .AddColumn("Value")
            .AddColumn("Actions");

        var slotNum = 1;

        foreach (var (item, quantity, _) in inventory.GetItems())
        {
            var totalValue = item.BaseValue * quantity;

            // Get available actions for the item
            var actionsText = GetAvailableActionsText(item);

            table.AddRow(
                slotNum.ToString(),
                item.Name.Value,
                quantity.ToString(),
                $"{totalValue:N0} gp",
                actionsText
            );

            slotNum++;
        }

        AnsiConsole.Write(table);
        return (int)ExitCode.Success;
    }

    /// <summary>
    ///     Gets a formatted string of available actions for an item.
    /// </summary>
    private static string GetAvailableActionsText(IItem item)
    {
        var actions = new List<string>();

        // All items can be examined and used (even if use does nothing interesting)
        actions.Add("examine");
        actions.Add("use");

        // Check for IActionableItem actions (excluding examine and use since we already added them)
        if (item is IActionableItem { Actions.Count: > 0 } actionable)
        {
            actions.AddRange(actionable.Actions
                .Where(action => action.ActionType != ItemAction.Examine && action.ActionType != ItemAction.Use)
                .Select(action => action.ActionType.ToString().ToLowerInvariant()));
        }

        // Check if item is equippable
        if (item is IEquippable equippable)
        {
            // Use "Wield" for weapons, "Equip" for other equipment
            var equipAction = equippable.Slot == EquipmentSlot.Weapon ? "wield" : "equip";
            actions.Add(equipAction);
        }

        return string.Join(", ", actions);
    }
}