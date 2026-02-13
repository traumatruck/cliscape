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
///     Mine rocks in the current location for ore.
/// </summary>
[Description("Mine rocks in your current location")]
public class MineCommand(GameState gameState, MiningService miningService) : Command<MineCommandSettings>, ICommand
{
    public static string CommandName => "mine";

    public override int Execute(CommandContext context, MineCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
        var location = player.CurrentLocation;

        // If no rock specified, list available rocks
        if (string.IsNullOrWhiteSpace(settings.RockType))
        {
            return ListRocks(location.MiningRocks, player);
        }

        // Find the matching rock
        var rock = FindRock(location.MiningRocks, settings.RockType);
        if (rock is null)
        {
            AnsiConsole.MarkupLine($"[red]There is no '{settings.RockType}' rock here.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'skills mine' without arguments to see available rocks.[/]");
            return (int)ExitCode.Failure;
        }

        var requestedCount = settings.Count ?? 1;
        var maxCount = Math.Min(requestedCount, rock.MaxActions);

        if (requestedCount > rock.MaxActions)
        {
            AnsiConsole.MarkupLine(
                $"[dim]Note: {rock.Name} can only be mined {rock.MaxActions} time(s) before it depletes.[/]");
        }

        // Validate via service
        var canMineResult = miningService.CanMine(player, rock);
        if (!canMineResult.Success)
        {
            AnsiConsole.MarkupLine($"[red]{canMineResult.Message}[/]");
            return (int)ExitCode.Failure;
        }

        // Execute via service
        var result = miningService.Mine(player, rock, maxCount, ItemRegistry.GetById);

        if (!result.Success)
        {
            AnsiConsole.MarkupLine($"[red]{result.Message}[/]");
            return (int)ExitCode.Failure;
        }

        // Display result
        if (result.OresObtained == 1)
        {
            AnsiConsole.MarkupLine(
                $"[green]You swing your {canMineResult.Value} at the rock and get some {result.OreName}.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine(
                $"[green]You swing your {canMineResult.Value} at the rock and get {result.OresObtained}x {result.OreName}.[/]");
        }

        AnsiConsole.MarkupLine($"[dim]You gain {result.TotalExperience:N0} Mining experience.[/]");

        if (result.LevelUp is { } levelUp)
        {
            AnsiConsole.MarkupLine(
                $"[bold yellow]Congratulations! Your Mining level is now {levelUp.NewLevel}![/]");
        }

        return (int)ExitCode.Success;
    }

    private static int ListRocks(IReadOnlyList<IMiningRock> rocks, Player player)
    {
        if (rocks.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no rocks here to mine.[/]");
            return (int)ExitCode.Success;
        }

        var miningLevel = player.GetSkillLevel(SkillConstants.MiningSkillName).Value;

        AnsiConsole.MarkupLine("[bold]Rocks at this location:[/]\n");

        var table = new Table()
            .AddColumn("Rock")
            .AddColumn("Req. Level")
            .AddColumn("Experience")
            .AddColumn("Required Pickaxe")
            .AddColumn("Ore");

        foreach (var rock in rocks)
        {
            var levelColor = miningLevel >= rock.RequiredLevel ? "green" : "red";
            var oreName = GetItemName(rock.OreItemId);

            table.AddRow(
                rock.Name,
                $"[{levelColor}]{rock.RequiredLevel}[/]",
                rock.Experience.ToString(),
                rock.RequiredPickaxe.ToString(),
                oreName
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[dim]Use 'skills mine <rock>' to mine a rock (e.g., 'skills mine copper').[/]");
        AnsiConsole.MarkupLine("[dim]Use 'skills mine <rock> -c <count>' to mine multiple times.[/]");

        return (int)ExitCode.Success;
    }

    private static IMiningRock? FindRock(IReadOnlyList<IMiningRock> rocks, string rockType)
    {
        return rocks.FirstOrDefault(r =>
            r.RockType.ToString().Equals(rockType, StringComparison.OrdinalIgnoreCase) ||
            r.Name.Contains(rockType, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetItemName(ItemId id)
    {
        var item = ItemRegistry.GetById(id);
        return item?.Name.Value ?? "Unknown";
    }
}