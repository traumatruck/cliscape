using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

public sealed class InventoryExamineCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to examine")]
    public required string ItemName { get; init; }
}