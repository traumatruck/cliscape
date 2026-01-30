using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

public sealed class ShopSellCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to sell")]
    public required string ItemName { get; init; }

    [CommandOption("-q|--quantity")]
    [Description("The quantity to sell (default: 1)")]
    public int? Quantity { get; init; }
}