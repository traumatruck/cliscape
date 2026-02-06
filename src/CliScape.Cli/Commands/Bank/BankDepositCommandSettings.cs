using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Bank;

public sealed class BankDepositCommandSettings : CommandSettings
{
    [CommandArgument(0, "[item]")]
    [Description("The name of the item to deposit")]
    public string ItemName { get; init; } = string.Empty;

    [CommandOption("-q|--quantity")]
    [Description("The quantity to deposit (defaults to all of that item)")]
    public int? Quantity { get; init; }

    [CommandOption("-a|--all")]
    [Description("Deposit all items from inventory")]
    public bool All { get; init; }

    public override ValidationResult Validate()
    {
        if (All && !string.IsNullOrEmpty(ItemName))
        {
            return ValidationResult.Error("Cannot specify both --all and an item name.");
        }

        if (!All && string.IsNullOrEmpty(ItemName))
        {
            return ValidationResult.Error("Must specify either an item name or --all.");
        }

        return ValidationResult.Success();
    }
}