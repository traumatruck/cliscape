using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Steal from stalls or pickpocket NPCs.
/// </summary>
[Description("Steal from stalls or pickpocket NPCs")]
public class ThieveCommand : Command<ThieveCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "thieve";

    public override int Execute(CommandContext context, ThieveCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Check if there are thieving targets here
        if (location.ThievingTargets.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]There is nothing to steal here.[/]");
            return (int)ExitCode.Failure;
        }

        // If no target specified, list available targets
        if (string.IsNullOrWhiteSpace(settings.Target))
        {
            return ListThievingTargets(location.ThievingTargets, player);
        }

        // Find the matching target
        var target = FindTarget(location.ThievingTargets, settings.Target);
        if (target is null)
        {
            AnsiConsole.MarkupLine($"[red]There is no '{settings.Target}' to steal from here.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'skills thieve' without arguments to see available targets.[/]");
            return (int)ExitCode.Failure;
        }

        var requestedCount = settings.Count ?? 1;
        return AttemptThieving(player, target, requestedCount);
    }

    private static int ListThievingTargets(IReadOnlyList<IThievingTarget> targets, Player player)
    {
        var thievingLevel = player.GetSkillLevel(SkillConstants.ThievingSkillName).Value;

        AnsiConsole.MarkupLine("[bold]Thieving targets at this location:[/]\n");

        var table = new Table()
            .AddColumn("Target")
            .AddColumn("Type")
            .AddColumn("Req. Level")
            .AddColumn("Experience")
            .AddColumn("Success Rate");

        foreach (var target in targets)
        {
            var levelColor = thievingLevel >= target.RequiredLevel ? "green" : "red";
            var successChance = ThievingHelper.CalculateSuccessChance(target, thievingLevel);
            var successPercent = thievingLevel >= target.RequiredLevel
                ? $"{successChance * 100:F0}%"
                : "N/A";

            table.AddRow(
                target.Name,
                target.TargetType.ToString(),
                $"[{levelColor}]{target.RequiredLevel}[/]",
                target.Experience.ToString(),
                successPercent
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[dim]Use 'skills thieve <target>' to steal (e.g., 'skills thieve bakery').[/]");
        AnsiConsole.MarkupLine("[dim]Use 'skills thieve <target> -c <count>' to attempt multiple times.[/]");
        AnsiConsole.MarkupLine("[dim yellow]Warning: Failing to steal will damage you![/]");

        return (int)ExitCode.Success;
    }

    private int AttemptThieving(Player player, IThievingTarget target, int count)
    {
        var thievingSkill = player.GetSkill(SkillConstants.ThievingSkillName);
        var thievingLevel = thievingSkill.Level.Value;

        // Check level requirement
        if (thievingLevel < target.RequiredLevel)
        {
            AnsiConsole.MarkupLine($"[red]You need level {target.RequiredLevel} Thieving to steal from this.[/]");
            return (int)ExitCode.Failure;
        }

        var successCount = 0;
        var failCount = 0;
        var totalXp = 0;
        var totalDamage = 0;
        var lootGained = new Dictionary<string, int>();

        var successChance = ThievingHelper.CalculateSuccessChance(target, thievingLevel);

        for (var i = 0; i < count; i++)
        {
            // Check if player is still alive
            if (player.CurrentHealth <= 0)
            {
                AnsiConsole.MarkupLine("[red]You have been knocked out![/]");
                break;
            }

            // Check inventory space
            if (player.Inventory.IsFull)
            {
                if (successCount == 0)
                {
                    AnsiConsole.MarkupLine("[red]Your inventory is full.[/]");
                    return (int)ExitCode.Failure;
                }

                AnsiConsole.MarkupLine("[yellow]Your inventory is full.[/]");
                break;
            }

            // Attempt to steal
            if (ThievingHelper.IsSuccessful(successChance))
            {
                // Success! Get loot
                var (lootItemId, quantity) = ThievingHelper.SelectLoot(target);
                var lootItem = ItemRegistry.GetById(lootItemId);

                if (lootItem is not null)
                {
                    // Add loot to inventory
                    for (var j = 0; j < quantity; j++)
                    {
                        if (!player.Inventory.TryAdd(lootItem))
                        {
                            break;
                        }
                    }

                    // Track loot
                    var lootName = lootItem.Name.Value;
                    if (!lootGained.TryAdd(lootName, quantity))
                    {
                        lootGained[lootName] += quantity;
                    }
                }

                // Grant experience
                Player.AddExperience(thievingSkill, target.Experience);
                successCount++;
                totalXp += target.Experience;
            }
            else
            {
                // Failed! Take damage
                player.TakeDamage(target.FailureDamage);
                failCount++;
                totalDamage += target.FailureDamage;
            }
        }

        // Display results
        if (successCount > 0)
        {
            var lootSummary = string.Join(", ", lootGained.Select(kvp =>
                kvp.Value == 1 ? kvp.Key : $"{kvp.Value}x {kvp.Key}"));

            AnsiConsole.MarkupLine($"[green]Successfully stole {successCount} time(s): {lootSummary}[/]");
            AnsiConsole.MarkupLine($"[dim]You gain {totalXp:N0} Thieving experience.[/]");
        }

        if (failCount > 0)
        {
            AnsiConsole.MarkupLine($"[red]You were caught {failCount} time(s) and took {totalDamage} damage.[/]");
            AnsiConsole.MarkupLine($"[dim]Current health: {player.CurrentHealth}/{player.MaximumHealth}[/]");
        }

        if (successCount == 0 && failCount == 0)
        {
            AnsiConsole.MarkupLine("[red]You didn't steal anything.[/]");
            return (int)ExitCode.Failure;
        }

        _gameState.Save();
        return (int)ExitCode.Success;
    }

    private static IThievingTarget? FindTarget(IReadOnlyList<IThievingTarget> targets, string targetName)
    {
        return targets.FirstOrDefault(t =>
            t.Name.Contains(targetName, StringComparison.OrdinalIgnoreCase) ||
            t.TargetType.ToString().Equals(targetName, StringComparison.OrdinalIgnoreCase));
    }
}