using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Equipment;

public sealed class UnequipCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item|slot>")]
    [Description("The name of the item or equipment slot to unequip")]
    public required string ItemOrSlot { get; init; }
}