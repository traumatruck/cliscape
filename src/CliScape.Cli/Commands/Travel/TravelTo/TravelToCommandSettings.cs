using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Travel.TravelTo;

public class TravelToCommandSettings : CommandSettings
{
    [CommandArgument(0, "[Location Name]")]
    [Description("The name of the location to travel to")]
    public string LocationName { get; init; } = string.Empty;
}