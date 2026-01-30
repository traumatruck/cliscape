using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Equipment;

public sealed class EquipCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to equip")]
    public required string ItemName { get; init; }
}