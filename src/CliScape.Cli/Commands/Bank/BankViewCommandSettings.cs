using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Bank;

public sealed class BankViewCommandSettings : CommandSettings
{
    [CommandOption("-p|--page")]
    [Description("The page number to display (28 items per page)")]
    [DefaultValue(1)]
    public int Page { get; init; } = 1;
}