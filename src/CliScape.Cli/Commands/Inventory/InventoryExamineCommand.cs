using CliScape.Core.Items;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

/// <summary>
///     Examine an item in inventory to see its description and stats.
/// </summary>
public class InventoryExamineCommand : Command<InventoryExamineCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "examine";

    public override int Execute(CommandContext context, InventoryExamineCommandSettings settings,
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

        // Show examine text (classic OSRS style)
        AnsiConsole.MarkupLine($"[italic]{item.ExamineText}[/]");
        AnsiConsole.WriteLine();

        // Show basic info
        var infoTable = new Table().HideHeaders().NoBorder();
        infoTable.AddColumn("Property");
        infoTable.AddColumn("Value");

        infoTable.AddRow("Value", $"[yellow]{item.BaseValue:N0} gp[/]");
        infoTable.AddRow("Tradeable", item.IsTradeable ? "[green]Yes[/]" : "[red]No[/]");
        infoTable.AddRow("Stackable", item.IsStackable ? "[green]Yes[/]" : "[dim]No[/]");

        AnsiConsole.Write(infoTable);

        // If equippable, show equipment stats
        if (item is IEquippable equippable)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold]Equipment Stats ({equippable.Slot})[/]");

            ShowEquipmentStats(equippable);
            ShowRequirements(equippable);
        }

        return (int)ExitCode.Success;
    }

    private static void ShowEquipmentStats(IEquippable equippable)
    {
        var stats = equippable.Stats;

        // Attack bonuses
        var attackTable = new Table().Title("[bold]Attack Bonuses[/]").NoBorder();
        attackTable.AddColumn("Stab").AddColumn("Slash").AddColumn("Crush").AddColumn("Ranged").AddColumn("Magic");
        attackTable.AddRow(
            FormatBonus(stats.StabAttackBonus),
            FormatBonus(stats.SlashAttackBonus),
            FormatBonus(stats.CrushAttackBonus),
            FormatBonus(stats.RangedAttackBonus),
            FormatBonus(stats.MagicAttackBonus)
        );
        AnsiConsole.Write(attackTable);

        // Defence bonuses
        var defenceTable = new Table().Title("[bold]Defence Bonuses[/]").NoBorder();
        defenceTable.AddColumn("Stab").AddColumn("Slash").AddColumn("Crush").AddColumn("Ranged").AddColumn("Magic");
        defenceTable.AddRow(
            FormatBonus(stats.StabDefenceBonus),
            FormatBonus(stats.SlashDefenceBonus),
            FormatBonus(stats.CrushDefenceBonus),
            FormatBonus(stats.RangedDefenceBonus),
            FormatBonus(stats.MagicDefenceBonus)
        );
        AnsiConsole.Write(defenceTable);

        // Other bonuses
        var otherTable = new Table().Title("[bold]Other Bonuses[/]").NoBorder();
        otherTable.AddColumn("Melee Str").AddColumn("Ranged Str").AddColumn("Magic Dmg").AddColumn("Prayer");
        otherTable.AddRow(
            FormatBonus(stats.MeleeStrengthBonus),
            FormatBonus(stats.RangedStrengthBonus),
            $"{stats.MagicDamageBonus}%",
            FormatBonus(stats.PrayerBonus)
        );
        AnsiConsole.Write(otherTable);

        // Attack speed for weapons
        if (equippable.Slot == EquipmentSlot.Weapon)
        {
            AnsiConsole.MarkupLine($"Attack Speed: [cyan]{stats.AttackSpeed}[/] ({stats.AttackSpeed * 0.6:F1}s)");
        }
    }

    private static void ShowRequirements(IEquippable equippable)
    {
        var hasRequirements = equippable.RequiredAttackLevel > 0 ||
                              equippable.RequiredStrengthLevel > 0 ||
                              equippable.RequiredDefenceLevel > 0 ||
                              equippable.RequiredRangedLevel > 0 ||
                              equippable.RequiredMagicLevel > 0;

        if (!hasRequirements)
        {
            return;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold]Requirements[/]");

        if (equippable.RequiredAttackLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Attack: [cyan]{equippable.RequiredAttackLevel}[/]");
        }

        if (equippable.RequiredStrengthLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Strength: [cyan]{equippable.RequiredStrengthLevel}[/]");
        }

        if (equippable.RequiredDefenceLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Defence: [cyan]{equippable.RequiredDefenceLevel}[/]");
        }

        if (equippable.RequiredRangedLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Ranged: [cyan]{equippable.RequiredRangedLevel}[/]");
        }

        if (equippable.RequiredMagicLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Magic: [cyan]{equippable.RequiredMagicLevel}[/]");
        }
    }

    private static string FormatBonus(int bonus)
    {
        return bonus switch
        {
            > 0 => $"[green]+{bonus}[/]",
            < 0 => $"[red]{bonus}[/]",
            _ => "[dim]0[/]"
        };
    }
}