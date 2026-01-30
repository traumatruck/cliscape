using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

public sealed class ShopBuyCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to buy")]
    public required string ItemName { get; init; }

    [CommandOption("-q|--quantity")]
    [Description("The quantity to buy (default: 1)")]
    [DefaultValue(1)]
    public int Quantity { get; init; } = 1;
}