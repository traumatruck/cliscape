using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

public sealed class SmeltCommandSettings : CommandSettings
{
    [CommandArgument(0, "[bar]")]
    [Description("The type of bar to smelt (e.g., 'bronze', 'iron', 'steel', 'mithril', 'adamantite', 'runite')")]
    public string? BarType { get; init; }

    [CommandOption("-c|--count <count>")]
    [Description("Number of bars to smelt (limited by materials and inventory space)")]
    public int? Count { get; init; }
}