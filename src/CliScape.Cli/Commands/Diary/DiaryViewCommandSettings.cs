using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Diary;

public sealed class DiaryViewCommandSettings : CommandSettings
{
    [CommandArgument(0, "[location]")]
    [Description("The location name of the diary to view (defaults to current location)")]
    public string Location { get; init; } = string.Empty;
}