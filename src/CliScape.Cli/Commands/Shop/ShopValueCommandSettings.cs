using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Shop;

public sealed class ShopValueCommandSettings : CommandSettings
{
    [CommandArgument(0, "<item>")]
    [Description("The name of the item to check value")]
    public required string ItemName { get; init; }
}