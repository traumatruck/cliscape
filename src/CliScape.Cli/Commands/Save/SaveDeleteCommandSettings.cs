using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Save;

public class SaveDeleteCommandSettings : CommandSettings
{
    [CommandOption("-f|--force")]
    [Description("Skip confirmation prompt and delete save file immediately")]
    public bool Force { get; init; }
}