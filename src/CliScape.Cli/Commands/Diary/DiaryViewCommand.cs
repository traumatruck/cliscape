using CliScape.Content.Achievements;
using CliScape.Core.Achievements;
using CliScape.Core.World;
using CliScape.Game;
using CliScape.Game.Achievements;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Diary;

/// <summary>
///     Command to view achievements for a specific diary location.
/// </summary>
public sealed class DiaryViewCommand : Command<DiaryViewCommandSettings>
{
    public const string CommandName = "view";

    public override int Execute(CommandContext context, DiaryViewCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = GameState.Instance.GetPlayer();
        var diaryService = DiaryService.Instance;
        var rewardService = DiaryRewardService.Instance;

        // Check for any newly completed achievements
        diaryService.CheckAllAchievements(player);

        var location = new LocationName(settings.Location);
        var diary = DiaryRegistry.Instance.GetDiary(location);

        if (diary == null)
        {
            AnsiConsole.MarkupLine($"[red]No diary found for location '{settings.Location}'.[/]");
            return (int)ExitCode.Failure;
        }

        var progress = player.DiaryProgress.GetProgress(location);

        AnsiConsole.MarkupLine($"[bold cyan]{diary.Location.Value} Achievement Diary[/]");
        AnsiConsole.WriteLine();

        // Display achievements grouped by tier
        foreach (var tier in Enum.GetValues<DiaryTier>())
        {
            var tierAchievements = diary.GetAchievementsForTier(tier).ToList();

            if (tierAchievements.Count == 0)
            {
                continue;
            }

            var completedCount = progress.GetCompletedCountForTier(diary, tier);
            var isTierComplete = progress.IsTierComplete(diary, tier);

            var tierColor = isTierComplete ? "green" : "yellow";
            AnsiConsole.MarkupLine($"[bold {tierColor}]{tier} Tier[/] ({completedCount}/{tierAchievements.Count})");

            foreach (var achievement in tierAchievements)
            {
                var isCompleted = progress.IsCompleted(achievement.Id);
                var statusIcon = isCompleted ? "[green]✓[/]" : "[grey]○[/]";
                var nameColor = isCompleted ? "green" : "white";

                AnsiConsole.MarkupLine($"  {statusIcon} [{nameColor}]{achievement.Name}[/]");
                AnsiConsole.MarkupLine($"     [grey]{achievement.Description}[/]");
            }

            // Show rewards if tier is complete
            if (isTierComplete)
            {
                var rewards = rewardService.GetRewards(location, tier);
                if (rewards.Length > 0)
                {
                    var rewardKey = $"{location.Value}_{tier}";
                    var alreadyClaimed = player.ClaimedDiaryRewards.Contains(rewardKey);

                    if (alreadyClaimed)
                    {
                        AnsiConsole.MarkupLine("  [grey]Rewards (already claimed):[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("  [green]Rewards (available to claim):[/]");
                    }

                    foreach (var reward in rewards)
                    {
                        AnsiConsole.MarkupLine($"    • {reward.Description}");
                    }
                }
            }

            AnsiConsole.WriteLine();
        }

        GameState.Instance.Save();

        return (int)ExitCode.Success;
    }
}