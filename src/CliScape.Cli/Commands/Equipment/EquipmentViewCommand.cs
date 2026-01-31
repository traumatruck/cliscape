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

        // Equipment table
        var equipmentTable = new Table()
            .AddColumn("Slot")
            .AddColumn("Item");

        foreach (var slot in Enum.GetValues<EquipmentSlot>())
        {
            var equipped = equipment.GetEquipped(slot);
            var itemDisplay = equipped is not null
                ? $"[yellow]{equipped.Name}[/]"
                : "[dim]Empty[/]";
            equipmentTable.AddRow(slot.ToString(), itemDisplay);
        }

        // Bonuses table
        var bonusesTable = new Table()
            .AddColumn("Stat")
            .AddColumn("Attack")
            .AddColumn("Defence");

        bonusesTable.AddRow("Stab", FormatBonus(equipment.TotalStabAttackBonus), FormatBonus(equipment.TotalStabDefenceBonus));
        bonusesTable.AddRow("Slash", FormatBonus(equipment.TotalSlashAttackBonus), FormatBonus(equipment.TotalSlashDefenceBonus));
        bonusesTable.AddRow("Crush", FormatBonus(equipment.TotalCrushAttackBonus), FormatBonus(equipment.TotalCrushDefenceBonus));
        bonusesTable.AddRow("Ranged", FormatBonus(equipment.TotalRangedAttackBonus), FormatBonus(equipment.TotalRangedDefenceBonus));
        bonusesTable.AddRow("Magic", FormatBonus(equipment.TotalMagicAttackBonus), FormatBonus(equipment.TotalMagicDefenceBonus));
        bonusesTable.AddEmptyRow();
        bonusesTable.AddRow("Melee Strength", FormatBonus(equipment.TotalMeleeStrengthBonus), "");
        bonusesTable.AddRow("Ranged Strength", FormatBonus(equipment.TotalRangedStrengthBonus), "");
        bonusesTable.AddRow("Magic Damage", $"{equipment.TotalMagicDamageBonus}%", "");
        bonusesTable.AddRow("Prayer", FormatBonus(equipment.TotalPrayerBonus), "");
        bonusesTable.AddRow("Weapon Speed", $"[cyan]{equipment.WeaponAttackSpeed}[/] ({equipment.WeaponAttackSpeed * 0.6:F1}s)", "");

        AnsiConsole.Write(new Columns(equipmentTable, bonusesTable).Collapse());

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