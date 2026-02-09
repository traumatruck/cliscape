using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

public sealed class ChopCommandSettings : CommandSettings
{
    [CommandArgument(0, "[tree]")]
    [Description("The type of tree to chop (e.g., 'tree', 'oak', 'willow', 'maple', 'yew', 'magic')")]
    public string? TreeType { get; init; }
}