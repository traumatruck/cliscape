using CliScape.Core.Items;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Equipment;

/// <summary>
///     View currently equipped items and total equipment stats.
/// </summary>
public class EquipmentViewCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "view";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var equipment = player.Equipment;

        AnsiConsole.MarkupLine("[bold]Equipment[/]");
        AnsiConsole.WriteLine();

        // Show equipped items by slot
        var slotsTable = new Table()
            .AddColumn("Slot")
            .AddColumn("Item");

        foreach (var slot in Enum.GetValues<EquipmentSlot>())
        {
            var equipped = equipment.GetEquipped(slot);
            var itemDisplay = equipped is not null
                ? $"[yellow]{equipped.Name}[/]"
                : "[dim]Empty[/]";
            slotsTable.AddRow(slot.ToString(), itemDisplay);
        }

        AnsiConsole.Write(slotsTable);
        AnsiConsole.WriteLine();

        // Show total bonuses
        AnsiConsole.MarkupLine("[bold]Total Bonuses[/]");

        // Attack bonuses
        var attackTable = new Table().Title("[bold]Attack Bonuses[/]").NoBorder();
        attackTable.AddColumn("Stab").AddColumn("Slash").AddColumn("Crush").AddColumn("Ranged").AddColumn("Magic");
        attackTable.AddRow(
            FormatBonus(equipment.TotalStabAttackBonus),
            FormatBonus(equipment.TotalSlashAttackBonus),
            FormatBonus(equipment.TotalCrushAttackBonus),
            FormatBonus(equipment.TotalRangedAttackBonus),
            FormatBonus(equipment.TotalMagicAttackBonus)
        );
        AnsiConsole.Write(attackTable);

        // Defence bonuses
        var defenceTable = new Table().Title("[bold]Defence Bonuses[/]").NoBorder();
        defenceTable.AddColumn("Stab").AddColumn("Slash").AddColumn("Crush").AddColumn("Ranged").AddColumn("Magic");
        defenceTable.AddRow(
            FormatBonus(equipment.TotalStabDefenceBonus),
            FormatBonus(equipment.TotalSlashDefenceBonus),
            FormatBonus(equipment.TotalCrushDefenceBonus),
            FormatBonus(equipment.TotalRangedDefenceBonus),
            FormatBonus(equipment.TotalMagicDefenceBonus)
        );
        AnsiConsole.Write(defenceTable);

        // Other bonuses
        var otherTable = new Table().Title("[bold]Other Bonuses[/]").NoBorder();
        otherTable.AddColumn("Melee Str").AddColumn("Ranged Str").AddColumn("Magic Dmg").AddColumn("Prayer");
        otherTable.AddRow(
            FormatBonus(equipment.TotalMeleeStrengthBonus),
            FormatBonus(equipment.TotalRangedStrengthBonus),
            $"{equipment.TotalMagicDamageBonus}%",
            FormatBonus(equipment.TotalPrayerBonus)
        );
        AnsiConsole.Write(otherTable);

        // Weapon speed
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(
            $"Weapon Speed: [cyan]{equipment.WeaponAttackSpeed}[/] ({equipment.WeaponAttackSpeed * 0.6:F1}s)");

        return (int)ExitCode.Success;
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