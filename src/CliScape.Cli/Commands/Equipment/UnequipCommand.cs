using CliScape.Core.Items;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Equipment;

/// <summary>
///     Unequip an item and return it to inventory.
/// </summary>
public class UnequipCommand : Command<UnequipCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "unequip";

    public override int Execute(CommandContext context, UnequipCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var inventory = player.Inventory;
        var equipment = player.Equipment;

        // Try to find by item name first
        var equipped = equipment.GetAllEquipped()
            .FirstOrDefault(e => e.Name.Value.Equals(settings.ItemOrSlot, StringComparison.OrdinalIgnoreCase));

        // If not found, try by slot name
        if (equipped is null && Enum.TryParse<EquipmentSlot>(settings.ItemOrSlot, true, out var slot))
        {
            equipped = equipment.GetEquipped(slot);
        }

        if (equipped is null)
        {
            AnsiConsole.MarkupLine($"[red]You don't have '{settings.ItemOrSlot}' equipped.[/]");
            return (int)ExitCode.Failure;
        }

        // Check inventory space
        if (!inventory.CanAdd(equipped))
        {
            AnsiConsole.MarkupLine("[red]Your inventory is full.[/]");
            return (int)ExitCode.Failure;
        }

        // Unequip and add to inventory
        equipment.Unequip(equipped.Slot);
        inventory.TryAdd(equipped);

        AnsiConsole.MarkupLine($"You unequip the [yellow]{equipped.Name}[/].");
        return (int)ExitCode.Success;
    }
}