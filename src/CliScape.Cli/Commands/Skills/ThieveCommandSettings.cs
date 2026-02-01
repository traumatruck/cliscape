using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

public sealed class ThieveCommandSettings : CommandSettings
{
    [CommandArgument(0, "[target]")]
    [Description("The target to steal from (e.g., 'bakery', 'silk', 'man', 'guard')")]
    public string? Target { get; init; }

    [CommandOption("-c|--count <count>")]
    [Description("Number of times to attempt thieving")]
    public int? Count { get; init; }
}