using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Inventory;

public sealed class InventoryInteractCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to interact with")]
    public required string ItemName { get; init; }

    [CommandOption("--eat")]
    [Description("Eat the item to restore hitpoints")]
    public bool Eat { get; init; }

    [CommandOption("--bury")]
    [Description("Bury the item to gain prayer experience")]
    public bool Bury { get; init; }

    [CommandOption("--use")]
    [Description("Use the item (generic action)")]
    public bool Use { get; init; }

    [CommandOption("--drink")]
    [Description("Drink the item (potions)")]
    public bool Drink { get; init; }

    [CommandOption("--read")]
    [Description("Read the item (books, scrolls)")]
    public bool Read { get; init; }
}