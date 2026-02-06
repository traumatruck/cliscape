using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Bank;

public sealed class BankWithdrawCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to withdraw")]
    public required string ItemName { get; init; }

    [CommandOption("-q|--quantity")]
    [Description("The quantity to withdraw (defaults to 1)")]
    [DefaultValue(1)]
    public int Quantity { get; init; } = 1;

    [CommandOption("-a|--all")]
    [Description("Withdraw all of this item from the bank")]
    public bool All { get; init; }
}