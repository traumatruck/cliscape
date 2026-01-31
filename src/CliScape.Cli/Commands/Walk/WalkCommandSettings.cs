using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Walk;

public class WalkCommandSettings : CommandSettings
{
    [CommandArgument(0, "<location>")]
    [Description("The name of the location to walk to")]
    public required string LocationName { get; init; }
}