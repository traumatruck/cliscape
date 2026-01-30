using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

public sealed class InventoryDropCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to drop")]
    public required string ItemName { get; init; }

    [CommandOption("-q|--quantity")]
    [Description("The quantity to drop (defaults to all)")]
    public int? Quantity { get; init; }
}