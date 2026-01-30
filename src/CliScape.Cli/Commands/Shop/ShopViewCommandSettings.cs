using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

public sealed class ShopViewCommandSettings : CommandSettings
{
    [CommandArgument(0, "<shop>")]
    [Description("The name (or part of name) of the shop to view")]
    public required string ShopName { get; init; }
}