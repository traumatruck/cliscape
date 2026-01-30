using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Equipment;

/// <summary>
///     Equip an item from inventory.
/// </summary>
public class EquipCommand : Command<EquipCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "equip";

    public override int Execute(CommandContext context, EquipCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var inventory = player.Inventory;
        var equipment = player.Equipment;

        // Find the item in inventory
        var itemEntry = inventory.GetItems()
            .FirstOrDefault(i => i.Item.Name.Value.Equals(settings.ItemName, StringComparison.OrdinalIgnoreCase));

        if (itemEntry == default)
        {
            AnsiConsole.MarkupLine($"[red]You don't have any '{settings.ItemName}' in your inventory.[/]");
            return (int)ExitCode.Failure;
        }

        var item = itemEntry.Item;

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
        var previous = equipment.Equip(equippable);

        // If there was a previous item, put it in inventory
        if (previous is not null)
        {
            // This shouldn't happen since we just removed an item, but handle it
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
}