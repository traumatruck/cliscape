using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game;
using CliScape.Game.Skills;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Fish at fishing spots in the current location.
/// </summary>
[Description("Fish at fishing spots in your current location")]
public class FishCommand(GameState gameState, FishingService fishingService) : Command<FishCommandSettings>, ICommand
{
    public static string CommandName => "fish";

    public override int Execute(CommandContext context, FishCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
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

        // Validate via service
        var (canFish, errorMessage) = fishingService.CanFish(player, spot);
        if (!canFish)
        {
            AnsiConsole.MarkupLine($"[red]{errorMessage}[/]");
            return (int)ExitCode.Failure;
        }

        // Execute via service
        var result = fishingService.Fish(player, spot, maxCount, ItemRegistry.GetById);

        if (!result.Success)
        {
            AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            return (int)ExitCode.Failure;
        }

        // Display catches
        var catchSummary = string.Join(", ", result.FishCaught.Select(kvp =>
            kvp.Value == 1 ? kvp.Key : $"{kvp.Value}x {kvp.Key}"));

        AnsiConsole.MarkupLine($"[green]You catch: {catchSummary}![/]");
        AnsiConsole.MarkupLine($"[dim]You gain {result.TotalExperience:N0} Fishing experience.[/]");

        if (result.LevelUp is { } levelUp)
        {
            AnsiConsole.MarkupLine(
                $"[bold yellow]Congratulations! Your Fishing level is now {levelUp.NewLevel}![/]");
        }

        return (int)ExitCode.Success;
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
            var toolName = GetItemName(spot.RequiredTool);
            var catches = string.Join(", ", spot.PossibleCatches.Select(c => GetItemName(c.FishItemId)));

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

    private static IFishingSpot? FindFishingSpot(IReadOnlyList<IFishingSpot> spots, string spotType)
    {
        return spots.FirstOrDefault(s =>
            s.SpotType.ToString().Equals(spotType, StringComparison.OrdinalIgnoreCase) ||
            s.Name.Contains(spotType, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetItemName(ItemId itemId)
    {
        var item = ItemRegistry.GetById(itemId);
        return item?.Name.Value ?? "unknown";
    }
}