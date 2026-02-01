using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

public sealed class CookCommandSettings : CommandSettings
{
    [CommandArgument(0, "[food]")]
    [Description("The raw food to cook (e.g., 'shrimps', 'trout', 'lobster', 'chicken', 'beef')")]
    public string? FoodType { get; init; }

    [CommandOption("-c|--count <count>")]
    [Description("Number of items to cook (limited by materials and inventory space)")]
    public int? Count { get; init; }
}