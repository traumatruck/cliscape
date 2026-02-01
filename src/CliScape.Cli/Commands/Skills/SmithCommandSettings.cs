using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

public sealed class SmithCommandSettings : CommandSettings
{
    [CommandArgument(0, "[bar]")]
    [Description("The type of bar to smith with (e.g., 'bronze', 'iron', 'steel')")]
    public string? BarType { get; init; }

    [CommandArgument(1, "[item]")]
    [Description("The item to smith (e.g., 'dagger', 'scimitar', 'platebody')")]
    public string? ItemType { get; init; }

    [CommandOption("-c|--count <count>")]
    [Description("Number of items to smith (limited by materials and inventory space)")]
    public int? Count { get; init; }
}