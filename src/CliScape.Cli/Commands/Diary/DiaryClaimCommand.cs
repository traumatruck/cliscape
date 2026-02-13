using CliScape.Content.Achievements;
using CliScape.Core.Achievements;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;
using CliScape.Game;
using CliScape.Game.Achievements;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Diary;

/// <summary>
///     Command to claim rewards for a completed diary tier.
/// </summary>
public sealed class DiaryClaimCommand(
    GameState gameState,
    DiaryRewardService diaryRewardService,
    DiaryRegistry diaryRegistry) : Command<DiaryClaimCommandSettings>
{
    public const string CommandName = "claim";

    public override int Execute(CommandContext context, DiaryClaimCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();

        var location = new LocationName(settings.Location);

        // Parse tier
        if (!Enum.TryParse<DiaryTier>(settings.Tier, true, out var tier))
        {
            AnsiConsole.MarkupLine(
                $"[red]Invalid tier '{settings.Tier}'. Valid tiers are: easy, medium, hard, elite[/]");
            return (int)ExitCode.Failure;
        }

        // Check if diary exists
        var diary = diaryRegistry.GetDiary(location);
        if (diary == null)
        {
            AnsiConsole.MarkupLine($"[red]No diary found for location '{settings.Location}'.[/]");
            return (int)ExitCode.Failure;
        }

        // Check if can claim
        if (!diaryRewardService.CanClaimRewards(player, location, tier, out var failureReason))
        {
            AnsiConsole.MarkupLine($"[red]{failureReason}[/]");
            return (int)ExitCode.Failure;
        }

        // Handle XP lamp skill selection
        SkillName? lampSkill = null;

        if (diaryRewardService.RequiresSkillSelection(location, tier))
        {
            // Get valid skills for the lamp
            var validSkills = diaryRewardService.GetValidLampSkills(location, tier).ToList();

            if (!string.IsNullOrEmpty(settings.Skill))
            {
                // Skill provided via command line
                lampSkill = validSkills.FirstOrDefault(s =>
                    s.Name.Equals(settings.Skill, StringComparison.OrdinalIgnoreCase));

                if (lampSkill == null)
                {
                    AnsiConsole.MarkupLine($"[red]Invalid skill '{settings.Skill}'.[/]");
                    return (int)ExitCode.Failure;
                }
            }
            else
            {
                // Prompt user to select skill

                if (validSkills.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No valid skills available for the experience lamp.[/]");
                    return (int)ExitCode.Failure;
                }

                var skillNames = validSkills.Select(s => s.Name).ToList();
                var selectedSkillName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[cyan]Choose a skill for your experience lamp:[/]")
                        .PageSize(10)
                        .AddChoices(skillNames)
                );

                lampSkill = validSkills.First(s => s.Name == selectedSkillName);
            }
        }

        // Claim rewards
        if (!diaryRewardService.ClaimRewards(player, location, tier, lampSkill, out var errorMessage))
        {
            AnsiConsole.MarkupLine($"[red]{errorMessage}[/]");
            return (int)ExitCode.Failure;
        }

        // Display success message with rewards
        AnsiConsole.MarkupLine($"[green]Successfully claimed rewards for {location.Value} {tier} tier![/]");
        AnsiConsole.WriteLine();

        var rewards = diaryRewardService.GetRewards(location, tier);
        AnsiConsole.MarkupLine("[bold]Rewards received:[/]");

        foreach (var reward in rewards)
        {
            switch (reward)
            {
                case ItemReward itemReward:
                    AnsiConsole.MarkupLine($"  • {itemReward.Description}");
                    break;

                case ExperienceLampReward lampReward:
                    if (lampSkill != null)
                    {
                        AnsiConsole.MarkupLine($"  • {lampReward.ExperienceAmount.Value} XP in {lampSkill.Name}");
                    }

                    break;
            }
        }

        return (int)ExitCode.Success;
    }
}