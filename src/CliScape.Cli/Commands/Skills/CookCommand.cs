using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Cook raw food at a cooking range or fire.
/// </summary>
[Description("Cook raw food at a cooking range or fire")]
public class CookCommand : Command<CookCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "cook";

    public override int Execute(CommandContext context, CookCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Check if there's a cooking range here
        if (location.CookingRanges.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]There is no cooking range or fire here.[/]");
            return (int)ExitCode.Failure;
        }

        // If no food specified, list available recipes
        if (string.IsNullOrWhiteSpace(settings.FoodType))
        {
            return ListCookingRecipes(player, location.CookingRanges[0]);
        }

        // Find the matching recipe
        var recipe = CookingHelper.FindRecipeByName(settings.FoodType);
        if (recipe is null)
        {
            AnsiConsole.MarkupLine($"[red]Unknown food: '{settings.FoodType}'.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'skills cook' without arguments to see available recipes.[/]");
            return (int)ExitCode.Failure;
        }

        var requestedCount = settings.Count ?? 1;
        return CookFood(player, location.CookingRanges[0], recipe.Value, requestedCount);
    }

    private static int ListCookingRecipes(Player player, ICookingRange range)
    {
        var cookingLevel = player.GetSkillLevel(SkillConstants.CookingSkillName).Value;

        AnsiConsole.MarkupLine($"[bold]Cooking at: {range.Name}[/]");
        if (range.BurnChanceReduction > 0)
        {
            AnsiConsole.MarkupLine($"[dim]This range reduces burn chance by {range.BurnChanceReduction}%.[/]\n");
        }
        else
        {
            AnsiConsole.MarkupLine("[dim]Fires have no burn protection.[/]\n");
        }

        var table = new Table()
            .AddColumn("Raw Food")
            .AddColumn("Req. Level")
            .AddColumn("Experience")
            .AddColumn("Stop Burn Level")
            .AddColumn("In Inventory");

        foreach (var recipe in CookingHelper.Recipes)
        {
            var levelColor = cookingLevel >= recipe.RequiredLevel ? "green" : "red";
            var rawName = CookingHelper.GetRawFoodName(recipe.RawItemId);
            var count = player.Inventory.GetItems()
                .Where(s => s.Item.Id == recipe.RawItemId)
                .Sum(s => s.Quantity);

            table.AddRow(
                rawName,
                $"[{levelColor}]{recipe.RequiredLevel}[/]",
                recipe.Experience.ToString(),
                recipe.StopBurnLevel.ToString(),
                count.ToString()
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[dim]Use 'skills cook <food>' to cook (e.g., 'skills cook shrimps').[/]");
        AnsiConsole.MarkupLine("[dim]Use 'skills cook <food> -c <count>' to cook multiple items.[/]");

        return (int)ExitCode.Success;
    }

    private int CookFood(Player player, ICookingRange range, CookingRecipe recipe,
        int count)
    {
        var cookingSkill = player.GetSkill(SkillConstants.CookingSkillName);
        var cookingLevel = cookingSkill.Level.Value;

        // Check level requirement
        if (cookingLevel < recipe.RequiredLevel)
        {
            AnsiConsole.MarkupLine($"[red]You need level {recipe.RequiredLevel} Cooking to cook this.[/]");
            return (int)ExitCode.Failure;
        }

        // Get item references
        var rawItem = ItemRegistry.GetById(recipe.RawItemId);
        var cookedItem = ItemRegistry.GetById(recipe.CookedItemId);
        var burntItem = ItemRegistry.GetById(recipe.BurntItemId);

        if (rawItem is null || cookedItem is null || burntItem is null)
        {
            AnsiConsole.MarkupLine("[red]Something went wrong while cooking.[/]");
            return (int)ExitCode.Failure;
        }

        var itemsCooked = 0;
        var itemsBurnt = 0;
        var totalXp = 0;

        // Calculate burn chance
        var burnChance = CookingHelper.CalculateBurnChance(recipe, cookingLevel, range.BurnChanceReduction);

        for (var i = 0; i < count; i++)
        {
            // Check for raw food
            if (!HasItem(player, recipe.RawItemId))
            {
                if (itemsCooked == 0 && itemsBurnt == 0)
                {
                    AnsiConsole.MarkupLine($"[red]You don't have any {rawItem.Name}.[/]");
                    return (int)ExitCode.Failure;
                }

                AnsiConsole.MarkupLine($"[yellow]You ran out of {rawItem.Name}.[/]");
                break;
            }

            // Remove raw food
            RemoveItem(player, recipe.RawItemId, 1);

            // Check if it burns
            if (CookingHelper.DoesBurn(burnChance))
            {
                // Add burnt item
                player.Inventory.TryAdd(burntItem);
                itemsBurnt++;
            }
            else
            {
                // Add cooked item
                player.Inventory.TryAdd(cookedItem);

                // Grant experience
                Player.AddExperience(cookingSkill, recipe.Experience);
                itemsCooked++;
                totalXp += recipe.Experience;
            }
        }

        // Display results
        if (itemsCooked > 0)
        {
            if (itemsCooked == 1)
            {
                AnsiConsole.MarkupLine($"[green]You successfully cook the {rawItem.Name}.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]You successfully cook {itemsCooked}x {cookedItem.Name}.[/]");
            }

            AnsiConsole.MarkupLine($"[dim]You gain {totalXp:N0} Cooking experience.[/]");
        }

        if (itemsBurnt > 0)
        {
            if (itemsBurnt == 1)
            {
                AnsiConsole.MarkupLine($"[yellow]You accidentally burn 1x {rawItem.Name}.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]You accidentally burn {itemsBurnt}x {rawItem.Name}.[/]");
            }
        }

        if (itemsCooked == 0 && itemsBurnt == 0)
        {
            AnsiConsole.MarkupLine("[red]You didn't cook anything.[/]");
            return (int)ExitCode.Failure;
        }

        _gameState.Save();
        return (int)ExitCode.Success;
    }

    private static bool HasItem(Player player, ItemId itemId)
    {
        return player.Inventory.GetItems().Any(slot => slot.Item.Id == itemId);
    }

    private static void RemoveItem(Player player, ItemId itemId, int quantity)
    {
        var remaining = quantity;
        foreach (var slot in player.Inventory.GetItems().Where(s => s.Item.Id == itemId).ToList())
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