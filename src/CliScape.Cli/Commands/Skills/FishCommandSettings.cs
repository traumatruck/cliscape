using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

public sealed class FishCommandSettings : CommandSettings
{
    [CommandArgument(0, "[spot]")]
    [Description("The type of fishing spot to fish at (e.g., 'net', 'lure', 'cage', 'harpoon')")]
    public string? SpotType { get; init; }

    [CommandOption("-c|--count <count>")]
    [Description("Number of times to fish (limited by spot type and inventory space)")]
    public int? Count { get; init; }
}