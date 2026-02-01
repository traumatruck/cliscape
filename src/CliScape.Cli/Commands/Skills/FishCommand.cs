using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Fish at fishing spots in the current location.
/// </summary>
[Description("Fish at fishing spots in your current location")]
public class FishCommand : Command<FishCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "fish";

    public override int Execute(CommandContext context, FishCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;

        // If no spot specified, list available fishing spots
        if (string.IsNullOrWhiteSpace(settings.SpotType))
        {
            return ListFishingSpots(location.FishingSpots, player);
        }

        // Find the matching fishing spot
        var spot = FindFishingSpot(location.FishingSpots, settings.SpotType);
        if (spot is null)
        {
            AnsiConsole.MarkupLine($"[red]There is no '{settings.SpotType}' fishing spot here.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'skills fish' without arguments to see available spots.[/]");
            return (int)ExitCode.Failure;
        }

        var requestedCount = settings.Count ?? 1;
        var maxCount = Math.Min(requestedCount, spot.MaxActions);

        if (requestedCount > spot.MaxActions)
        {
            AnsiConsole.MarkupLine(
                $"[dim]Note: This fishing spot only allows {spot.MaxActions} catches before the spot moves.[/]");
        }

        return CatchFishMultiple(player, spot, maxCount);
    }

    private static int ListFishingSpots(IReadOnlyList<IFishingSpot> spots, Player player)
    {
        if (spots.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no fishing spots here.[/]");
            return (int)ExitCode.Success;
        }

        var fishingLevel = player.GetSkillLevel(SkillConstants.FishingSkillName).Value;

        AnsiConsole.MarkupLine("[bold]Fishing spots at this location:[/]\n");

        var table = new Table()
            .AddColumn("Spot")
            .AddColumn("Type")
            .AddColumn("Req. Level")
            .AddColumn("Tool")
            .AddColumn("Catches");

        foreach (var spot in spots)
        {
            var levelColor = fishingLevel >= spot.RequiredLevel ? "green" : "red";
            var toolName = GetToolName(spot.RequiredTool);
            var catches = string.Join(", ", spot.PossibleCatches.Select(c => GetFishName(c.FishItemId)));

            table.AddRow(
                spot.Name,
                spot.SpotType.ToString(),
                $"[{levelColor}]{spot.RequiredLevel}[/]",
                toolName,
                catches
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[dim]Use 'skills fish <type>' to fish at a spot (e.g., 'skills fish net').[/]");
        AnsiConsole.MarkupLine("[dim]Use 'skills fish <type> -c <count>' to fish multiple times.[/]");

        return (int)ExitCode.Success;
    }

    private int CatchFishMultiple(Player player, IFishingSpot spot, int count)
    {
        var fishingSkill = player.GetSkill(SkillConstants.FishingSkillName);
        var fishingLevel = fishingSkill.Level.Value;

        // Check level requirement
        if (fishingLevel < spot.RequiredLevel)
        {
            AnsiConsole.MarkupLine($"[red]You need level {spot.RequiredLevel} Fishing to fish here.[/]");
            return (int)ExitCode.Failure;
        }

        // Check for required tool
        if (!HasTool(player, spot.RequiredTool))
        {
            var toolName = GetToolName(spot.RequiredTool);
            AnsiConsole.MarkupLine($"[red]You need a {toolName} to fish here.[/]");
            return (int)ExitCode.Failure;
        }

        // Determine which fish can be caught based on level
        var possibleCatches = spot.PossibleCatches
            .Where(c => fishingLevel >= c.RequiredLevel)
            .ToList();

        if (possibleCatches.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]You can't catch anything at this spot yet.[/]");
            return (int)ExitCode.Failure;
        }

        var fishCaught = new Dictionary<string, int>();
        var totalXp = 0;

        for (var i = 0; i < count; i++)
        {
            // Check inventory space
            if (player.Inventory.IsFull)
            {
                if (fishCaught.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]Your inventory is full.[/]");
                    return (int)ExitCode.Failure;
                }

                AnsiConsole.MarkupLine("[yellow]Your inventory is full.[/]");
                break;
            }

            // Pick a random fish
            var caughtFish = possibleCatches[Random.Shared.Next(possibleCatches.Count)];

            // Get the fish item
            var fishItem = ItemRegistry.GetById(caughtFish.FishItemId);
            if (fishItem is null)
            {
                continue;
            }

            // Add to inventory
            if (!player.Inventory.TryAdd(fishItem))
            {
                break;
            }

            // Grant experience
            Player.AddExperience(fishingSkill, caughtFish.Experience);
            totalXp += caughtFish.Experience;

            // Track catch count by fish name
            var fishName = fishItem.Name.Value;
            if (!fishCaught.TryAdd(fishName, 1))
            {
                fishCaught[fishName]++;
            }
        }

        if (fishCaught.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]You didn't catch anything.[/]");
            return (int)ExitCode.Failure;
        }

        // Display catches
        var catchSummary = string.Join(", ", fishCaught.Select(kvp =>
            kvp.Value == 1 ? kvp.Key : $"{kvp.Value}x {kvp.Key}"));

        AnsiConsole.MarkupLine($"[green]You catch: {catchSummary}![/]");
        AnsiConsole.MarkupLine($"[dim]You gain {totalXp:N0} Fishing experience.[/]");

        _gameState.Save();
        return (int)ExitCode.Success;
    }

    private static IFishingSpot? FindFishingSpot(IReadOnlyList<IFishingSpot> spots, string spotType)
    {
        // Match by spot type name
        return spots.FirstOrDefault(s =>
            s.SpotType.ToString().Equals(spotType, StringComparison.OrdinalIgnoreCase) ||
            s.Name.Contains(spotType, StringComparison.OrdinalIgnoreCase));
    }

    private static bool HasTool(Player player, ItemId toolId)
    {
        // Check inventory
        var hasInInventory = player.Inventory.GetItems()
            .Any(slot => slot.Item.Id == toolId);

        if (hasInInventory)
        {
            return true;
        }

        // Check equipped items
        return player.Equipment.GetAllEquipped()
            .Any(item => item.Id == toolId);
    }

    private static string GetToolName(ItemId toolId)
    {
        var item = ItemRegistry.GetById(toolId);
        return item?.Name.Value ?? "unknown tool";
    }

    private static string GetFishName(ItemId fishId)
    {
        var item = ItemRegistry.GetById(fishId);
        return item?.Name.Value ?? "unknown fish";
    }
}