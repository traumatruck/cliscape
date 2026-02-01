using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

public sealed class ChopCommandSettings : CommandSettings
{
    [CommandArgument(0, "[tree]")]
    [Description("The type of tree to chop (e.g., 'tree', 'oak', 'willow', 'maple', 'yew', 'magic')")]
    public string? TreeType { get; init; }

    [CommandOption("-c|--count <count>")]
    [Description("Number of times to chop (limited by tree type and inventory space)")]
    public int? Count { get; init; }
}