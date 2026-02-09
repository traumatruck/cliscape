using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Slayer;

/// <summary>
///     Settings for the slayer command.
/// </summary>
public class SlayerCommandSettings : CommandSettings
{
    [CommandOption("-m|--master <MASTER>")]
    [Description("The slayer master to get a task from")]
    public string? Master { get; set; }

    [CommandOption("-c|--cancel")]
    [Description("Cancel your current slayer task")]
    public bool Cancel { get; set; }
}