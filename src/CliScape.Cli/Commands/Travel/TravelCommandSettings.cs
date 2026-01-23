using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Travel;

public class TravelCommandSettings : CommandSettings
{
    [Description("Lists the adjacent locations the player can travel too.")]
    public string List { get; set; } = string.Empty;
}