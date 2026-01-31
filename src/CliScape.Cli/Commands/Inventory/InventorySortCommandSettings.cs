using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

public sealed class InventorySortCommandSettings : CommandSettings
{
    [CommandArgument(0, "<from>")]
    [Description("The slot number to move from (1-28)")]
    public required int FromSlot { get; init; }

    [CommandArgument(1, "<to>")]
    [Description("The slot number to move to (1-28)")]
    public required int ToSlot { get; init; }

    [CommandOption("-i|--insert")]
    [Description("Insert the item at the target position, shifting other items")]
    public bool Insert { get; init; }
}