using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Combat;

/// <summary>
///     Settings for the combat loot command.
/// </summary>
public class CombatLootCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to pick up, or 'all' to take everything")]
    public string? ItemName { get; set; }

    [CommandArgument(1, "<amount>")]
    [Description("The amount to pick up (default: all available)")]
    public int? Amount { get; set; }
}
