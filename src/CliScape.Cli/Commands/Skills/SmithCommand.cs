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
///     Smith bars into equipment at an anvil.
/// </summary>
[Description("Smith bars into equipment at an anvil")]
public class SmithCommand(GameState gameState) : Command<SmithCommandSettings>, ICommand
{
    public static string CommandName => "smith";

    public override int Execute(CommandContext context, SmithCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Check if there's an anvil here
        if (location.Anvils.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]There is no anvil here.[/]");
            return (int)ExitCode.Failure;
        }

        // Check for hammer
        if (!SmithingHelper.HasHammer(player))
        {
            AnsiConsole.MarkupLine("[red]You need a hammer to smith.[/]");
            return (int)ExitCode.Failure;
        }

        // If no bar specified, list available bar types
        if (string.IsNullOrWhiteSpace(settings.BarType))
        {
            return ListBarTypes(player);
        }

        // Get bar ID from name
        var barId = SmithingHelper.GetBarIdFromName(settings.BarType);
        if (barId is null)
        {
            AnsiConsole.MarkupLine($"[red]Unknown bar type: '{settings.BarType}'.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'skills smith' without arguments to see available bar types.[/]");
            return (int)ExitCode.Failure;
        }

        // If no item specified, list available recipes for this bar
        if (string.IsNullOrWhiteSpace(settings.ItemType))
        {
            return ListSmithingRecipes(player, barId.Value);
        }

        // Find the matching recipe
        var recipe = SmithingHelper.FindSmithingRecipe(barId.Value, settings.ItemType);
        if (recipe is null)
        {
            AnsiConsole.MarkupLine($"[red]Unknown item: '{settings.ItemType}'.[/]");
            AnsiConsole.MarkupLine(
                $"[dim]Use 'skills smith {settings.BarType}' to see available items for this bar type.[/]");
            return (int)ExitCode.Failure;
        }

        var requestedCount = settings.Count ?? 1;
        return SmithItems(player, barId.Value, recipe.Value, requestedCount);
    }

    private static int ListBarTypes(Player player)
    {
        AnsiConsole.MarkupLine("[bold]Available bar types for smithing:[/]\n");

        var smithingLevel = player.GetSkillLevel(SkillConstants.SmithingSkillName).Value;

        var table = new Table()
            .AddColumn("Bar")
            .AddColumn("Min Level")
            .AddColumn("In Inventory");

        var barTypes = new[]
        {
            ("Bronze", 1, new ItemId(1500)),
            ("Iron", 15, new ItemId(1501)),
            ("Steel", 30, new ItemId(1502)),
            ("Mithril", 50, new ItemId(1503)),
            ("Adamantite", 70, new ItemId(1504)),
            ("Runite", 85, new ItemId(1505))
        };

        foreach (var (name, minLevel, barId) in barTypes)
        {
            var levelColor = smithingLevel >= minLevel ? "green" : "red";
            var count = player.Inventory.GetItems()
                .Where(s => s.Item.Id == barId)
                .Sum(s => s.Quantity);

            table.AddRow(
                name,
                $"[{levelColor}]{minLevel}[/]",
                count.ToString()
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine(
            "\n[dim]Use 'skills smith <bar>' to see recipes (e.g., 'skills smith bronze').[/]");
        AnsiConsole.MarkupLine(
            "[dim]Use 'skills smith <bar> <item>' to smith an item (e.g., 'skills smith bronze dagger').[/]");

        return (int)ExitCode.Success;
    }

    private static int ListSmithingRecipes(Player player, ItemId barId)
    {
        var recipes = SmithingHelper.GetRecipesForBar(barId);
        if (recipes.Length == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No smithing recipes available for this bar type yet.[/]");
            return (int)ExitCode.Success;
        }

        var barItem = ItemRegistry.GetById(barId);
        var barName = barItem?.Name.Value ?? "Unknown bar";
        var smithingLevel = player.GetSkillLevel(SkillConstants.SmithingSkillName).Value;

        AnsiConsole.MarkupLine($"[bold]Smithing recipes for {barName}:[/]\n");

        var table = new Table()
            .AddColumn("Item")
            .AddColumn("Req. Level")
            .AddColumn("Bars Needed")
            .AddColumn("Experience");

        foreach (var recipe in recipes)
        {
            var levelColor = smithingLevel >= recipe.RequiredLevel ? "green" : "red";
            var itemName = ItemRegistry.GetById(recipe.ResultItemId)?.Name.Value ?? "Unknown";

            table.AddRow(
                itemName,
                $"[{levelColor}]{recipe.RequiredLevel}[/]",
                recipe.BarCount.ToString(),
                recipe.Experience.ToString()
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine(
            $"\n[dim]Use 'skills smith {barName.Split(' ')[0].ToLower()} <item>' to smith an item.[/]");
        AnsiConsole.MarkupLine("[dim]Use '-c <count>' to smith multiple items.[/]");

        return (int)ExitCode.Success;
    }

    private int SmithItems(Player player, ItemId barId, SmithingRecipe recipe, int count)
    {
        var smithingSkill = player.GetSkill(SkillConstants.SmithingSkillName);
        var smithingLevel = smithingSkill.Level.Value;

        // Check level requirement
        if (smithingLevel < recipe.RequiredLevel)
        {
            AnsiConsole.MarkupLine($"[red]You need level {recipe.RequiredLevel} Smithing to smith this item.[/]");
            return (int)ExitCode.Failure;
        }

        // Get item references
        var resultItem = ItemRegistry.GetById(recipe.ResultItemId);
        var barItem = ItemRegistry.GetById(barId);

        if (resultItem is null || barItem is null)
        {
            AnsiConsole.MarkupLine("[red]Something went wrong while smithing.[/]");
            return (int)ExitCode.Failure;
        }

        var itemsSmithed = 0;
        var totalXp = 0;

        for (var i = 0; i < count; i++)
        {
            // Check inventory space
            if (player.Inventory.IsFull)
            {
                if (itemsSmithed == 0)
                {
                    AnsiConsole.MarkupLine("[red]Your inventory is full.[/]");
                    return (int)ExitCode.Failure;
                }

                AnsiConsole.MarkupLine("[yellow]Your inventory is full.[/]");
                break;
            }

            // Check for bars
            var barCount = CountItem(player, barId);
            if (barCount < recipe.BarCount)
            {
                if (itemsSmithed == 0)
                {
                    AnsiConsole.MarkupLine(
                        $"[red]You need {recipe.BarCount}x {barItem.Name} but only have {barCount}.[/]");
                    return (int)ExitCode.Failure;
                }

                AnsiConsole.MarkupLine($"[yellow]You ran out of {barItem.Name}.[/]");
                break;
            }

            // Remove bars
            RemoveItem(player, barId, recipe.BarCount);

            // Add item to inventory
            player.Inventory.TryAdd(resultItem);

            // Grant experience
            Player.AddExperience(smithingSkill, recipe.Experience);
            itemsSmithed++;
            totalXp += recipe.Experience;
        }

        if (itemsSmithed == 1)
        {
            AnsiConsole.MarkupLine($"[green]You hammer the bars and create a {resultItem.Name}.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]You hammer the bars and create {itemsSmithed}x {resultItem.Name}.[/]");
        }

        AnsiConsole.MarkupLine($"[dim]You gain {totalXp:N0} Smithing experience.[/]");

        gameState.Save();
        return (int)ExitCode.Success;
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