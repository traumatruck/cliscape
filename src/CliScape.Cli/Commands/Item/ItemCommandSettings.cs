using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Item;

public sealed class ItemCommandSettings : CommandSettings
{
    [CommandOption("-n|--name <name>")]
    [Description("The name of the item to interact with")]
    public string? ItemName { get; init; }

    [CommandOption("-s|--slot <slot>")]
    [Description("The inventory slot number of the item (1-28)")]
    public int? ItemSlot { get; init; }

    [CommandOption("-t|--target <target>")]
    [Description("The name of another item in your inventory to use the first item on")]
    public string? TargetItemName { get; init; }

    [CommandOption("-x|--examine")]
    [Description("Examine the item to see its description and stats")]
    public bool Examine { get; init; }

    [CommandOption("-e|--eat")]
    [Description("Eat the item to restore hitpoints")]
    public bool Eat { get; init; }

    [CommandOption("-b|--bury")]
    [Description("Bury the item to gain prayer experience")]
    public bool Bury { get; init; }

    [CommandOption("-u|--use")]
    [Description("Use the item (generic action)")]
    public bool Use { get; init; }

    [CommandOption("-d|--drink")]
    [Description("Drink the item (potions)")]
    public bool Drink { get; init; }

    [CommandOption("-r|--read")]
    [Description("Read the item (books, scrolls)")]
    public bool Read { get; init; }

    [CommandOption("-q|--equip")]
    [Description("Equip the item (weapons, armor, accessories)")]
    public bool Equip { get; init; }
}