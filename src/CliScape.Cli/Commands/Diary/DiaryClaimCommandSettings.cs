using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Diary;

public sealed class DiaryClaimCommandSettings : CommandSettings
{
    [CommandArgument(0, "<location>")]
    [Description("The location name of the diary")]
    public string Location { get; init; } = string.Empty;

    [CommandArgument(1, "<tier>")]
    [Description("The tier to claim rewards for (easy, medium, hard, elite)")]
    public string Tier { get; init; } = string.Empty;

    [CommandOption("--skill")]
    [Description("The skill to apply experience lamp rewards to")]
    public string? Skill { get; init; }
}