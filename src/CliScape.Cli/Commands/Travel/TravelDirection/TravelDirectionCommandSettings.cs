using System.ComponentModel;
using CliScape.Core.World;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Travel.TravelDirection;

public class TravelDirectionCommandSettings : CommandSettings
{
    [CommandArgument(0, "[Direction]")]
    [Description("The direction to travel (North, East, South, West)")]
    public Direction Direction { get; init; }
}
