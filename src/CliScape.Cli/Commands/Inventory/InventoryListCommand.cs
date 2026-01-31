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

        var table = new Table()
            .AddColumn("#")
            .AddColumn("Item")
            .AddColumn("Qty")
            .AddColumn("Value")
            .AddColumn("Actions");

        // Show all 28 slots, including empty ones
        for (var i = 0; i < Core.Items.Inventory.MaxSlots; i++)
        {
            var slot = inventory.GetSlot(i);
            var slotNum = i + 1;

            if (slot.IsEmpty)
            {
                table.AddRow(
                    $"[dim]{slotNum}[/]",
                    "[dim]---[/]",
                    "[dim]-[/]",
                    "[dim]-[/]",
                    "[dim]-[/]"
                );
            }
            else
            {
                var item = slot.Item!;
                var quantity = slot.Quantity;
                var totalValue = item.BaseValue * quantity;
                var actionsText = GetAvailableActionsText(item);

                table.AddRow(
                    slotNum.ToString(),
                    item.Name.Value,
                    quantity.ToString(),
                    $"{totalValue:N0} gp",
                    actionsText
                );
            }
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
        if (item is IEquippable)
        {
            actions.Add("equip");
        }

        return string.Join(", ", actions);
    }
}