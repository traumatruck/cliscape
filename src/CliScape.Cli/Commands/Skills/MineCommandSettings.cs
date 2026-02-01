using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

public sealed class MineCommandSettings : CommandSettings
{
    [CommandArgument(0, "[rock]")]
    [Description("The type of rock to mine (e.g., 'copper', 'tin', 'iron', 'coal', 'mithril', 'adamantite', 'runite')")]
    public string? RockType { get; init; }

    [CommandOption("-c|--count <count>")]
    [Description("Number of times to mine (limited by rock type and inventory space)")]
    public int? Count { get; init; }
}