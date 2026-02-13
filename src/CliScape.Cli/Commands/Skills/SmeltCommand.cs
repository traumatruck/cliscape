using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Smelt ores into bars at a furnace.
/// </summary>
[Description("Smelt ores into bars at a furnace")]
public class SmeltCommand(GameState gameState) : Command<SmeltCommandSettings>, ICommand
{
    public static string CommandName => "smelt";

    public override int Execute(CommandContext context, SmeltCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Check if there's a furnace here
        if (location.Furnaces.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]There is no furnace here.[/]");
            return (int)ExitCode.Failure;
        }

        // If no bar specified, list available recipes
        if (string.IsNullOrWhiteSpace(settings.BarType))
        {
            return ListSmeltingRecipes(player);
        }

        // Find the matching recipe
        var recipe = SmithingHelper.FindSmeltingRecipe(settings.BarType);
        if (recipe is null)
        {
            AnsiConsole.MarkupLine($"[red]Unknown bar type: '{settings.BarType}'.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'skills smelt' without arguments to see available recipes.[/]");
            return (int)ExitCode.Failure;
        }

        var requestedCount = settings.Count ?? 1;
        return SmeltBars(player, recipe.Value, requestedCount);
    }

    private static int ListSmeltingRecipes(Player player)
    {
        var smithingLevel = player.GetSkillLevel(SkillConstants.SmithingSkillName).Value;

        AnsiConsole.MarkupLine("[bold]Smelting recipes:[/]\n");

        var table = new Table()
            .AddColumn("Bar")
            .AddColumn("Req. Level")
            .AddColumn("Experience")
            .AddColumn("Materials");

        foreach (var recipe in SmithingHelper.SmeltingRecipes)
        {
            var levelColor = smithingLevel >= recipe.RequiredLevel ? "green" : "red";
            var barName = ItemRegistry.GetById(recipe.ResultBarId)?.Name.Value ?? "Unknown";

            var materials = SmithingHelper.GetOreName(recipe.PrimaryOreId);
            if (recipe.SecondaryOreId is not null)
            {
                materials += $" + {recipe.SecondaryOreCount}x {SmithingHelper.GetOreName(recipe.SecondaryOreId.Value)}";
            }

            table.AddRow(
                barName,
                $"[{levelColor}]{recipe.RequiredLevel}[/]",
                recipe.Experience.ToString(),
                materials
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[dim]Use 'skills smelt <bar>' to smelt bars (e.g., 'skills smelt bronze').[/]");
        AnsiConsole.MarkupLine("[dim]Use 'skills smelt <bar> -c <count>' to smelt multiple bars.[/]");

        return (int)ExitCode.Success;
    }

    private int SmeltBars(Player player, SmeltingRecipe recipe, int count)
    {
        var smithingSkill = player.GetSkill(SkillConstants.SmithingSkillName);
        var smithingLevel = smithingSkill.Level.Value;

        // Check level requirement
        if (smithingLevel < recipe.RequiredLevel)
        {
            AnsiConsole.MarkupLine($"[red]You need level {recipe.RequiredLevel} Smithing to smelt this bar.[/]");
            return (int)ExitCode.Failure;
        }

        // Get item references
        var barItem = ItemRegistry.GetById(recipe.ResultBarId);
        var primaryOre = ItemRegistry.GetById(recipe.PrimaryOreId);
        var secondaryOre = recipe.SecondaryOreId is not null ? ItemRegistry.GetById(recipe.SecondaryOreId.Value) : null;

        if (barItem is null || primaryOre is null)
        {
            AnsiConsole.MarkupLine("[red]Something went wrong while smelting.[/]");
            return (int)ExitCode.Failure;
        }

        var barsSmelted = 0;
        var totalXp = 0;

        for (var i = 0; i < count; i++)
        {
            // Check inventory space
            if (player.Inventory.IsFull)
            {
                if (barsSmelted == 0)
                {
                    AnsiConsole.MarkupLine("[red]Your inventory is full.[/]");
                    return (int)ExitCode.Failure;
                }

                AnsiConsole.MarkupLine("[yellow]Your inventory is full.[/]");
                break;
            }

            // Check for primary ore
            if (!HasItem(player, recipe.PrimaryOreId))
            {
                if (barsSmelted == 0)
                {
                    AnsiConsole.MarkupLine($"[red]You don't have any {primaryOre.Name}.[/]");
                    return (int)ExitCode.Failure;
                }

                AnsiConsole.MarkupLine($"[yellow]You ran out of {primaryOre.Name}.[/]");
                break;
            }

            // Check for secondary ore if needed
            if (recipe.SecondaryOreId is not null)
            {
                var secondaryCount = CountItem(player, recipe.SecondaryOreId.Value);
                if (secondaryCount < recipe.SecondaryOreCount)
                {
                    if (barsSmelted == 0)
                    {
                        AnsiConsole.MarkupLine(
                            $"[red]You need {recipe.SecondaryOreCount}x {secondaryOre?.Name.Value ?? "secondary ore"} but only have {secondaryCount}.[/]");
                        return (int)ExitCode.Failure;
                    }

                    AnsiConsole.MarkupLine($"[yellow]You ran out of {secondaryOre?.Name.Value ?? "secondary ore"}.[/]");
                    break;
                }
            }

            // Remove ores
            RemoveItem(player, recipe.PrimaryOreId, 1);
            if (recipe.SecondaryOreId is not null)
            {
                RemoveItem(player, recipe.SecondaryOreId.Value, recipe.SecondaryOreCount);
            }

            // Add bar to inventory
            player.Inventory.TryAdd(barItem);

            // Grant experience
            Player.AddExperience(smithingSkill, recipe.Experience);
            barsSmelted++;
            totalXp += recipe.Experience;
        }

        if (barsSmelted == 1)
        {
            AnsiConsole.MarkupLine($"[green]You smelt the ore and create a {barItem.Name}.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]You smelt the ores and create {barsSmelted}x {barItem.Name}.[/]");
        }

        AnsiConsole.MarkupLine($"[dim]You gain {totalXp:N0} Smithing experience.[/]");

        return (int)ExitCode.Success;
    }

    private static bool HasItem(Player player, ItemId itemId)
    {
        return player.Inventory.GetItems().Any(slot => slot.Item.Id == itemId);
    }

    private static int CountItem(Player player, ItemId itemId)
    {
        return player.Inventory.GetItems()
            .Where(slot => slot.Item.Id == itemId)
            .Sum(slot => slot.Quantity);
    }

    private static void RemoveItem(Player player, ItemId itemId, int quantity)
    {
        var remaining = quantity;
        foreach (var slot in player.Inventory.GetItems().Where(s => s.Item.Id == itemId))
        {
            var toRemove = Math.Min(slot.Quantity, remaining);
            player.Inventory.Remove(slot.Item, toRemove);
            remaining -= toRemove;
            if (remaining <= 0)
            {
                break;
            }
        }
    }
}